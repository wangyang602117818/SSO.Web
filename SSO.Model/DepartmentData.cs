using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;

namespace SSO.Model
{
    public class DepartmentData
    {
        [BsonElement("scopedSlots")]
        public ScopedSlots ScopedSlots { get { return new ScopedSlots(); } }
        [BsonElement("value")]
        public string Value { get { return Code; } }

        public int Id { get; set; }
        [BsonElement("key")]
        public string Code { get; set; }
        [BsonElement("title")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Layer { get; set; }
        public string ParentCode { get; set; }
        [BsonElement("children")]
        public List<DepartmentData> Children { get; set; }
    }
    public class ScopedSlots
    {
        public string title = "custom";
    }
}
