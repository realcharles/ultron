using System.Web;
using System.Web.Mvc;

namespace Z.A
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new Z.MVC.Core.ErrorAttribute());
        }
    }
}
