using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Z.A.IoC;
using Z.MVC.Core;

namespace Z.A.Controllers
{
    [Auth]
    public class CollectorController : BAsyncController<CollectorIoC>
    {
        #region 采集管理
        public ActionResult NC()
        {
            return View();
        }
        /// <summary>
        /// 新采集
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Minutes"></param>
        /// <param name="For"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public async Task<string> NewNC(string Name, int Minutes,string For,string Key,string Value)
        {
            return await Exec<string>(() => this.IoC.NewNC(Name,Minutes,For,Key,Value).toJSON());
        }
        public async Task<string> GetMyNC() {
            return await Exec<string>(() => IoC.GetMyNC().toJSON());
        }
        public async Task<string> GetNC(string id) {
            return await Exec<string>(() => IoC.GetNC(id).toJSON());
        }
        public async Task<string> DeleteNC(string id) {
            return await Exec<string>(() => IoC.DeleteNC(id).toJSON());
        }
        #endregion
    }
}