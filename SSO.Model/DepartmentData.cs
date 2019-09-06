using System;
using System.Collections.Generic;

namespace SSO.Model
{
    public class DepartmentData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Layer { get; set; }

        public List<DepartmentData> Departments { get; set; }
    }
}
