// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.Entities/sysdiagram.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;

namespace Domain.Entities.Models
{
    public partial class sysdiagram
    {
        #region Instance Properties

        public byte[] definition { get; set; }
        public int diagram_id { get; set; }
        public string name { get; set; }
        public int principal_id { get; set; }
        public Nullable<int> version { get; set; }

        #endregion
    }
}