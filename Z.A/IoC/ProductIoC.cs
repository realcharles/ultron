using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z.A.Models.Product;
using Z.MVC.Core;

namespace Z.A.IoC
{
    public class ProductIoC:BIoC
    {
        //public async IResponseType GetMyProduct(string userId, string id) {
        //    try {
        //        var collection = Collection<ProductType>();
        //        var query = Query.And(Query.EQ("Creater.ID", userId));
        //        var groups = collection.Find(query).SetFields("_id","ProductID","Name","Statu").GroupBy(t => t.Statu);
        //        foreach (var g in groups) { 
                    
        //        }
        //    }
        //    catch (Exception ex) {
        //        return ResponseType.Create(false, ex.Message);
        //    }
        //}
    }
}