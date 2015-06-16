using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EBayV2.Shopping;

namespace Z.EBayV2
{
    public static class Extension
    {
        #region 通用
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

        #region ShippingCost
        public static async System.Threading.Tasks.Task<IList<ShippingCostType>> GetShippingCost(this EBayItemType item)
        {
            return await Shopping.Util.GetShippingCosts(item.ItemID);
        }
        #endregion

        #region ListingType
        public static string toListingType(this string listingtype)
        {
            switch (listingtype)
            {
                case "Auction":
                case "Chinese":
                case "Dutch":
                case "Live":
                    return "Auction";
                case "FixedPriceItem":
                case "StoresFixedPrice":
                case "PersonalOffer":
                case "Fixed":
                    return "Fixed";
                default:
                    return listingtype;
            }
        }
        #endregion
    }
}
