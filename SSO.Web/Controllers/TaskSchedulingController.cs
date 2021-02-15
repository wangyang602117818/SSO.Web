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
    [AllowAnonymous]
    public class TaskSchedulingController : Controller
    {
        TaskScheduling taskScheduling = new TaskScheduling();
        TaskTrigger taskTrigger = new TaskTrigger();
        public ActionResult Index()
        {
            //Data.Models.TaskScheduling taskScheduling = new Data.Models.TaskScheduling()
            //{
            //    Name = "每天备份1",
            //    Description = "备份1",
            //    Status = SchedulingTaskEnum.Ready,
            //    Api = "http://www.ssoapi.com:8030/TaskScheduling/m1"
            //};
            //int taskId = taskScheduling.InsertIdentity(taskScheduling);
            //Data.Models.TaskTrigger taskTrigger = new Data.Models.TaskTrigger()
            //{
            //    Type = "每天",
            //    Description = "每隔3秒运行",
            //    Crons = "0/5 * * * * ?"
            //};
            //int triggerId = taskTrigger.InsertIdentity(taskTrigger);
            //Data.Models.TaskSchedulingTriggerMapping taskSchedulingTriggerMapping = new Data.Models.TaskSchedulingTriggerMapping()
            //{
            //    TaskId = taskId,
            //    TriggerId = triggerId
            //};
            //taskSchedulingTriggerMapping.Insert(taskSchedulingTriggerMapping);
            return new ResponseModel<string>(ErrorCode.success, "");
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
                Description1 = searchValue,
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
        public ActionResult GetExamples(DateTime date, string crons, int count = 10)
        {
            CronExpression cronExpression = new CronExpression(crons);
            SchedulingExample schedulingExample = new SchedulingExample()
            {
                Examples = new List<DateTimeOffset>(),
                CronsDescriptions = new List<string>()
            };
            for (var i = 0; i < count; i++)
            {
                var timeAfter = schedulingExample.Examples.Count == 0 ? date : schedulingExample.Examples[schedulingExample.Examples.Count - 1];
                var nextRunTime = cronExpression.GetNextValidTimeAfter(timeAfter);
                if (nextRunTime != null)
                    schedulingExample.Examples.Add(nextRunTime.Value.LocalDateTime);
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