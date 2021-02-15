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
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log4Net.InfoLog("start...");

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Log4Net.InfoLog("end...");
            
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            Log4Net.InfoLog("Shutdown...");
            
            base.OnShutdown();
        }
    }
}
