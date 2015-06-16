using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Z.A.IoC;
using Z.MVC.Core;
using Z.MVC.Core.Models;
namespace Z.A.Controllers
{
    public class AccountController : BAsyncController<AccountIoC>
    {
        //
        // GET: /Account/
        public ActionResult Login()
        {
            return View();
        }

        #region 用户验证
        public string Verify(string Usr, string Psd)
        {
            try
            {
                var user = this.IoC.Verify(Usr, Psd);
                if (user == null)
                    return ResponseType.Create(false, "用户名密码不匹配").toJSON();
                else
                {
                    SetAuth(user);
                    return ResponseType.Create(true, "/Home/Dashboard/").toJSON();
                }
            }
            catch (Exception ex)
            {
                return ResponseType.Create(false, ex.Message).toJSON();
            }

        }
        private void SetAuth(UserM user)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.ID, DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), true, user.toJSON());
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            Response.Cookies.Add(cookie);
        }

        #endregion
    }
}