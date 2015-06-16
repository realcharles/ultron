using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z.A.Models;
using Z.A.Models.Collector;
using Z.MVC.Core;
using Z.MVC.Core.Models;

namespace Z.A.IoC
{
    public class CollectorIoC : BIoC
    {
        public IResponseType NewNC(string Name, int Minutes, string For, string Key, string Value)
        {
            try
            {
                var collction = Collection<CollectorType>();
                var nc = new CollectorType();
                nc.Name = Name;
                nc.ExecDT = DateTime.Now.AddMinutes(Minutes);
                nc.For = new CollectorFor()
                {
                    For = For,
                    Key = Key,
                    Value = Value
                };
                nc.InsDT = DateTime.Now;
                nc.Owner = User;
                nc.Timestamp = DateTime.Now;
                nc.Logs = new List<LogM>();
                nc.Logs.Add(new LogM()
                {
                    InsDT = DateTime.Now,
                    User = User.Name,
                    Message = "新建采集任务"
                });
                nc.Result = Result.Create(false, "未执行");
                collction.Insert(nc);
                return ResponseType.Create(true, "成功!");
            }
            catch (Exception ex)
            {
                return ResponseType.Create(false, ex.Message);
            }
        }
        public IResponseType GetMyNC()
        {
            try
            {
                var collction = Collection<CollectorType>();
                var min = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")).AddDays(-30);
                var query = Query.And(Query.EQ("Owner.ID", User.ID), Query.GTE("InsDT", new BsonDateTime(min)));
                var list = collction.Find(query).SetFields("_id", "Name", "InsDT", "Result").OrderByDescending(n => n.InsDT).Select(nc => new
                {
                    ID = nc._id.ToString(),
                    nc.Name,
                    YearMonth = nc.InsDT.ToString("yyyy年MM月"),
                    Day = (nc.InsDT.Day <= 9 ? "0" + nc.InsDT.Day.ToString() : nc.InsDT.Day.ToString()) + "日",
                    nc.Result
                }).GroupBy(nc => nc.YearMonth).ToList();
                if (list.Count == 0)
                    return ResponseType.Create(false, "您还没有采集计划哦");
                return ResponseType.Create(true, new
                {
                    sum = list.Sum(t=>t.Count()),
                    datas = list.Select(nc => new
                {

                    id = nc.Key,
                    name = nc.Key + "(" + nc.Count() + ")",
                    children = nc.GroupBy(t => t.Day).Select(m => new
                    {
                        id = m.Key,
                        name = m.Key + "(" + m.Count() + ")",
                        children = m.Select(n => new
                        {
                            id = n.ID,
                            name = n.Name,
                            n.Result
                        })
                    })
                })
                }.toJSON());

            }
            catch (Exception ex)
            {
                return ResponseType.Create(false, ex.Message);
            }
        }

        public IResponseType GetNC(string id) {
            try {
                var collection = Collection<CollectorType>();
                var query = Query.EQ("_id", ObjectId.Parse(id));
                var nc = collection.FindOne(query);
                if (nc == null)
                    return ResponseType.Create(false,id+ "不存在");
                return ResponseType.Create(true, new
                {
                    ID=nc._id.ToString(),
                    nc.Name,
                    InsDT = nc.InsDT.ToString("yyyy/MM/dd HH:mm:ss"),
                    ExecDT = nc.ExecDT.ToString("yyyy/MM/dd HH:mm:ss"),
                    User = nc.Owner.Name,
                    For = nc.For.Key + ":" + nc.For.Value + " in " + nc.For.For,
                    nc.Result,
                    Logs = nc.Logs.Select(l => l.User+" "+l.InsDT.ToString("yyyy/MM/dd HH:mm:ss")+" "+l.Message)
                }.toJSON());
            }
            catch (Exception ex) {
                return ResponseType.Create(false, ex.Message);
            }
        }

        public IResponseType DeleteNC(string id) {
            try
            {
                var collection = Collection<CollectorType>();
                var query = Query.EQ("_id", ObjectId.Parse(id));
                collection.Remove(query);
                return ResponseType.Create(true, "");
            }
            catch (Exception ex)
            {
                return ResponseType.Create(false, ex.Message);
            }
        }
    }
}