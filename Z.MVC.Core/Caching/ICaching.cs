using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
namespace Z.MVC.Core.Caching
{
    public interface ICaching
    {
    }
  
    public class CachingFactory
    { 
        public static MemoCaching Create(string name)
        {
            return new MemoCaching(name);
        }
    }
}
