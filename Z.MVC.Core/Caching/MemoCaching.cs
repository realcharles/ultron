using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.MVC.Core.Caching
{
    public class MemoCaching:System.Runtime.Caching.MemoryCache,ICaching
    {
       public MemoCaching(string name):base(name)
        { }
    }
}
