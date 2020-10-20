using System.Collections.Generic;

namespace SSO.Web.Models
{
    public class LogListModel
    {
        private int pageIndex = 1;
        private int pageSize = 10;
        public string From { get; set; }
        public string UserId { get; set; }
        public Dictionary<string, string> Sorts { get; set; }
        public int PageIndex { get => pageIndex; set => pageIndex = value; }
        public int PageSize { get => pageSize; set => pageSize = value; }
    }
}