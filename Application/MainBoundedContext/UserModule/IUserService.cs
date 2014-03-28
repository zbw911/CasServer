// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.MainBoundedContext/IUserService.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Collections.Generic;
using Application.Dto;
using Application.Dto.User;
using CASServer.Models;

namespace Application.MainBoundedContext.UserModule
{
    public interface IUserService
    {
        #region Instance Methods

        BaseState ChangeNick(int userid, string nickname);
        BaseState ChangeSex(int userid, int sex);
        BaseState CheckPhoneToken(string username, string token);

        /// <summary>
        ///   在使用
        /// </summary>
        /// <param name="token"> </param>
        void ConfirmEmail(string token);

        /// <summary>
        ///   取得用户昵称
        /// </summary>
        /// <param name="email"> </param>
        /// <returns> </returns>
        string GetNickNameByUserName(string email);

        /// <summary>
        ///   找回密码类的方法
        /// </summary>
        /// <param name="baseurl"> </param>
        /// <param name="model"> </param>
        /// <returns> </returns>
        BaseState GetPassWord(string baseurl, GetPwdModel model);

        System.DateTime? GetRegDateTime(decimal uid);

        /// <summary>
        ///   取得确认token
        /// </summary>
        /// <param name="email"> </param>
        /// <returns> </returns>
        string GetTokenByUserName(string email);

        /// <summary>
        ///   要用用户id 取得 Uid
        /// </summary>
        /// <param name="userid"> </param>
        /// <returns> </returns>
        decimal GetUidByUserId(int userid);

        /// <summary>
        ///   用户头像
        /// </summary>
        /// <param name="UserId"> </param>
        /// <returns> </returns>
        string GetUserAvatar(int UserId);

        /// <summary>
        ///   根据用户Uid 取得 头像key
        /// </summary>
        /// <param name="Uid"> </param>
        /// <returns> </returns>
        string GetUserAvatarByUid(decimal Uid);

        /// <summary>
        ///   根据token找到用户名
        /// </summary>
        /// <param name="restToken"> </param>
        /// <returns> </returns>
        string GetUserEmailByRestToken(string restToken);

        /// <summary>
        ///   通过确认token取得用户userid
        /// </summary>
        /// <param name="token"> </param>
        /// <returns> </returns>
        int GetUserIdByConfirmToken(string token);

        /// <summary>
        ///   根据用户编号读取用户数据
        /// </summary>
        /// <param name="userId"> </param>
        /// <returns> </returns>
        UserProfileModel GetUserProfile(decimal uid);

        /// <summary>
        ///   根据用户编号读取用户数据
        /// </summary>
        /// <param name="userId"> </param>
        /// <returns> </returns>
        UserProfileModel GetUserProfileByCache(decimal uid);

        UserProfileModel GetUserProfileByNickName(string nickname);
        List<UserProfileModel> GetUserProfileByNickNames(string[] nicknames);

        UserProfileModel GetUserProfileByUserName(string username);
        List<UserProfileModel> GetUserProfiles(decimal[] uids);

        /// <summary>
        /// </summary>
        /// <param name="userid"> </param>
        /// <returns> </returns>
        decimal InserOrUpdateExtUid(int userid);

        /// <summary>
        ///   是否已经激活
        /// </summary>
        /// <param name="token"> </param>
        /// <returns> </returns>
        bool IsConfirmedByToken(string token);

        /// <summary>
        ///   通过 token重置密码
        /// </summary>
        /// <param name="token"> </param>
        /// <param name="NewPassword"> </param>
        /// <returns> </returns>
        bool ResetPasswordByEmailToken(string token, string NewPassword);

        /// <summary>
        ///   重置手机
        /// </summary>
        /// <param name="phoneNumber"> </param>
        /// <param name="token"> </param>
        /// <param name="newPassword"> </param>
        /// <param name="username"> </param>
        /// <returns> </returns>
        BaseState ResetPasswordByPhoneToken(string token, string newPassword, string username);

        /// <summary>
        ///   更新头像
        /// </summary>
        /// <param name="Userid"> </param>
        /// <param name="key"> </param>
        void UpdateUserAvatar(int Userid, string key);

        /// <summary>
        ///   昵称
        /// </summary>
        /// <returns> </returns>
        bool UserNickExist(string nickname);

        #endregion

        bool AddUserProfiles(UserProfileModel userprofiles);
    }
}