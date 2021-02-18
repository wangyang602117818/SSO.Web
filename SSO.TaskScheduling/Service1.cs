using Quartz;
using Quartz.Impl;
using SSO.Model;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SSO.TaskScheduling
{
    public partial class Service1 : ServiceBase
    {
        public Processor processor = new Processor();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log4Net.InfoLog("start...");
            processor.StartWork();
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            processor.EndWork().Wait();
            Log4Net.InfoLog("end...");
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            processor.EndWork().Wait();
            Log4Net.InfoLog("Shutdown...");
            base.OnShutdown();
        }
    }
}
