using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Z.MVC.Core
{
    public class CacheAttribute : OutputCacheAttribute
    {
        public CacheAttribute()
        {
            this.Duration = 60;
            this.Location = System.Web.UI.OutputCacheLocation.Any;
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            filterContext.HttpContext.Response.Cache.SetOmitVaryStar(true);
        }
    }
}
