using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z.MVC.Core;

namespace Z.A.IoC
{
    public class TaskIoC:BIoC
    {
        public IResponseType Run(string id) {
            var task = Z.MVC.Core.ZTaskManager.Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task == null)
            {
                return ResponseType.Create(false, id + "不存在");
            }
            task.Run();
            return ResponseType.Create(true, "操作成功");
        }
    }
}