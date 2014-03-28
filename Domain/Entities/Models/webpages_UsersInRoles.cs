// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.Entities/webpages_UsersInRoles.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

namespace Domain.Entities.Models
{
    public partial class webpages_UsersInRoles
    {
        #region Instance Properties

        public virtual UserExtend UserExtend { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual webpages_Roles webpages_Roles { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        #endregion
    }
}