using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z.A.IoC;
using Z.MVC.Core;

namespace Z.A.Controllers
{
   [Auth]
    public class HomeController : BAsyncController<HomeIoC>
    {
        //
        // GET: /Home/
        public ActionResult Dashboard()
        {
            return View();
        }
	}
}