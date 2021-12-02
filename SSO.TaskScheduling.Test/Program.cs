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
        static Task tableTask = null;
        static Processor processor = new Processor();
        static void Main(string[] args)
        {
            //Console.WriteLine("请输入:");
            //string table = Console.ReadLine();
            //tableTask = new Task(MonitorTable, table);
            //tableTask.Start();

            //processor.StartWork();

           

            Console.WriteLine("ok");
            Console.ReadKey();
        }

        static void MonitorTable(object tablename)
        {
            while (true)
            {
                MonitorTableData monitorTableData = new MonitorTableData();
                monitorTableData.Monitor((string)tablename);
                Thread.Sleep(1000);
            }
        }
    }
}
