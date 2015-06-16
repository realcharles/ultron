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
    public class TaskController : BAsyncController<TaskIoC>
    {
        //
        // GET: /Task/
        public ActionResult List()
        {
            return View();
        }

        public string GetTasks() {
            return ResponseType.Create(true, Z.MVC.Core.ZTaskManager.Tasks.OrderBy(t => t.Id).Select(t => new { 
                t.Id,
                t.Name,
                Statu=t.Statu.ToString(),
                RunTime=t.PreRunTime == DateTime.MinValue?"": t.PreRunTime.toDT()
            }).toJSON()).toJSON();
        }
        [AllowAnonymous]
        public void RunAsync(string id) {
            AsyncManager.OutstandingOperations.Increment();
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    var resp = this.IoC.Run(id);
                    AsyncManager.Parameters["resp"] = resp;
                }
                catch (Exception ex)
                {
                    AsyncManager.Parameters["resp"] = ResponseType.Create(false, ex.Message);
                }
                finally
                {
                    AsyncManager.OutstandingOperations.Decrement();
                }
            });
           
        }
        public string RunCompleted(IResponseType resp) {
            return resp.toJSON();
        }
	}
}