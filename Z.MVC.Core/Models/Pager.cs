using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.MVC.Core.Models
{
    public class Pager
    {
        public int pi;
        public int ps;
        public static Pager Create(int pi, int ps) {
            Pager pager = new Pager();
            pager.pi = pi;
            pager.ps = ps;
            return pager;
        }
    }

    public class PageResult
    {
        public int _total;
        public int _page;
        public int _pageSize;
        public object _data;
        public static PageResult Create(int total, int page, int pagesize, object data)
        {
            PageResult pr = new PageResult();
            pr._total = total;
            pr._data = data;
            pr._page = page;
            pr._pageSize = pagesize;
            return pr;
        }
    }
}
