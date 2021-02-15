using Quartz;
using Quartz.Impl;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.TaskScheduling.Test
{
    class Program
    {
        static StdSchedulerFactory factory = new StdSchedulerFactory();
        static IScheduler scheduler = factory.GetScheduler().Result;
        static Task task = null;
        static Dictionary<int, IScheduler> schedulers = new Dictionary<int, IScheduler>();

        public static object M { get; private set; }

        static void Main(string[] args)
        {
            

            var exprss = "0 0 20 14 2 ? 2021";
            bool valid = CronExpression.IsValidExpression(exprss);
            CronExpression cronExpression = new CronExpression(exprss);
         
            var desc = cronExpression.GetExpressionSummary();
            var time01 = cronExpression.GetNextValidTimeAfter(DateTimeOffset.Now);
            //var t01 = test.StartNow().WithCronSchedule("0 19 16 14 2 ? *").Build();
            //var next01 = t01.GetNextFireTimeUtc();
            //var next02 = t01.GetPreviousFireTimeUtc();
            //MsQueue<FileConvertModel> msQueue = new MsQueue<FileConvertModel>(AppSettings.GetValue("task_scheduling_msqueue"));
            //msQueue.CreateQueue();
            int count = 0;
            var schedulings = new Business.TaskScheduling().GetTaskSchedulings(0, ref count);

            scheduler.Start();
            
            foreach (var scheduling in schedulings)
            {
                IJobDetail jobDetail = JobBuilder.Create<HelloJob>()
                   .WithIdentity("job" + scheduling.Id)
                   .UsingJobData("data", JsonSerializerHelper.Serialize(scheduling))
                   .Build();

                //添加触发器
                var triggers = new Business.TaskTrigger().GetTaskTriggers(scheduling.Id);

                Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>> dictionary = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();
                HashSet<ITrigger> triggerSet = new HashSet<ITrigger>();
                foreach (var trigger in triggers)
                {
                    var tri = TriggerBuilder.Create().WithIdentity("trigger" + trigger.Id);
                    if (trigger.Activate == null)
                    {
                        tri.StartNow();
                    }
                    else
                    {
                        if (trigger.Activate < trigger.CreateTime) continue;
                        tri.StartAt(trigger.Activate.Value);
                    }
                    tri.WithCronSchedule(trigger.Crons);
                    if (trigger.Expire != null)
                    {
                        if (trigger.Expire < trigger.CreateTime) continue;
                        tri.EndAt(trigger.Expire);
                    }
                    triggerSet.Add(tri.Build());
                }
                dictionary.Add(jobDetail, triggerSet);
                scheduler.ScheduleJobs(dictionary, true);
                
            }

            //Thread.Sleep(5000);

            //scheduler.PauseJob(new JobKey("job16"));

            //var job16 = scheduler.GetJobDetail(new JobKey("job16")).Result;
            //var job17 = scheduler.GetJobDetail(new JobKey("job17")).Result;

            //TriggerKey triggerKey1 = new TriggerKey("trigger1");
            //ICronTrigger cronTrigger1 = (ICronTrigger)scheduler.GetTrigger(triggerKey1).Result;
            //cronTrigger1.CronExpressionString = "0/1 * * * * ?";
            //scheduler.RescheduleJob(triggerKey1, cronTrigger1);

            //TriggerKey triggerKey2 = new TriggerKey("trigger2");
            //ICronTrigger cronTrigger2 = (ICronTrigger)scheduler.GetTrigger(triggerKey2).Result;
            //cronTrigger2.CronExpressionString = "0/1 * * * * ?";
            //scheduler.RescheduleJob(triggerKey2, cronTrigger2);

            // Trigger the job to run now, and then repeat every 10 seconds
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("trigger1")
            //    .StartNow()

            //    .WithCronSchedule("0/3 * * * * ?")
            //    .Build();
            //scheduler.ScheduleJob(job, trigger);


            //Thread.Sleep(10000);
            //TriggerKey triggerKey = new TriggerKey("trigger1");
            //ICronTrigger cronTrigger = (ICronTrigger)scheduler.GetTrigger(triggerKey).Result;
            //cronTrigger.CronExpressionString = "0/1 * * * * ?";

            //scheduler.RescheduleJob(triggerKey, cronTrigger);
            //Thread.Sleep(10000);

            //scheduler.Shutdown(true);

            Console.ReadKey();
        }
    }
}
