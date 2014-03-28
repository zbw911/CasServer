// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/CompositionRoot/RegisterFileServer.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using Dev.Framework.FileServer;
using Dev.Framework.FileServer.Config;
using Dev.Framework.FileServer.DocFile;
using Dev.Framework.FileServer.ImageFile;
using Dev.Framework.FileServer.ShareImpl;
using Dev.Web.CompositionRootBase;
using Ninject;

namespace CompositionRoot
{
    internal class RegisterFileServer : IRegister
    {
        #region IRegister Members

        public void Register()
        {
            var x = new ReadConfig();

            //公用方法 
            this.Kernel.Bind<IKey>().To<ShareFileKey>();
            this.Kernel.Bind<IUploadFile>().To<ShareUploadFile>();

            //文档类型
            this.Kernel.Bind<IDocFile>().To<DocFileUploader>();
            //图片类型
            this.Kernel.Bind<IImageFile>().To<ImageUploader>();
        }

        public IKernel Kernel { get; set; }

        #endregion
    }
}