using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Z.MVC.Core.Models;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NLog;
namespace Z.MVC.Core
{
    public static class Extension
    {
        #region JSON
        public static string toJSON(this object anyobject)
        {
            return JsonConvert.SerializeObject(anyobject);
        }
       
        public static T toObject<T>(this string JsonString)
        {
            return JsonConvert.DeserializeObject<T>(JsonString);
        }
        public static object toObject(this string JsonString)
        {
            return JsonConvert.DeserializeObject(JsonString);
        }
      
        #endregion 

        #region Debug

        public static void Debug(this object obj, EnumLogType lt, string log)
        {
            Task.Factory.StartNew(()=>{
                var logger = NLog.LogManager.GetCurrentClassLogger();
                switch (lt)
                {
                    case EnumLogType.Debug:
                        logger.Debug(log);
                        break;
                    case EnumLogType.Info:
                        logger.Info(log);
                        break;
                    case EnumLogType.Warn:
                        logger.Warn(log);
                        break;
                    case EnumLogType.Error:
                        logger.Error(log);
                        break;
                    case EnumLogType.Fatal:
                        logger.Fatal(log);
                        break;
                }
            });
        }  
        #endregion

        #region Log
        public static void WriteLog(this object obj,string guid,string usr,string value)
        {
            LogType log = new LogType();
            log.GUID = guid;
            log.User = usr;
            log.CreateTime = DateTime.Now;
            log.Value = value;
            obj.WriteLog(log);
        }
        public static void WriteLog(this object obj, LogType log)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    IDB db = new IDB();
                    var collection = db.Collection<LogType>();
                    collection.Insert(log);
                }
                catch (Exception ex) {
                    ex.Debug(EnumLogType.Error, "WriteLog:"+ex.Message);
                }
            });
        }
        public static IList<string> ReadLog(this object obj, string guid) {
            var result = Task.Factory.StartNew<IList<string>>(() => {
                try
                {
                    IDB db = new IDB();
                    var collection = db.Collection<LogType>();
                    var query = Query.EQ("GUID", guid);
                    var logs = collection.Find(query).SetFields("User", "CreateTime", "Value").SetSortOrder("CreateTime").Select(i => string.Format("[{0}][{1}]:{2}", i.User, i.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"), i.Value));
                    return logs.ToList();
                }
                catch (Exception ex) {
                    ex.Debug(EnumLogType.Error, "ReadLog:" + ex.Message);
                    return new List<string>();
                }
            });
            return result.Result;
        }
        #endregion

        #region Pager
        public static Pager toPager(this HttpRequestBase request) { 
            int pi = 1;
            int ps = 20;
            if(!string.IsNullOrEmpty(request.QueryString["page"]))
                int.TryParse(request.QueryString["page"],out pi);
             if(!string.IsNullOrEmpty(request.QueryString["pageSize"]))
                int.TryParse(request.QueryString["pageSize"],out ps);
             return Pager.Create(pi, ps);
        }
        public static MongoCursor<T> toPage<T>(this MongoCursor<T> cursor, Pager page)
        {
            return cursor.SetSkip((page.pi - 1) * page.ps).SetLimit(page.ps);
        }
        public static IEnumerable<T> toPage<T>(this IEnumerable<T> list, Pager page) {
            return list.Skip((page.pi - 1) * page.ps).Take(page.ps);
        }
        #endregion

        #region 格式化
        public static string toDT(this DateTime dt) {
            return dt.ToString("yyyy/MM/dd HH:MM:ss");
        }
        #endregion
    }
}
