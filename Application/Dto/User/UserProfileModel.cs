// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.Dto/UserProfileModel.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;

namespace Application.Dto.User
{
    public class UserProfileModel
    {
        #region Instance Properties

        public string Avator { get; set; }
        public Nullable<int> City { get; set; }
        public string CityName { get; set; }

        public string Email { get; set; }
        public Nullable<bool> IsConfirmEmail { get; set; }
        public Nullable<bool> IsConfirmPhone { get; set; }
        public string NickName { get; set; }
        public string Phone { get; set; }
        public Nullable<int> Province { get; set; }
        public string ProvinceName { get; set; }
        public Nullable<int> Sex { get; set; }
        public decimal Uid { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        #endregion

        //public string PhonePasswordResetToken { get; set; }
        //public Nullable<System.DateTime> PhonePasswordResetTokenExpirationDate { get; set; }
        //public Nullable<int> PhonePasswordResendCount { get; set; }
        //public Nullable<System.DateTime> LastPhonePasswordResetTokenTime { get; set; }
        //public virtual ICollection<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
    }
}