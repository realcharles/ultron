using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Z.MVC.Core
{
    public class ErrorAttribute: System.Web.Mvc.HandleErrorAttribute
    {
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            //如果异常未处理
            if (!filterContext.ExceptionHandled)
            {
                Exception innerException = filterContext.Exception;
                //如果错误码为500
                if ((new HttpException(null, innerException).GetHttpCode() == 500) && this.ExceptionType.IsInstanceOfType(innerException))
                {
                    //获取出现异常的controller名和action名，用于记录
                    string controllerName = (string)filterContext.RouteData.Values["controller"];
                    string actionName = (string)filterContext.RouteData.Values["action"];
                    //定义一个HandErrorInfo，用于Error页使用
                    HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                    //ViewResult是ActionResult，经常出现在controller中action方法return后面，但是出现形式是View()
                    ViewResult result = new ViewResult
                    {
                        ViewName = this.View,
                        MasterName = this.Master,
                        //定义ViewData，使用的是泛型
                        ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                        TempData = filterContext.Controller.TempData
                    };
                    filterContext.Result = result;
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.StatusCode = 500;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

                    NLog.LogManager.GetCurrentClassLogger().Error(string.Format("[/{0}/{1}/]:{2}\r\nStackTrack:{3}",controllerName,actionName,innerException.Message,innerException.StackTrace));
                }
            }
            base.OnException(filterContext);
        }
    }
}
