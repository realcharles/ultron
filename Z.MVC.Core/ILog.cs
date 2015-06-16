using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.MVC.Core
{
    public class ILog
    {
        #region 日志对象
        private NLog.Logger _logger;
        protected NLog.Logger logger
        {
            get
            {
                if (_logger == null)
                    _logger = NLog.LogManager.GetCurrentClassLogger();
                return _logger;
            }
        }
        #endregion
    }
}
