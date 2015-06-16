using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using Z.MVC.Core;

namespace Z.A
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            InitZ();
        }
        private void InitZ()
        {
            #region MongoDBSetting
            G.MongoDBSetting = new Z.MVC.Core.Models.MongoDBSetting();
            string file = AppDomain.CurrentDomain.BaseDirectory + @"config\MongoDB.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode MongoNode = doc.LastChild;
            foreach (XmlNode node in MongoNode.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;
                switch (node.Name)
                {
                    case "DBName":
                        G.MongoDBSetting.DBName = node.InnerText;
                        break;
                    case "ConnectionString":
                        G.MongoDBSetting.ConnectionString = node.InnerText;
                        break;
                }
            }
            #endregion
            #region EBayDevSetting
            Z.EBayV2.BUtil.SetConfigure(AppDomain.CurrentDomain.BaseDirectory + @"config\EBayDevAccount.xml");
            #endregion
            #region Task
            Z.MVC.Core.ZTaskManager.Add(new Task.T0());
            #endregion
        }
    }
}
