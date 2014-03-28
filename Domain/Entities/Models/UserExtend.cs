// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.Entities/UserExtend.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Collections.Generic;

namespace Domain.Entities.Models
{
    public partial class UserExtend
    {
        #region C'tors

        public UserExtend()
        {
            this.webpages_UsersInRoles = new List<webpages_UsersInRoles>();
        }

        #endregion

        #region Instance Properties

        public virtual ICollection<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
        public decimal Uid { get; set; }
        public int UserId { get; set; }

        #endregion
    }
}