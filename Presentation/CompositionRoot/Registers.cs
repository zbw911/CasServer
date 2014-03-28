// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/CompositionRoot/Registers.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using Application.CasHander;
using Application.MainBoundedContext.UserModule;
using Dev.CasServer;
using Dev.CasServer.Authenticator;
using Dev.Data;
using Dev.Data.Configuration;
using Dev.Data.ContextStorage;
using Dev.Web.CompositionRootBase;
using Domain.Entities.Models;
using Domain.MainBoundedContext.UserExtend.Repository;
using Domain.MainBoundedContext.UserProfile.Repository;
using Domain.MainBoundedContext.webpages_Membership.Repository;
using Ninject;

namespace CompositionRoot
{
    public class Registers : RegisterContextBase, IRegister
    {
        #region Constants

        private const string DefaultConnection = "DefaultConnection";

        #endregion

        //private const string SysManagerContext = "SysManagerContext1";

        #region IRegister Members

        public override IKernel Kernel { get; set; }

        public override void Register()
        {
            this.Kernel.Bind<IDbContextStorage>().To<WebDbContextStorage>();

            //var name = Dev.Comm.Core.AssemblyManager.GetAssemblyFileName("Dev.Demo.Mapping");


            CommonConfig.Instance()
                .ConfigureDbContextStorage(this.Kernel.Get<IDbContextStorage>())
                .ConfigureData<passportContext>(DefaultConnection, createDbFileImmediately: true);


            //CommonConfig.Instance()
            //   .ConfigureDbContextStorage(this.Kernel.Get<IDbContextStorage>())
            //   .ConfigureData(DefaultConnection, new[] { System.Reflection.Assembly.Load("Domain.Entities") }, false, false);


            // Repository
            this.RegContextWith<IUserProfileRepository, UserProfileRepository>(DefaultConnection);
            this.RegContextWith<IUserExtendRepository, UserExtendRepository>(DefaultConnection);
            this.RegContextWith<IWebpagesMembershipRepository, WebpagesMembershipRepository>(DefaultConnection);


            ////Service 
            this.RegServiceWith<IUserService, UserService>();


            //CAS
            this.RegServiceWith<ICasAuthenticator, FormsCasAuthenticator>();
            this.RegServiceWith<IUserValidate, UserValidateFake>();


          
        }

        #endregion
    }
}