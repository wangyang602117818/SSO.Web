using Quartz;
using Quartz.Impl;
using SSO.Business;
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
        static void Main(string[] args)
        {

            //new Processor().StartWork();
            Console.WriteLine("ok");
            Console.ReadKey();
        }

    }
    public class C
    {
        public string name { get; set; }
        public List<string> values { get; set; }
    }
}
