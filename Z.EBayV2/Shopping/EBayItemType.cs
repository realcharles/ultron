
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EBayV2.Models;

namespace Z.EBayV2.Shopping
{
    public class EBayItemType
    {
        public ObjectId _id;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TimeStamp;       
        public string ItemID;
        public string SKU;
        public EBayVariations Variations;
        public string Site;
        public DateTime StartTime;
        public DateTime EndTime;
        public string ViewItemURLForNaturalSearch;
        public string ListingType;
        public string Location;
        public string GalleryURL;
        public string[] PictureURL;
        public string PrimaryCategoryID;
        public string PrimaryCategoryName;
        public string PrimaryCategoryIDPath;
        public CurrentType CurrentPrice;
        public CurrentType ConvertedCurrentPrice;
        public string ListingStatus;
        public string TimeLeft;
        public string Title;
        public string Subtitle;
        public string Country;
        public bool AutoPay;
        public string ConditionID;
        public string ConditionDisplayName;
        public string QuantityAvailableHint;
        public int QuantityThreshold;
        public bool BestOfferEnabled;
        public string Description;
        public string[] PaymentMethods;
        public int HitCount;
        public int BidCount;
        public int Quantity;
        public int QuantitySold;
        public int QuantitySoldByPickupInStore;
        public EBaySeller Seller;
        public int HandlingTime;
        public string[] ShipToLocations;
        public string[] ExcludeShipToLocation;
        public bool GlobalShipping;
        public EBayMember HighBidder;       
        public EBayStore Storefront;
        public EBayReturnPolicy ReturnPolicy;
        public string[] PaymentAllowedSite;
        public bool IntegratedMerchantCreditCardEnabled;        
        public EBayItemSpecifics ItemSpecifics;
        public bool NewBestOffer;

        public IList<ShippingCostType> ShippingCosts;

    }
    public class EBaySeller {
        public string UserID;
        public string FeedbackRatingStar;
        public int FeedbackScore;
        public decimal PositiveFeedbackPercent;

    }
    public class EBayMember {
        public string UserID;
        public bool FeedbackPrivate;
        public string FeedbackRatingStar;
        public int FeedbackScore;
    }
    public class EBayItemSpecifics {
        public NameValue[] NameValueList;
    }

    public class NameValue{
        public string Name;
        public string[] Value;
    }
    public class EBayStore
    {
        public string StoreURL;
        public string StoreName;
    }
    public class EBayReturnPolicy {
        public string Refund;
        public string ReturnsWithin;
        public string ReturnsAccepted;
        public string ShippingCostPaidBy;
    }
    public class EBayVariations {
        public EBayVariation[] Variation;
    }
    public class EBayVariation {
        public string SKU;
        public CurrentType StartPrice;
        public int Quantity;
        public EBayVariationSpecifics VariationSpecifics;
        public EBaySellingStatus SellingStatus;
    }
    public class EBayVariationSpecifics {
        public NameValue[] NameValueList;
    }
    public class EBaySellingStatus{
        public int QuantitySold;
        public int QuantitySoldByPickupInStore;
    }
}
