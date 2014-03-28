// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/CompositionRoot/RegisterCache.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using Dev.Framework.Cache;
using Dev.Framework.Cache.Impl;
using Dev.Web.CompositionRootBase;
using Ninject;

namespace CompositionRoot
{
    internal class RegisterCache : IRegister
    {
        #region IRegister Members

        public void Register()
        {
            //暂时先使用 httpruntime cache
            this.Kernel.Bind<ICacheState>().To<HttpRuntimeCache>();
            this.Kernel.Bind<ICacheWraper>().To<CacheWraper>();
        }

        public IKernel Kernel { get; set; }

        #endregion
    }
}