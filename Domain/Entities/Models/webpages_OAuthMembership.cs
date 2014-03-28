// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.Entities/webpages_OAuthMembership.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

namespace Domain.Entities.Models
{
    public partial class webpages_OAuthMembership
    {
        #region Instance Properties

        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public int UserId { get; set; }

        #endregion
    }
}