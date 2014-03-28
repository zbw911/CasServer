// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.Dto/GetPwdModel.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CASServer.Models
{
    public class GetPwdModel
    {
        #region Instance Properties

        [DefaultValue(0)]
        public int GetPwdType { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "验证码")]
        [Required(ErrorMessage = "验证码是必须的")]
        [StringLength(4, ErrorMessage = "{0}最大长度为{1}")]
        public string Validcode { get; set; }

        #endregion
    }
}