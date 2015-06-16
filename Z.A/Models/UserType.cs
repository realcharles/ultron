using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z.A.Models
{
    public class UserType:IMongo
    {
        public string UserID;
        public string Name;
        public string Password;
    }
}