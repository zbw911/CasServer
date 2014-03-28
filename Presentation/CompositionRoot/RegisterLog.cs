// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/CompositionRoot/RegisterLog.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using Dev.Log;
using Dev.Log.Config;
using Dev.Web.CompositionRootBase;

namespace CompositionRoot
{
    internal class RegisterLog : IRegister
    {
        #region IRegister Members

        public Ninject.IKernel Kernel { get; set; }

        public void Register()
        {
            Setting.SetLogSeverity(LogSeverity.Debug);
            Setting.AttachLog(new Dev.Log.Impl.ObserverLogToLog4net());
        }

        #endregion
    }
}