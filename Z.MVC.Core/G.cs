using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Z.MVC.Core.Models;
namespace Z.MVC.Core
{
    /// <summary>
    /// 全局对象
    /// </summary>
    public class G
    {
        #region 当前用户
        public static UserM User
        {
             get
            {
                if (HttpContext.Current == null)
                    return null;
                if (HttpContext.Current.Session["USER"] != null)
                    return (UserM)HttpContext.Current.Session["USER"];
                if (HttpContext.Current.Request == null)
                    return null;
                HttpCookie cookie = HttpContext.Current.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
                if (cookie == null)
                    return null;
                System.Web.Security.FormsAuthenticationTicket ticket = System.Web.Security.FormsAuthentication.Decrypt(cookie.Value);
                return ticket.UserData.toObject<UserM>();
            }
            set
            {
                HttpContext.Current.Session.Timeout = (int)System.Web.Security.FormsAuthentication.Timeout.TotalMinutes;
                HttpContext.Current.Session["USER"] = value;
            }
        }
        #endregion

        #region Admin
        public static UserM Admin
        {
            get
            {
                return new UserM() { 
                    ID="admin",
                    Name="管理员"
                };
            }
        }
        #endregion

        #region 目录
        private static IList<MenuItemType> _menus;
        public static IList<MenuItemType> MENUS
        {
            get
            {
                if (_menus == null || _menus.Count == 0)
                {
                    _menus = new List<MenuItemType>();
                    #region 加载目录
                    string file =AppDomain.CurrentDomain.BaseDirectory + @"config\menu.xml";
                    if (System.IO.File.Exists(file))
                    {
                        var doc = new System.Xml.XmlDocument();
                        doc.Load(file);
                        #region 解析文件
                        foreach (System.Xml.XmlNode root in doc.ChildNodes)
                        {
                            if (root.NodeType != System.Xml.XmlNodeType.Element)
                                continue;
                            foreach(System.Xml.XmlNode node in root.ChildNodes)
                            { 
                                var menu = toMenu(node);
                                if(menu.SubMenuItems != null && menu.SubMenuItems.Count > 1)
                                {
                                    menu.SubMenuItems = menu.SubMenuItems.OrderBy(m => m.Order).ToList();
                                }
                                _menus.Add(menu);
                            }
                        }
                        #endregion
                    }

                    #endregion
                    _menus = _menus.OrderBy(m => m.Order).ToList();
                }
                return _menus;
            }
        }
       
        private static MenuItemType toMenu(System.Xml.XmlNode node)
        {
            MenuItemType menu = new MenuItemType();
            menu.IsSelected = false;
            foreach (System.Xml.XmlAttribute attr in node.Attributes)
            {
                switch (attr.Name.ToLower())
                {
                    case "id":
                        menu.MenuID = attr.Value.Trim();
                        break;
                    case "name":
                        menu.MenuName = attr.Value.Trim();
                        break;
                    case "order":
                        menu.Order = int.Parse(attr.Value.Trim());
                        break;
                    case "image":
                        menu.MenuImage = attr.Value.Trim();
                        break;
                    case "target":
                        menu.Target = attr.Value.Trim();
                        break;
                    case "roles":
                        if(!string.IsNullOrEmpty(attr.Value))
                            menu.Roles = attr.Value.Split(new char[] { ',' }).Select(t=>int.Parse(t)).ToArray();
                        break;
                    case "imgurl":
                        menu.ImgUrl = attr.Value.Trim();
                        break;
                }
            }
            if(node.HasChildNodes)
            {
                menu.SubMenuItems = new List<MenuItemType>();
                 foreach(System.Xml.XmlNode subnode in node.ChildNodes)
                 {
                     if (subnode.NodeType != System.Xml.XmlNodeType.Element)
                         continue;
                     menu.SubMenuItems.Add(toMenu(subnode));
                 }
            }
            return menu;
        }
        #endregion

        #region 数据库
        public static Models.MongoDBSetting MongoDBSetting { get; set; }
        #endregion
    }
}
