using CronExpressionDescriptor;
using Quartz;
using SSO.Business;
using SSO.Model;
using SSO.Util.Client;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class TaskSchedulingController : BaseController
    {
        TaskScheduling taskScheduling = new TaskScheduling();
        TaskTrigger taskTrigger = new TaskTrigger();
        TaskSchedulingTriggerMapping taskSchedulingTriggerMapping = new TaskSchedulingTriggerMapping();
        protected MessageCenterService messageCenter = new MessageCenterService(messageBaseUrl);
        public ActionResult Index()
        {
            return new ResponseModel<string>(ErrorCode.success, "");
        }
        public ActionResult AddScheduling(SchedulingModel schedulingModel)
        {
            if (taskScheduling.GetByName(schedulingModel.Name) != null)
            {
                return new ResponseModel<string>(ErrorCode.record_exist, "");
            }
            Data.Models.TaskScheduling scheduling = new Data.Models.TaskScheduling()
            {
                Name = schedulingModel.Name,
                Description = schedulingModel.Description,
                Api = schedulingModel.Api,
                Status = SchedulingStateEnum.Stoped
            };
            if (schedulingModel.Trigger != null) schedulingModel.TriggerIds.Add(schedulingModel.Trigger.Value);
            List<DateTimeOffset> nextRunTimes = new List<DateTimeOffset>();
            var list = taskTrigger.GetByIds(schedulingModel.TriggerIds);
            foreach (var item in list)
            {
                var nextRunTime = new CronExpression(item.Crons).GetNextValidTimeAfter(DateTime.Now);
                nextRunTimes.Add(nextRunTime.Value);
            }
            nextRunTimes.Sort();
            scheduling.NextRunTime = nextRunTimes[0].LocalDateTime;
            int count = taskScheduling.InsertScheduling(new List<object>() { scheduling, new { triggerIds = schedulingModel.TriggerIds } });
            if (count > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            return new ResponseModel<string>(ErrorCode.server_exception, "");
        }
        public ActionResult UpdateScheduling(UpdateSchedulingModel updateSchedulingModel)
        {
            if (updateSchedulingModel.Trigger != null) updateSchedulingModel.TriggerIds.Add(updateSchedulingModel.Trigger.Value);
            Data.Models.TaskScheduling scheduling = new Data.Models.TaskScheduling()
            {
                Id = updateSchedulingModel.Id,
                Name = updateSchedulingModel.Name,
                Description = updateSchedulingModel.Description,
                Api = updateSchedulingModel.Api,
                UpdateTime = DateTime.Now,
                Status = SchedulingStateEnum.Stoped
            };
            List<DateTimeOffset> nextRunTimes = new List<DateTimeOffset>();
            var list = taskTrigger.GetByIds(updateSchedulingModel.TriggerIds);
            foreach (var item in list)
            {
                var nextRunTime = new CronExpression(item.Crons).GetNextValidTimeAfter(DateTime.Now);
                nextRunTimes.Add(nextRunTime.Value);
            }
            nextRunTimes.Sort();
            scheduling.NextRunTime = nextRunTimes[0].LocalDateTime;
            int count = taskScheduling.UpdateScheduling(scheduling, updateSchedulingModel.Id, updateSchedulingModel.TriggerIds);
            if (count > 0)
            {
                messageCenter.InsertTaskScheduling(Environment.MachineName, updateSchedulingModel.Id, 0, (int)SchedulingStateEnum.Stoped);
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            return new ResponseModel<string>(ErrorCode.server_exception, "");
        }
        public ActionResult GetSchedulingList(string searchValue = "", int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            Data.Models.TaskScheduling trigger = new Data.Models.TaskScheduling()
            {
                Description = searchValue,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = taskScheduling.GetPageList(ref count, trigger);
            return new ResponseModel<IEnumerable<Data.Models.TaskScheduling>>(ErrorCode.success, result, count);
        }
        public ActionResult EnableScheduling(int id, bool enable)
        {
            if (taskScheduling.EnableScheduling(id, enable) > 0)
            {
                if (!enable) messageCenter.InsertTaskScheduling(Environment.MachineName, id, 0, (int)SchedulingStateEnum.Stoped);
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            return new ResponseModel<string>(ErrorCode.server_exception, "");
        }
        public ActionResult GetSchedulingById(int id)
        {
            return new ResponseModel<object>(ErrorCode.success, taskScheduling.GetSchedulingById(id));
        }
        public ActionResult DeleteScheduling(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0) return new ResponseModel<int>(ErrorCode.success, 0);
            if (taskScheduling.CheckSchedulingRunning(ids) > 0) return new ResponseModel<int>(ErrorCode.task_is_running, 0);
            var count = taskScheduling.DeleteScheduling(ids);
            if (count == -1) return new ResponseModel<string>(ErrorCode.record_has_been_used, "");
            if (count > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult AddTrigger(TriggerModel triggerModel)
        {
            Data.Models.TaskTrigger trigger = new Data.Models.TaskTrigger()
            {
                Crons = triggerModel.Crons,
                Activate = triggerModel.Activate,
                Expire = triggerModel.Expire
            };
            trigger.Description = ExpressionDescriptor.GetDescription(triggerModel.Crons, new Options()
            {
                DayOfWeekStartIndexZero = false,
                Use24HourTimeFormat = true,
                Locale = "zh-CN"
            });
            trigger.Description1 = ExpressionDescriptor.GetDescription(triggerModel.Crons, new Options()
            {
                DayOfWeekStartIndexZero = false,
                Use24HourTimeFormat = true,
                Locale = "en"
            });
            if (taskTrigger.Insert(trigger) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult UpdateTrigger(UpdateTriggerModel updateTriggerModel)
        {
            Data.Models.TaskTrigger trigger = new Data.Models.TaskTrigger()
            {
                Id = updateTriggerModel.Id,
                Crons = updateTriggerModel.Crons,
                Activate = updateTriggerModel.Activate,
                Expire = updateTriggerModel.Expire,
                UpdateTime = DateTime.Now
            };
            trigger.Description = ExpressionDescriptor.GetDescription(updateTriggerModel.Crons, new Options()
            {
                DayOfWeekStartIndexZero = false,
                Use24HourTimeFormat = true,
                Locale = "zh-CN"
            });
            trigger.Description1 = ExpressionDescriptor.GetDescription(updateTriggerModel.Crons, new Options()
            {
                DayOfWeekStartIndexZero = false,
                Use24HourTimeFormat = true,
                Locale = "en"
            });
            if (taskTrigger.Update(trigger) > 0)
            {
                messageCenter.InsertTaskScheduling(Environment.MachineName, 0, updateTriggerModel.Id, (int)SchedulingStateEnum.Stoped);
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult DeleteTrigger(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0) return new ResponseModel<int>(ErrorCode.success, 0);
            if (taskSchedulingTriggerMapping.CheckTriggerExists(ids))
            {
                return new ResponseModel<int>(ErrorCode.record_has_been_used, 0);
            }
            var count = taskTrigger.Delete(ids);
            if (count == -1) return new ResponseModel<string>(ErrorCode.record_has_been_used, "");
            if (count > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult GetTriggerList(string searchValue = "", int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            Data.Models.TaskTrigger trigger = new Data.Models.TaskTrigger()
            {
                Description = searchValue,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = taskTrigger.GetPageList(ref count, trigger);
            return new ResponseModel<IEnumerable<Data.Models.TaskTrigger>>(ErrorCode.success, result, count);
        }
        public ActionResult GetTriggerById(int id)
        {
            return new ResponseModel<Data.Models.TaskTrigger>(ErrorCode.success, taskTrigger.GetById(id));
        }
        public ActionResult GetExamples(DateTime start, string crons, DateTime? end = null, int count = 10)
        {
            CronExpression cronExpression = new CronExpression(crons);
            SchedulingExample schedulingExample = new SchedulingExample()
            {
                Examples = new List<DateTimeOffset>(),
                CronsDescriptions = new List<string>()
            };
            if (start < end || end == null)
            {
                schedulingExample.Examples.Add(start);
                count--;
            }
            for (var i = 0; i < count; i++)
            {
                var timeAfter = schedulingExample.Examples.Count == 0 ? start : schedulingExample.Examples[schedulingExample.Examples.Count - 1];
                var nextRunTime = cronExpression.GetNextValidTimeAfter(timeAfter);
                if (nextRunTime != null)
                {
                    if (end == null || nextRunTime < end)
                        schedulingExample.Examples.Add(nextRunTime.Value.LocalDateTime);
                }
            }
            var descEn = ExpressionDescriptor.GetDescription(crons, new Options()
            {
                DayOfWeekStartIndexZero = false,
                Use24HourTimeFormat = true,
                Locale = "en"
            });
            var descZh = ExpressionDescriptor.GetDescription(crons, new Options()
            {
                DayOfWeekStartIndexZero = false,
                Use24HourTimeFormat = true,
                Locale = "zh-CN"
            });
            schedulingExample.CronsDescriptions.Add(descEn);
            schedulingExample.CronsDescriptions.Add(descZh);
            return new ResponseModel<SchedulingExample>(ErrorCode.success, schedulingExample);
        }
        public ActionResult StartScheduling(int id)
        {
            if (taskScheduling.UpdateStatus(id, (int)SchedulingStateEnum.Running) > 0)
            {
                messageCenter.InsertTaskScheduling(Environment.MachineName, id, 0, (int)SchedulingStateEnum.Running);
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult StopScheduling(int id)
        {
            if (taskScheduling.UpdateStatus(id, (int)SchedulingStateEnum.Stoped) > 0)
            {
                messageCenter.InsertTaskScheduling(Environment.MachineName, id, 0, (int)SchedulingStateEnum.Stoped);
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult Test()
        {

            return Content("k");
        }
        public ActionResult M()
        {
            return new ResponseModel<string>(ErrorCode.success, "m");
        }
        public ActionResult M1()
        {
            return new ResponseModel<string>(ErrorCode.success, "m1");
        }
    }
}