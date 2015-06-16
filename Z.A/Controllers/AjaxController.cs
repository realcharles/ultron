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
    public class AjaxController : BAsyncController<AjaxIoC>
    {
        #region Menu
        public void GetMenuAsync() {
           
            AsyncManager.OutstandingOperations.Increment();
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    var menus =  this.IoC.GetMenu();
                    AsyncManager.Parameters["resp"] = ResponseType.Create(true, menus.toJSON());
                }
                catch (Exception ex)
                {
                    AsyncManager.Parameters["resp"] = ResponseType.Create(false,ex.Message);
                }
                finally
                {
                    AsyncManager.OutstandingOperations.Decrement();
                }
            });

        }
        public string GetMenuCompleted(ResponseType resp) {
            return resp.toJSON();
        }
        #endregion
    }
}