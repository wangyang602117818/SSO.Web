using Newtonsoft.Json.Linq;
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
using System.Security.Cryptography;
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

            Business.User user = new Business.User();

            var res = user.Login("cn01", "a123456".GetSha256());
            //processor.StartWork();



            Console.WriteLine("ok");
            Console.ReadKey();
        }

    }
}
