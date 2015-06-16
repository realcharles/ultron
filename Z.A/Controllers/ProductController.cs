using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z.A.IoC;
using Z.MVC.Core;

namespace Z.A.Controllers
{
    public class ProductController : BAsyncController<ProductIoC>
    {

        #region 我的产品库
        public ActionResult My()
        {
            return View();
        }
        //public async string GetMyProduct(string id) {
        //    return await ProductIoC.GetMyProduct(G.User.ID,id);
        //}
        #endregion
    }
}