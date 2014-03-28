// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.CasHander/UserValidateFake.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Web;
using Dev.CasServer;
using Domain.MainBoundedContext.UserExtend.Repository;
using Domain.MainBoundedContext.UserProfile.Repository;
using Domain.MainBoundedContext.webpages_Membership.Repository;
using WebMatrix.WebData;

namespace Application.CasHander
{
    /// <summary>
    ///   一个假的用户登录判断，这个随后应该是去业务中进行判断的
    /// </summary>
    public class UserValidateFake : IUserValidate
    {
        #region Readonly & Static Fields

        private readonly IUserExtendRepository _userExtendRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        #endregion

        #region Fields

        private IWebpagesMembershipRepository _webpagesMembershipRepository;

        #endregion

        #region C'tors

        public UserValidateFake(IUserProfileRepository userProfileRepository, IUserExtendRepository userExtendRepository,
                                IWebpagesMembershipRepository webpagesMembershipRepository)
        {
            this._userExtendRepository = userExtendRepository;
            this._userProfileRepository = userProfileRepository;
            this._webpagesMembershipRepository = webpagesMembershipRepository;
        }

        #endregion

        #region IUserValidate Members

        public bool PerformAuthentication(string strUserName, string strPassWord, bool rembers, out string ErrorMsg)
        {
            HttpContext.Current.Response.AddHeader("P3P",
                                                   "CP=\"CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR\"");
            ErrorMsg = "";
            //WebSecurity.ConfirmAccount()
            if (WebSecurity.Login(strUserName, strPassWord, rembers))
            {
                return true;
            }

            return false;
        }

        public object GetExtendProperty(string strUserName)
        {
            var userid = WebSecurity.GetUserId(strUserName);

            var userExtend = this._userExtendRepository.Single(x => x.UserId == userid);

            var uid = userExtend.Uid;

            var profile = this._userProfileRepository.Single(x => x.UserId == userid);


            var obj = new
                          {
                              Uid = uid,
                              profile.Sex,
                              profile.NickName,
                              profile.City,
                              profile.Province,
                              profile.UserName,
                              profile.Email,
                              profile.Avator,
                              profile.CityName,
                              profile.ProvinceName,
                              profile.UserId
                          };

            return obj;
        }

        #endregion
    }
}