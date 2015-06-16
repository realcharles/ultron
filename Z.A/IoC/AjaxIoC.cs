using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z.MVC.Core;
using Z.MVC.Core.Models;

namespace Z.A.IoC
{
    public class AjaxIoC:BIoC
    {
        public IList<MenuItemType> GetMenu() {
            return Z.MVC.Core.G.MENUS;
        }
    }
}