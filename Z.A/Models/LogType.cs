using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Z.A.Models
{
    public class LogType
    {
        public ObjectId _id;
        public string Type;
        public string Logger;
        public DateTime CreateTime;
        public string content;
    }
}