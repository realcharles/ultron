using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.MVC.Core.Models
{  
    /// <summary>
    /// 用于MongoDB保存数据，这样当保存用户时，不用再去关联取名字
    /// </summary>
    public class UserM {
        public string ID;
        public string Name;
    }
    

}
