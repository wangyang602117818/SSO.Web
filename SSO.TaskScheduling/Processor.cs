using Quartz;
using Quartz.Impl;
using SSO.Business;
using SSO.Model;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.TaskScheduling
{
    public class Processor
    {
        static IScheduler scheduler = new StdSchedulerFactory().GetScheduler().Result;
        Task task = null;
        Business.TaskScheduling taskScheduling = new Business.TaskScheduling();
        Business.TaskTrigger taskTrigger = new TaskTrigger();
        public void StartWork()
        {
            task = Task.Factory.StartNew(() =>
            {
                MsQueue<SchedulingQueueModel> msQueue = new MsQueue<SchedulingQueueModel>(AppSettings.GetValue("task_scheduling_msqueue"));
                msQueue.ReceiveMessage(Worker);
            });
            int count = 0;
            var schedulings = taskScheduling.GetTaskSchedulings((int)SchedulingStateEnum.Running, ref count);

            scheduler.Start();

            foreach (var scheduling in schedulings)
            {
                var dict = GetTriggersAndJobs(scheduling);
                if (dict.Count > 0) scheduler.ScheduleJobs(dict, true);
            }
        }
        public async Task EndWork()
        {
            await scheduler.Shutdown(true);
        }
        /// <summary>
        /// message queue中发过来的命令
        /// </summary>
        /// <param name="obj"></param>
        private void Worker(SchedulingQueueModel obj)
        {
            if (obj.SchedulingState == SchedulingStateEnum.Stoped)
            {
                if (obj.SchedulingId > 0) StopJob(obj.SchedulingId);
                if (obj.TriggerId > 0) StopJobByTrigger(obj.TriggerId);
            }
            else
            {
                if (obj.SchedulingId > 0) StartJob(obj.SchedulingId);
            }
        }
        Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>> GetTriggersAndJobs(Data.Models.TaskScheduling scheduling)
        {
            IJobDetail jobDetail = JobBuilder.Create<SchedulingJob>()
                  .WithIdentity("job-" + scheduling.Id)
                  .UsingJobData("data", JsonSerializerHelper.Serialize(scheduling))
                  .Build();
            //触发器
            var triggers = taskTrigger.GetTaskTriggers(scheduling.Id);
            Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>> dictionary = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();
            HashSet<ITrigger> triggerSet = new HashSet<ITrigger>();
            foreach (var trigger in triggers)
            {
                if (trigger.Expire != null && trigger.Expire < DateTime.Now) continue;
                var tri = TriggerBuilder.Create().WithIdentity("trigger-" + scheduling.Id + "-" + trigger.Id);
                if (trigger.Activate == null)
                {
                    tri.StartNow();
                }
                else
                {
                    DateTimeOffset? nextRunTime;
                    if (trigger.Activate < DateTimeOffset.Now)
                    {
                        nextRunTime = new CronExpression(trigger.Crons).GetNextValidTimeAfter(DateTimeOffset.Now);
                    }
                    else
                    {
                        nextRunTime = trigger.Activate;
                    }
                    if (nextRunTime == null) continue;
                    tri.StartAt(nextRunTime.Value);
                }
                tri.WithCronSchedule(trigger.Crons);
                if (trigger.Expire != null) tri.EndAt(trigger.Expire);
                triggerSet.Add(tri.Build());
            }
            if (triggerSet.Count > 0)
            {
                dictionary.Add(jobDetail, triggerSet);
            }
            return dictionary;
        }

        void StartJob(int schedulingId)
        {
            Log4Net.InfoLog("start job by schedulingId=" + schedulingId);
            var scheduling = taskScheduling.GetById(schedulingId);
            if (scheduling == null) return;
            var dict = GetTriggersAndJobs(scheduling);
            if (dict.Count > 0) scheduler.ScheduleJobs(dict, true);
        }
        void StopJob(int schedulingId)
        {
            Log4Net.InfoLog("stop job by schedulingId=" + schedulingId);
            JobKey jobKey = new JobKey("job-" + schedulingId);
            scheduler.PauseJob(jobKey);
        }
        void StopJobByTrigger(int triggerId)
        {
            Log4Net.InfoLog("stop job by triggerId=" + triggerId);
            var list = new TaskSchedulingTriggerMapping().GetByTriggerId(triggerId);
            foreach (var item in list)
            {
                JobKey jobKey = new JobKey("job-" + item.SchedulingId);
                scheduler.PauseJob(jobKey);
            }
        }
    }
}
