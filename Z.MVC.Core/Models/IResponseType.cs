using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.MVC.Core
{
    public interface IResponseType:IModel
    {
    }
    
    /// <summary>
    /// Http Response
    /// </summary>
    public class ResponseType : IResponseType
    {
        private ResponseType() { }
        /// <summary>
        /// 状态
        /// </summary>
        public bool ok;
        /// <summary>
        /// JSON 格式的数据
        /// </summary>
        public string data;
      
        /// <summary>
        /// 创建返回
        /// </summary>
        /// <param name="statuCode">状态码</param>
        /// <param name="datas">JSON格式的数据</param>
        /// <param name="nextRequest">下一个请求</param>
        /// <returns></returns>
        public static ResponseType Create(bool ok=true, string data = null)
        {
            ResponseType response = new ResponseType();
            response.ok = ok;
            response.data = data;
            return response;
        }
    }

    public class PageResultType : IResponseType {
        public int _total;
        public int _page;
        public int _pageSize;
        public object _data;
        public static PageResultType Create(int total, int page, int pagesize, object data)
        {
            PageResultType pr = new PageResultType();
            pr._total = total;
            pr._data = data;
            pr._page = page;
            pr._pageSize = pagesize;
            return pr;
        }
        public static PageResultType CreateNull(int page,int pagesize) {
            return Create(0, page, pagesize, null);
        }
    }
}
