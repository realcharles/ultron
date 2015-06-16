using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z.A.Models.Product
{
    public class CatelogType:IMongo
    {
        public string CatelogID;
        public string CatelogName;
        public string Parent;
       
        public static CatelogType CatelogNullType{
            get{
                return new CatelogType() { 
                    CatelogID="XX",
                    CatelogName="XX",
                    Parent=string.Empty
                };
            }
        }
    }

    public static class Extension {
        public static string toCatelogCode(this CatelogType catelog)
        {
            if (string.IsNullOrEmpty(catelog.Parent))
                return catelog.CatelogID;
            return catelog.Parent +"-"+ catelog.CatelogID;
        }
        public static string[] toCatelog(this string catelogCode) {
            return catelogCode.Split(new char[] { '-' });
        }
    }
}