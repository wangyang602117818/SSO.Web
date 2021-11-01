using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.TaskScheduling.Schedules
{
    interface ISchedule
    {
        string Name { get;  }
        string Description { get;}
        string Execute();
    }
}
