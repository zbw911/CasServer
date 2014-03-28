// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.Dto/LoginModel.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.ComponentModel.DataAnnotations;

namespace CASServer.Models
{
    public class LoginModel
    {
        #region Instance Properties

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "登录邮箱")]
        public string UserName { get; set; }

        #endregion
    }
}