using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z.MVC.Core.Models;

namespace Z.A.Models.Product
{
    public class ProductType:IMongo
    {
        /// <summary>
        /// 产品号
        /// </summary>
        public string ProductId;
        /// <summary>
        /// 产品名
        /// </summary>
        public string Name;
        /// <summary>
        /// 创建者
        /// </summary>
        public UserM Creater;
        /// <summary>
        /// 产品源
        /// </summary>
        public ProductFor For;
        /// <summary>
        /// 类目编号
        ///结构类似：AA-BB-CC
        /// </summary>
        public string CatelogCode;
        /// <summary>
        /// 产品状态
        /// </summary>
        public EnumProductStatu Statu;
    }
    public class ProductFor
    {
        public string For;
        public string Key;
        public string Value;
    }
    public enum EnumProductStatu {
        WaitingForConfirm = 0,
        WaitingForAudit=10,
        CanUse=16,
        Die=40
    }

    public static class Extension {
        public static string toText(this EnumProductStatu statu) {
            switch (statu) { 
                case EnumProductStatu.WaitingForConfirm:
                    return "待确认";
                case EnumProductStatu.WaitingForAudit:
                    return "待审核";
                case EnumProductStatu.CanUse:
                    return "可使用";
                case EnumProductStatu.Die:
                    return "淘汰";
                default:
                    return statu.ToString();
            }
        }
        public static string toProductStatuText(this int statu) {
            return toText((EnumProductStatu)statu);
        }
    }
}