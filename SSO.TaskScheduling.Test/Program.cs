using Quartz;
using Quartz.Impl;
using SSO.Business;
using SSO.Data.Models;
using SSO.Model;
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
        static IScheduler scheduler = new StdSchedulerFactory().GetScheduler().Result;
        static Task companyTask = null;
        static Task roleTask = null;
        static void Main(string[] args)
        {
            companyTask = new Task(MonitorCompany);
            companyTask.Start();

            roleTask = new Task(MonitorRole);
            roleTask.Start();
            //new Processor().StartWork();
            Console.WriteLine("ok");
            Console.ReadKey();
        }

        static void MonitorCompany()
        {
            while (true)
            {
                MonitorTableData monitorTableData = new MonitorTableData();
                monitorTableData.Monitor("company");
                Thread.Sleep(1000);
            }
        }
        static void MonitorRole()
        {
            while (true)
            {
                MonitorTableData monitorTableData = new MonitorTableData();
                monitorTableData.Monitor("role");
                Thread.Sleep(1000);
            }
        }
    }
}
