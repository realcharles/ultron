using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z.MVC.Core.Models;

namespace Z.A.Models.Collector
{
    public class CollectorType:IMongo
    {
        public string Name;
        /// <summary>
        /// 所有者
        /// </summary>
        public UserM Owner;
        /// <summary>
        /// 执行时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]   
        public DateTime ExecDT;
        public CollectorFor For;
        public IList<LogM> Logs;
        public Result Result;
    }
    public class CollectorFor
    {
        public string For;
        public string Key;
        public string Value;
    }

    public static class Extension{
        public static void Log(this CollectorType collector, string msg) {
            if (collector.Logs == null)
                collector.Logs = new List<LogM>();
            collector.Logs.Add(new LogM() { 
                User=(Z.MVC.Core.G.User==null?"":Z.MVC.Core.G.User.Name),
                InsDT=DateTime.Now,
                Message=msg
            });
        }
    }
}