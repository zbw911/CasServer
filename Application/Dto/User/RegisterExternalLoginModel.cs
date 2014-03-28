// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.Dto/RegisterExternalLoginModel.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.ComponentModel.DataAnnotations;

namespace CASServer.Models
{
    public class RegisterExternalLoginModel
    {
        #region Instance Properties

        public string ExternalLoginData { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        #endregion
    }
}