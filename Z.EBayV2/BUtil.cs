using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Z.EBayV2.Models;

namespace Z.EBayV2
{
    public class BUtil
    {
        public static void SetConfigure(string xmlfile) {
            if (System.IO.File.Exists(xmlfile))
            {
                _devSetting = new EbayDevSetting();
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlfile);
                XmlNode MongoNode = doc.LastChild;
                foreach (XmlNode node in MongoNode.ChildNodes)
                {
                    if (node.NodeType != XmlNodeType.Element)
                        continue;
                    switch (node.Name)
                    {
                        case "AppID":
                            _devSetting.AppID = node.InnerText;
                            break;
                        case "DevID":
                            _devSetting.DevID = node.InnerText;
                            break;
                        case "CertID":
                            _devSetting.CertID = node.InnerText;
                            break;
                    }
                }
            }
        }
        protected static EbayDevSetting _devSetting;
        protected static EbayDevSetting DevSetting
        {
            get
            {
                return _devSetting;
            }
            set
            {
                _devSetting = value;
            }
        }

    }
  
}
