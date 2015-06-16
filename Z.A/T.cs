using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z.A.Models;
using Z.MVC.Core;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
namespace Z.A
{
    /// <summary>
    /// 全局对象
    /// </summary>
    public class T:IDB
    {
        #region 用户列表
        private static IList<UserType> _userList;
        /// <summary>
        /// 当前所有可用用户的清单
        /// </summary>
        public static IList<UserType> UserList {
            get {
                if (_userList == null || _userList.Count == 0) {
                    IDB DB = new IDB();
                    var collection = DB.Collection<UserType>();
                    var query = Query.EQ("Statu", 1);
                    _userList = collection.Find(query).SetFields("UserID", "Name", "Roles", "CKDMS", "Accounts").ToList();
                }
                return _userList;
            }
        }
        #endregion

        #region 当前用户
        /// <summary>
        /// 当前用户
        /// </summary>
        public static UserType User {
            get {
                return UserList.Where(u => u.UserID == Z.MVC.Core.G.User.ID).FirstOrDefault();
            }
        }
        #endregion

        #region 币种
        public const string BZDM = "USD";
        #endregion
    }
}