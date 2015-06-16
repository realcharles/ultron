using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.EBayV2.Shopping
{
    public class GetSingleItemResponseType:BResponseType
    {
        public EBayItemType Item;
    }
    public class GetMultipleItemsResponseType : BResponseType
    {
        public IList<EBayItemType> Item;
    }
    public class GetShippingCostsResponseType : BResponseType {
        public ShippingCostSummaryType ShippingCostSummary;
    }

    public class GetSingleItemResponseNull:GetSingleItemResponseType
    {
        public bool ok;
        public string data;
        public static GetSingleItemResponseNull Create(bool ok, string data) {
            return new GetSingleItemResponseNull() {
                ok=ok,
                data=data
            };
        }
    }
}
