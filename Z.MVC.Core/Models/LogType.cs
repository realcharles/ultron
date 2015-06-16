using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Z.MVC.Core.Models
{
    public class LogType
    {
        public ObjectId _id;
        public string GUID;
        public string User;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime;
        public string Value;
    }
}