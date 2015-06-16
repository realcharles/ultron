using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z.MVC.Core;

namespace Z.A.Models
{
    public static class Extension
    {
        #region GenID
        public static string GenID<T>(this T t,int num=9) {
           return GUtil.GenerateUniqueId(num);
        }
        #endregion
    }
}