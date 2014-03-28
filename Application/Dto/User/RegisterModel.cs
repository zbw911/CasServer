// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.Dto/RegisterModel.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dev.Comm.Web.Mvc.Validate;

namespace CASServer.Models
{
    public class RegisterModel
    {
        #region Instance Properties

        [Required(ErrorMessage = "所在地是必选项")]
        [Display(Name = "市")]
        public int City { get; set; }

        [Required]
        [Display(Name = "市名")]
        public string CityName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }


        [EnforceTrue(ErrorMessage = "同意用户协议为必选项")]
        [Display(Name = "用户协议")]
        public bool IsAgree { get; set; }

        [Required]
        [Display(Name = "昵称")]
        //[^￥#$~!@%^&*();'\"?><[\\]{}\\|,:/=+—“”‘]
        [RegularExpression(@"^[a-zA-Z0-9\u4e00-\u9fa5]+$", ErrorMessage = @"{0}只能使用汉字、字母、数字")]
        //[StringLength(14, MinimumLength = 2, ErrorMessage = "{0}长度为在{2}-{1}之间")]
        [CnStringLength(14, MinimumLength = 4, ErrorMessage = "长度在4-14字符之间")]
        public string NickName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "省")]
        public int Province { get; set; }

        [Required]
        [Display(Name = "省名")]
        public string ProvinceName { get; set; }

        [Required]
        [Display(Name = "性别")]
        public int Sex { get; set; }

        [Required]
        [Display(Name = "邮件地址")]
        //[EmailAddress(ErrorMessage = "不是有效的邮件地址")]
        //[Remote("check", "account", ErrorMessage = "此用户不能使用，可能已经被占用或不合法")]
        [StringLength(100, ErrorMessage = "{0} 必须最多使用 {1} 个字符。")]
        [RegularExpression(
            "^(([0-9a-zA-Z]+)|([0-9a-zA-Z]+[_.0-9a-zA-Z-]*[0-9a-zA-Z]+))@([a-zA-Z0-9-]+[.])+(net|NET|com|COM|gov|GOV|mil|MIL|org|ORG|edu|EDU|int|INT|cn|CN)$"
            , ErrorMessage = "不是邮件地址")]
        public string UserName { get; set; }

        [Display(Name = "验证码")]
        [Required(ErrorMessage = "验证码是必须的")]
        [StringLength(4, ErrorMessage = "{0}最大长度为{1}")]
        public string Validcode { get; set; }

        #endregion
    }
}