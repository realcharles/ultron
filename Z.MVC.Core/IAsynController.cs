using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Z.MVC.Core.Caching;
using Z.MVC.Core.Models;

namespace Z.MVC.Core
{
    public interface IAsynController
    {
    }
   
    public class BAsyncController<T>:AsyncController,IAsynController
    {
        
        #region 实体对象
        private T _ioc;
        public T IoC {
            get
            {
                if (_ioc == null)
                    _ioc = IoCFactory.Create<T>();
                return _ioc;
            }
            set {
                _ioc = value;
            }
        }
        #endregion
        #region 缓存
        private MemoCaching _cache;
        public MemoCaching MemoCache
        {
            get
            {
                if (_cache == null)
                    _cache = CachingFactory.Create(ControllerName);
                return _cache;
            }
        }
        #endregion
        #region Pager
        public Pager Page {
            get {
                return Request.toPager();
            }
        }
        #endregion
        #region NLog
        public NLog.Logger Logger{
            get{
                return NLog.LogManager.GetCurrentClassLogger();
            }
        }
        #endregion
        #region ControllerName
        public string ControllerName
        {
            get
            {
                return this.ControllerContext.RouteData.Values["Controller"].ToString();
            }
        }
        #endregion
        #region ActionName
        public string ActionName
        {
            get
            {
                return this.ControllerContext.RouteData.Values["Action"].ToString();
            }
        }
        #endregion
        #region 构造函数
        public BAsyncController() {
            IoC = IoCFactory.Create<T>();
        }
        #endregion
        #region override
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string controller = ControllerName.ToLower();
            string action = ActionName.ToLower();
            ViewData["init"] = "$." + controller+"_"+ action + ".ready()";
            ViewData["zjs"] = controller + "/" + action + ".js";
            base.OnActionExecuted(filterContext);
        }
        #endregion

        #region SyncExec
        public async Task<M> Exec<M>(Func<M> function)
        {
            return await Task.Factory.StartNew<M>(function);
        }
        #endregion
    }

}
