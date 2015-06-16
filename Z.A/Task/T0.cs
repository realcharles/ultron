using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z.A.Models;
using Z.A.Models.Collector;
using Z.A.Models.Product;
using Z.EBayV2;
using Z.EBayV2.Models;
using Z.EBayV2.Shopping;
using Z.MVC.Core;

namespace Z.A.Task
{
    public class T0 : ZTask
    {
        public T0() : base("采集服务") { }
        protected override void Begin(object obj = null)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Do();
            });
        }

        #region 采集任务
        public async void Do()
        {
            try
            {
                this.Debug(EnumLogType.Info, "Starting T0");
                var collection = Collection<CollectorType>();
                var query = Query.And(Query.LTE("ExecDT", new BsonDateTime(DateTime.Now)), Query.EQ("Result.Ok", false));
                var groups = collection.Find(query).GroupBy(t => t.For.For);
                foreach (var list in groups)
                {
                    foreach (var collector in list)
                    {
                        var IDs = await DoCollecter(collector);
                        collector.Result = Result.Create(true,"返回记录:"+ IDs.Count.ToString());
                        var dic = GenMyProduct(IDs,collector);
                        foreach(var kv in dic)
                            collector.Log(string.Format("{0}:{1}",kv.Key,kv.Value));
                        collection.Save(collector);
                    }
                }

                this.Debug(EnumLogType.Info, "End T0");
            }
            catch (Exception ex)
            {
                ex.Debug(EnumLogType.Error, "T0:" + ex.Message);
            }

        }

        public async System.Threading.Tasks.Task<IList<string>> DoCollecter(CollectorType collector)
        {
            IList<string> IDs = new List<string>();//用于保存成功的ItemID生成Product
            try
            {
                collector.Log("Starting...");
                var items = collector.For.Value.Split(new char[] { ',' });
               
                switch (collector.For.For.ToLower())
                {
                    case "ebay":
                        switch (collector.For.Key.ToLower())
                        {
                            case "itemid":
                                #region
                                foreach (var item in items)
                                {
                                    var result  = await GetEBayItem(item);
                                    collector.Log(result.Message);
                                    if (result.Ok)
                                    {
                                        IDs.Add(item);
                                    }
                                }
                                #endregion
                                break;
                        }
                        break;
                    case "amazon":
                        switch (collector.For.Key.ToLower()) { 
                            case "asin":
                                #region
                                foreach (var item in items)
                                {
                                    var result = await GetAmazonItem(item);
                                    collector.Log(result.Message);
                                    if (result.Ok)
                                    {
                                        IDs.Add(item);
                                    }
                                }
                                #endregion
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                collector.Log(ex.Message);
            }
            finally
            {
                
            }
            return IDs;
        }

        #region ebay
        private async System.Threading.Tasks.Task<Result> GetEBayItem(string ItemID)
        {
            //先检查此ItemID是否已经有了
            var collection = Collection<EBayItemType>();
            var query = Query.EQ("ItemID", ItemID);
            var check = collection.FindOne(query);
            if (check == null)
            {
                GetSingleItemResponseType resp = await Z.EBayV2.Shopping.Util.GetSingleItem(ItemID);
                if (resp is GetSingleItemResponseNull)
                {
                    return Result.Create(false, "[" + ItemID + "]采集异常:" + ((GetSingleItemResponseNull)resp).data);
                }
                if (resp != null && resp.Item != null)
                {
                    var result = await SaveEBayItem(resp.Item);
                    if (result.Ok)
                        return Result.Create(true, "[" + ItemID + "]采集成功,保存成功");
                    else
                        return Result.Create(false, "[" + ItemID + "]采集成功,保存失败:" + result.LastErrorMessage);
                }
                return Result.Create(false, "[" + ItemID + "]采集失败");
            }
            return Result.Create(true, "[" + ItemID + "]前人已采");
        }


        private async System.Threading.Tasks.Task<MongoDB.Driver.WriteConcernResult> SaveEBayItem(Z.EBayV2.Shopping.EBayItemType item)
        {
            var collction = Collection<EBayItemType>();
            item.TimeStamp = DateTime.Now;
            var query = Query.EQ("ItemID", item.ItemID);
            var check = collction.FindOneAs<EBayItemType>(query);
            if (check == null)
            {
                var cost = await item.GetShippingCost();
                item.ShippingCosts = cost;
                return collction.Insert(item);
            }
            else
            {
                item._id = check._id;
                return collction.Save(item);
            }
        }
        #endregion

        #region amazon
        private async System.Threading.Tasks.Task<Result> GetAmazonItem(string ItemID) {
            return await System.Threading.Tasks.Task.Run<Result>(()=> Result.Create(false, ""));
        }
        #endregion

        #region 生成我的产品信息
        private Dictionary<string,string> GenMyProduct(IList<string> IDs,CollectorType collector) {
            var collection = Collection<Models.Product.ProductType>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var id in IDs) {
                try
                {
                    var query = Query.And(Query.EQ("Creater.ID", collector.Owner.ID),
                                          Query.And(Query.EQ("For.For", collector.For.For), Query.EQ("For.Key", collector.For.Key), Query.EQ("For.Value", id)));
                    var check = collection.FindOne(query);
                    if (check != null)
                    {
                        dic.Add(id, "跳过");
                        continue;
                    }
                    Models.Product.ProductType prd = new Models.Product.ProductType();
                    prd.Creater = collector.Owner;
                    prd.For = new Models.Product.ProductFor()
                    {
                        For = collector.For.For,
                        Key = collector.For.Key,
                        Value = id
                    };
                    prd.InsDT = DateTime.Now;
                    prd.Name = collector.For.For + "-" + collector.For.Key + "-" + id;
                    prd.Timestamp = DateTime.Now;
                    prd.ProductId = prd.GenID();
                    prd.Statu = Models.Product.EnumProductStatu.WaitingForConfirm;
                    prd.CatelogCode = CatelogType.CatelogNullType.toCatelogCode();
                    collection.Insert(prd);
                    dic.Add(id, prd.ProductId);
                }
                catch (Exception ex) {
                    dic.Add(id, ex.Message);
                }
            }
            return dic;
        }
        #endregion
        #endregion

        protected override void End()
        {

        }
    }
}