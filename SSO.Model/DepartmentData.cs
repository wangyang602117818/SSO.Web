using System.Collections.Generic;
using Newtonsoft.Json;
namespace SSO.Model
{
    public class DepartmentData
    {
        [JsonProperty("scopedSlots")]
        public ScopedSlots ScopedSlots { get { return new ScopedSlots(); } }
        [JsonProperty("value")]
        public string Value { get { return Code; } }

        public int Id { get; set; }
        [JsonProperty("key")]
        public string Code { get; set; }
        [JsonProperty("title")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Layer { get; set; }
        public string ParentCode { get; set; }
        [JsonProperty("children")]
        public List<DepartmentData> Children { get; set; }
    }
    public class ScopedSlots
    {
        public string title = "custom";
    }
}
