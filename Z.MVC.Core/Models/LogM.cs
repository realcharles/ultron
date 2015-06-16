using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.MVC.Core.Models
{
    public class LogM
    {
         [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime InsDT;
         public string User;
        public string Message;
    }
}
