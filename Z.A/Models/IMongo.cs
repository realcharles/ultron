using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z.A.Models
{
    public class IMongo
    {
        public ObjectId _id;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]   
        public DateTime InsDT;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]   
        public DateTime Timestamp;
    }
}