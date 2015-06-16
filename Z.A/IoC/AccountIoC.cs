using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z.A.Models;
using Z.MVC.Core;
using Z.MVC.Core.Models;

namespace Z.A.IoC
{
    public class AccountIoC : BIoC
    {
        public UserM Verify(string u, string p) {
            var collection = Collection<UserType>();
            var query = Query.And(Query.EQ("UserID", u), Query.EQ("Password",GUtil.GetMd5Hash(p)));
            var usr = collection.FindOne(query);
            if (usr == null)
                return null;
            return new UserM() { 
                ID=usr.UserID,
                Name=usr.Name
            };
        }
    }
}