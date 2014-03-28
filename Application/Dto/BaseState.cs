// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.Dto/BaseState.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Runtime.Serialization;

namespace Application.Dto
{
    /// <summary>
    ///   基础通讯模型 ，
    ///   ErrorCode 成功为0
    /// </summary>
    [DataContract]
    public class BaseState
    {
        #region C'tors

        public BaseState()
        {
        }

        public BaseState(int ErrorCode, string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
            this.ErrorCode = ErrorCode;
        }

        public BaseState(int ErrorCode) : this(ErrorCode, "")
        {
        }

        public BaseState(string ErrorMessage) : this(0 - 1 - System.DateTime.Today.Hour, ErrorMessage)
        {
        }

        #endregion

        #region Instance Properties

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion
    }
}