using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Z.EBayV2.Shopping
{
    public class Util : BUtil
    {
        #region
        protected const string gateway = "http://open.api.ebay.com/shopping?callname={0}";
        protected static WebRequest CreateRequest(string url)
        {
            WebRequest request = HttpWebRequest.Create(url);
            request.Headers.Add("X-EBAY-API-APP-ID", DevSetting.AppID);
            request.Headers.Add("X-EBAY-API-RESPONSE-ENCODING", "JSON");
            request.Headers.Add("X-EBAY-API-VERSION", "897");
            return request;
        }
        #endregion

        #region GetSingleItem
        public static async Task<GetSingleItemResponseType> GetSingleItem(string itemid)
        {
            try
            {
                var url = string.Format(gateway + "&ItemID={1}&IncludeSelector=Details,Description,ItemSpecifics,Variations", "GetSingleItem", itemid);
                var request = CreateRequest(url);
                var resp = await request.GetResponseAsync();
                var stream = resp.GetResponseStream();
                var reader = new System.IO.StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                GetSingleItemResponseType rt = json.toObject<GetSingleItemResponseType>();
                return rt;
            }
            catch (Exception ex) {
                return GetSingleItemResponseNull.Create(false,ex.Message);
            }
        }
        #endregion

        #region GetMultipleItems
        public static async Task<GetMultipleItemsResponseType> GetMultipleItems(IList<string> itemids, string IncludeSelector, EventHandler handler)
        {
            try
            {
                string itemid = string.Empty;
                foreach (var id in itemids)
                    itemid += id + ",";
                if (itemid.EndsWith(","))
                    itemid = itemid.Substring(0, itemid.Length - 1);
                var url = string.Format(gateway + "&ItemID={1}&IncludeSelector={2}", "GetMultipleItems", itemid, IncludeSelector);
                var request = CreateRequest(url);
                var resp = await request.GetResponseAsync();
                var stream = resp.GetResponseStream();
                var reader = new System.IO.StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                GetMultipleItemsResponseType rts = json.toObject<GetMultipleItemsResponseType>();
                return rts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetShippingCosts
        /// <summary>
        /// 获取运费
        /// US,CA,GB,DE,AU
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public static async Task<IList<ShippingCostType>> GetShippingCosts(string itemid)
        {
            IList<ShippingCostType> list = new List<ShippingCostType>();
            var codes = CountryCodes;
            foreach (var code in codes)
            {
                try
                {
                    var url = string.Format(gateway + "&ItemID={1}&DestinationCountryCode={2}", "GetShippingCosts", itemid, code);
                    var request = CreateRequest(url);
                    var response = await request.GetResponseAsync();
                    var stream = response.GetResponseStream();
                    var reader = new System.IO.StreamReader(stream);
                    var json = await reader.ReadToEndAsync();
                    GetShippingCostsResponseType rt = json.toObject<GetShippingCostsResponseType>();
                    list.Add(new ShippingCostType()
                    {
                        CountryCode = code,
                        ShippingCost = rt.ShippingCostSummary
                    });
                    return list;
                }
                catch
                {
                    continue;
                }

            }
            return list;
        }
        private static IList<string> _countryCodes;
        public static IList<string> CountryCodes
        {
            get
            {
                if (_countryCodes == null || _countryCodes.Count == 0)
                {
                    _countryCodes = new List<string>() {
                    "US","GB","DE","CA","AU"
                };
                }
                return _countryCodes;
            }
            set
            {
                _countryCodes = value;
            }
        }
        #endregion

    }

}
