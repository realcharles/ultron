using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Z.MVC.Core.Caching;
using Z.DB.Mongo;
using Z.MVC.Core.Models;

namespace Z.MVC.Core
{
    
    public class BIoC:IDB
    {
        public BIoC() {
            User = G.User;
        }

        #region 当前用户
        public UserM User { get; set; }        
        #endregion
    }
    public class IoCFactory
    {  
        public static T Create<T>()
        {
           return (T)Activator.CreateInstance(typeof(T));
        }
    }

}
