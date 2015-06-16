using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.MVC.Core.Models
{
    public class MenuItemType
    {
        /// <summary>
        /// 目录号
        /// </summary>
        public string MenuID;
        /// <summary>
        /// 目录图片
        /// </summary>
        public string MenuImage;
        /// <summary>
        /// 目录名
        /// </summary>
        public string MenuName;
        /// <summary>
        /// 序号
        /// </summary>
        public int Order;
        /// <summary>
        /// 目标
        /// </summary>
        public string Target;
        /// <summary>
        /// 角色列表
        /// </summary>
        public int[] Roles;
        /// <summary>
        /// 子目录
        /// </summary>
        public IList<MenuItemType> SubMenuItems;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected;
        /// <summary>
        /// 图片
        /// </summary>
        public string ImgUrl;

    }
}
