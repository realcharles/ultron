using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EBayV2.Models;

namespace Z.EBayV2.Shopping
{
    public class ShippingCostType {
        public string CountryCode;
        public ShippingCostSummaryType ShippingCost;
    }
    public class ShippingCostSummaryType {
        public string ShippingServiceName;
        public CurrentType ShippingServiceCost;
        public string ShippingType;
        public CurrentType ListedShippingServiceCost;
        public string LogisticPlanType;
    }
}
