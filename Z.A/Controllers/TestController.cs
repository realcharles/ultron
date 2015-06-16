using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z.A.Models;
using Z.MVC.Core;

namespace Z.A.Controllers
{
    public class TestController : Controller
    {
        public string InitAdmin() {
            IDB db = new IDB();
            var collection = db.Collection<UserType>();
            UserType user = new UserType();
            user.UserID = "admin";
            user.Name = "管理员";
            user.Password = GUtil.GetMd5Hash("f");
            user.InsDT = DateTime.Now;
            collection.Insert(user);
            return user.UserID;
        }
	}
}