// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/UserController.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Collections.Generic;
using System.Web.Http;
using Application.Dto.User;
using Application.MainBoundedContext.UserModule;
using Dev.CasServer.Authenticator;
using Dev.Comm.Web.Mvc.Filter;
using Dev.Framework.FileServer;
using WebMatrix.WebData;

namespace CASServer.ApiControllers
{
    public class UserController : ApiController
    {
        #region Readonly & Static Fields

        private readonly IKey _key;
        private readonly IUserService _userService;

        #endregion

        #region Fields

        private ICasAuthenticator _casAuthenticator;

        #endregion

        #region C'tors

        public UserController(IUserService userService, IKey key)
        {
            this._userService = userService;
            this._key = key;
            this._casAuthenticator = new FormsCasAuthenticator();
        }

        #endregion

        #region Instance Methods

        public string AvatarLoader()
        {
            return "";
        }

        [WebApiAllowCrossSiteJson]
        [HttpGet, HttpPost]
        public bool CheckNick([FromUri] string nickname)
        {
            return this._userService.UserNickExist(nickname);
        }

        [WebApiAllowCrossSiteJson]
        public UserInfo Get()
        {
            return new UserInfo
                       {
                           UserId = WebSecurity.CurrentUserId,
                           UserName = WebSecurity.CurrentUserName
                       };
        }

        [HttpGet, HttpPost]
        public DateTime? GetRegDateTime([FromUri] decimal uid)
        {
            return this._userService.GetRegDateTime(uid);
        }

        [WebApiAllowCrossSiteJson]
        public UserProfileModel GetUserInfo([FromUri] decimal uid)
        {
            return this._userService.GetUserProfileByCache(uid);
        }

        public UserProfileModel GetUserInfoByNickname([FromUri] string nickname)
        {
            return this._userService.GetUserProfileByNickName(nickname);
        }

        public UserProfileModel GetUserInfoByUserName([FromUri] string username)
        {
            return this._userService.GetUserProfileByUserName(username);
        }

        public List<UserProfileModel> GetUserInfoList([FromUri] decimal[] uids)
        {
            return this._userService.GetUserProfiles(uids);
        }

        public List<UserProfileModel> GetUserInfoListByNickNames([FromUri] string[] nicknames)
        {
            return this._userService.GetUserProfileByNickNames(nicknames);
        }

        #endregion

        //[WebApiAllowCrossSiteJson]
        //public UserProfileModel GetUserInfo(decimal[] uid)
        //{
        //    return userService.GetUserProfileByCache(uid);
        //}
    }
}