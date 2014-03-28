// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.MainBoundedContext/UserService.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Application.Dto.User;
using CASServer.Models;
using Dev.Crosscutting.Adapter;
using Dev.Framework.Cache;
using Dev.Framework.FileServer;
using Domain.Entities.Models;
using Domain.MainBoundedContext.UserExtend.Repository;
using Domain.MainBoundedContext.UserProfile.Repository;
using Domain.MainBoundedContext.webpages_Membership.Repository;
using WebMatrix.WebData;

namespace Application.MainBoundedContext.UserModule
{
    public class UserService : IUserService
    {
        #region Readonly & Static Fields

        private readonly ICacheWraper _cacheWraper;
        private readonly IImageFile _imageFile;
        private readonly IUserExtendRepository _userExtendRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IWebpagesMembershipRepository _webpagesMembershipRepository;

        #endregion

        #region C'tors

        public UserService(IUserProfileRepository userProfileRepository,
                           IUserExtendRepository userExtendRepository,
                           IWebpagesMembershipRepository webpagesMembershipRepository,
                           ICacheWraper cacheWraper, IImageFile imageFile)
        {
            this._userExtendRepository = userExtendRepository;
            this._userProfileRepository = userProfileRepository;
            this._webpagesMembershipRepository = webpagesMembershipRepository;
            this._cacheWraper = cacheWraper;
            this._imageFile = imageFile;
        }

        #endregion

        #region Instance Methods

        private string GeneratePhonePasswordResetToken(string userName)
        {
            var userid = WebSecurity.GetUserId(userName);
            var code = Dev.Comm.Security.CreateRandomCode(6);


            this._userProfileRepository.CreatePhonePasswordResetToken(userid, code);

            return code;
        }

        private BaseState GetPassWordByEmail(string baseurl, GetPwdModel model)
        {
            if (!Dev.Comm.Validate.Validate.IsEmail(model.UserName))
            {
                return new BaseState(-1, "用户名非邮箱!");
            }


            var nick = this.GetNickNameByUserName(model.UserName);
            var token = WebSecurity.GeneratePasswordResetToken(model.UserName);
            var mail = SystemMessagerManager.GetContentForGetPass(baseurl, nick, token);
            var isok = SystemMessagerManager.SendValidateMail(baseurl, model.UserName, nick, "找回密码", mail);

            if (isok)
            {
                return new BaseState();
            }

            return new BaseState { ErrorCode = -1, ErrorMessage = "发送邮件失败" };
        }

        private BaseState GetPassWordByPhone(GetPwdModel model)
        {
            var userid = WebSecurity.GetUserId(model.UserName);
            var uid = this.GetUidByUserId(userid);
            var profile = this._userProfileRepository.FindOne(x => x.UserId == userid);
            if (profile == null)
                return new BaseState(-1, "用户不存在");
            var phone = profile.Phone;
            if (string.IsNullOrEmpty(phone))
            {
                return new BaseState(-1, "用户还未设置手机号");
            }

            if (profile.LastPhonePasswordResetTokenTime.HasValue &&
                profile.LastPhonePasswordResetTokenTime.Value.AddMinutes(1) > System.DateTime.Now
                ||
                profile.PhonePasswordResendCount.HasValue && profile.PhonePasswordResendCount >= 5 &&
                profile.LastPhonePasswordResetTokenTime.HasValue &&
                profile.LastPhonePasswordResetTokenTime.Value.AddHours(1) > System.DateTime.Now)
            {
                return new BaseState(-1, "短信发送过于频繁，请稍后再试");
            }


            var code = this.GeneratePhonePasswordResetToken(model.UserName);

            var message = "尊敬的" + phone + "，您好！XXXXX发送给您的认证码是" + code + "，请在网站上输入，找回XXXXX密码。如非本人操作，请忽略。";
            var issend = SystemMessagerManager.SendSMS(phone, message, uid);

            if (!issend)
                return new BaseState(-1, "发送失败");

            return new BaseState(0, phone + "," + model.UserName);
        }

        #endregion

        #region IUserService Members

        public bool UserNickExist(string nickname)
        {
            return this._userProfileRepository.FindOne(x => x.NickName == nickname) != null;
        }

        public bool AddUserProfiles(UserProfileModel userprofiles)
        {
            this._userProfileRepository.Add(userprofiles.ProjectedAs<UserProfile>());

            this._userProfileRepository.UnitOfWork.SaveChanges();

            return true;
        }

        /// <summary>
        ///   根据用户编号读取用户数据
        /// </summary>
        /// <param name="userId"> </param>
        /// <returns> </returns>
        public UserProfileModel GetUserProfile(decimal uid)
        {
            var extend = this._userExtendRepository.FindOne(x => x.Uid == uid);

            if (extend == null)
                return null;


            var user =
                this._userProfileRepository.FindOne(m => m.UserId == extend.UserId).ProjectedAs<UserProfileModel>();
            user.Uid = uid;
            return user;
        }

        /// <summary>
        ///   根据用户编号读取用户数据
        /// </summary>
        /// <param name="userId"> </param>
        /// <returns> </returns>
        public UserProfileModel GetUserProfileByCache(decimal uid)
        {
            return this._cacheWraper.SmartyGetPut(uid, new TimeSpan(0, 1, 0), () => this.GetUserProfile(uid));
        }

        public List<UserProfileModel> GetUserProfiles(decimal[] uids)
        {
            var listuser =
                this._userExtendRepository.GetQuery(x => uids.Contains(x.Uid)).Select(x => new { x.UserId, x.Uid }).ToList
                    ();
            var listuserids = listuser.Select(x => x.UserId);
            var userprofiles = this._userProfileRepository.GetQuery(m => listuserids.Contains(m.UserId));


            var list = userprofiles.ProjectedAsCollection<UserProfileModel>();


            var listhasuid = new List<UserProfileModel>();

            list.ForEach(x =>
                             {
                                 x.Uid = listuser.First(y => y.UserId == x.UserId).Uid;
                                 listhasuid.Add(x);
                             });

            return list;
        }

        public UserProfileModel GetUserProfileByNickName(string nickname)
        {
            var user = this._userProfileRepository.FindOne(m => m.NickName == nickname).ProjectedAs<UserProfileModel>();

            if (user == null)
                return null;

            var extend = this._userExtendRepository.FindOne(x => x.UserId == user.UserId);

            if (extend == null)
                return null;

            user.Uid = extend.Uid;

            return user;
        }

        public List<UserProfileModel> GetUserProfileByNickNames(string[] nicknames)
        {
            var user =
                this._userProfileRepository.GetQuery(m => nicknames.Contains(m.NickName)).ProjectedAsCollection
                    <UserProfileModel>();

            if (user == null)
                return null;

            //return user;

            var userids = user.Select(x => x.UserId);


            var listuser =
                this._userExtendRepository.GetQuery(x => userids.Contains(x.UserId)).Select(x => new { x.UserId, x.Uid }).
                    ToList();

            listuser.ForEach(x => { user.First(y => y.UserId == x.UserId).Uid = x.Uid; });


            return user;

            //if (extend == null)
            //    return null;

            //user.UserId = extend.UserId;

            //return user;
        }

        public UserProfileModel GetUserProfileByUserName(string username)
        {
            var profile = this._userProfileRepository.FindOne(x => x.UserName == username);

            if (profile == null)
                return null;

            var uid = this._userExtendRepository.First(x => x.UserId == profile.UserId).Uid;

            var upm = profile.ProjectedAs<UserProfileModel>();

            upm.Uid = uid;

            return upm;
        }

        public BaseState ChangeNick(int userid, string nickname)
        {
            var profile = this._userProfileRepository.FindOne(x => x.UserId == userid);

            if (profile == null)
                return new BaseState(-1, "用户不存在");

            if (profile.NickName == nickname)
                return new BaseState(0, "未做修改");

            if (this.UserNickExist(nickname))
                return new BaseState(-1, nickname + "已经存在");

            profile.NickName = nickname;
            this._userProfileRepository.Update(profile);
            this._userProfileRepository.UnitOfWork.SaveChanges();

            return new BaseState();
        }

        public BaseState ChangeSex(int userid, int sex)
        {
            if (!new[] { 1, 2 }.Contains(sex))
            {
                return new BaseState(-1, "性别参数不正确");
            }

            var profile = this._userProfileRepository.FindOne(x => x.UserId == userid);

            if (profile == null)
                return new BaseState(-1, "用户不存在");


            profile.Sex = sex;
            this._userProfileRepository.Update(profile);
            this._userProfileRepository.UnitOfWork.SaveChanges();

            return new BaseState();
        }

        public DateTime? GetRegDateTime(decimal uid)
        {
            var extend = this._userExtendRepository.FindOne(x => x.Uid == uid);
            if (extend == null)
                return null;

            var wmr = this._webpagesMembershipRepository.FindOne(x => x.UserId == extend.UserId);

            if (wmr == null)
                return null;

            return wmr.CreateDate;
        }


        public decimal GetUidByUserId(int userid)
        {
            var ext = this._userExtendRepository.FindOne(x => x.UserId == userid);

            if (ext == null)
                return 0;

            return ext.Uid;
        }

        public decimal InserOrUpdateExtUid(int userid)
        {
            //new Domain.Entities.Models.UserExtend { UserId = userid };

            var model = this._userExtendRepository.FindOne(x => x.UserId == userid);

            if (model == null)
            {
                model = new Domain.Entities.Models.UserExtend { UserId = userid };
                this._userExtendRepository.Add(model);
                this._userExtendRepository.UnitOfWork.SaveChanges();
            }
            else
            {
                // nothing 
            }


            return model.Uid;
        }

        public bool IsConfirmedByToken(string token)
        {
            var profile = this._webpagesMembershipRepository.FindOne(x => x.ConfirmationToken == token);
            if (profile == null)
                return false;

            return profile.IsConfirmed != null && profile.IsConfirmed.Value;
        }

        public string GetNickNameByUserName(string email)
        {
            var profile = this._userProfileRepository.FindOne(x => x.UserName == email);
            return profile == null ? "" : profile.NickName;
        }

        public string GetTokenByUserName(string email)
        {
            var profile = this._userProfileRepository.Single(x => x.UserName == email);

            var msp = this._webpagesMembershipRepository.Single(x => x.UserId == profile.UserId);

            return msp.ConfirmationToken;
        }

        public BaseState GetPassWord(string baseurl, GetPwdModel model)
        {
            var exist = this._userProfileRepository.FindOne(x => x.Email == model.UserName);
            // .UserExists(model.UserName);
            if (exist == null)
            {
                return new BaseState(-2, "邮箱不正确");
            }

            if (!WebSecurity.IsConfirmed(model.UserName))
            {
                return new BaseState(-3, "此帐户还未激活");
            }


            if (model.GetPwdType == 0)
            {
                return this.GetPassWordByEmail(baseurl, model);
            }
            else if (model.GetPwdType == 1)
            {
                return this.GetPassWordByPhone(model);
            }
            throw new ArgumentException("类型只能为0,1", "model.GetPwdType");
        }

        public string GetUserEmailByRestToken(string restToken)
        {
            var userid = WebSecurity.GetUserIdFromPasswordResetToken(restToken);

            var user = this._userProfileRepository.FindOne(x => x.UserId == userid);
            if (user == null)
                return "";

            return user.UserName;
        }

        public bool ResetPasswordByEmailToken(string token, string NewPassword)
        {
            return WebSecurity.ResetPassword(token, NewPassword);
        }

        public BaseState ResetPasswordByPhoneToken(string token, string newPassword, string username)
        {
            //if (string.IsNullOrEmpty(phoneNumber))
            //    return new BaseState(-1, "手机号码不能为空");
            if (string.IsNullOrEmpty(token))
                return new BaseState(-1, "重置码不能为空");

            if (string.IsNullOrEmpty(newPassword))
            {
                return new BaseState(-1, "密码不能空");
            }

            var user = this._userProfileRepository.FindOne(
                x =>
                x.UserName == username
                );

            if (user == null)
            {
                return new BaseState(-1, "不存在的用户");
            }

            if (user.PhonePasswordResetToken.ToLower() != token.ToLower())
            {
                return new BaseState(-1, "重置码不正确");
            }

            if (user.PhonePasswordResetTokenExpirationDate <= System.DateTime.Now)
            {
                return new BaseState(-1, "手机重置码已经过期");
            }


            var passwordResetToken = WebSecurity.GeneratePasswordResetToken(user.UserName);
            var isrest = WebSecurity.ResetPassword(passwordResetToken, newPassword);

            if (isrest)
            {
                this._userProfileRepository.ResetTokenCount(userName: user.UserName);
                return new BaseState();
            }

            return new BaseState(-1, "重置失败");
        }

        public BaseState CheckPhoneToken(string username, string token)
        {
            var user = this._userProfileRepository.FindOne(x => x.UserName == username);
            if (user == null)
                return new BaseState(-1, "用户不存在");
            if (user.PhonePasswordResetToken != token)
                return new BaseState(-1, "重置码不正确");

            if (user.PhonePasswordResetTokenExpirationDate <= System.DateTime.Now)
                return new BaseState(-1, "重置码已经过期");

            return new BaseState();
        }

        /// <summary>
        ///   仅用于用户名是邮件的情况下
        /// </summary>
        /// <param name="token"> </param>
        public void ConfirmEmail(string token)
        {
            var usrid = this.GetUserIdByConfirmToken(token); // WebSecurity.GetUserIdFromPasswordResetToken(token);

            var user = this._userProfileRepository.Single(x => x.UserId == usrid);

            if (Dev.Comm.Validate.Validate.IsEmail(user.Email) && user.Email == user.UserName)
            {
                user.IsConfirmEmail = true;

                this._userProfileRepository.Update(user);
                this._userProfileRepository.UnitOfWork.SaveChanges();
            }
        }

        /// <summary>
        ///   通过确认token取得用户userid
        /// </summary>
        /// <param name="token"> </param>
        /// <returns> </returns>
        public int GetUserIdByConfirmToken(string token)
        {
            var msp = this._webpagesMembershipRepository.First(x => x.ConfirmationToken == token);
            return msp.UserId;
        }

        public void UpdateUserAvatar(int Userid, string key)
        {
            var user = this._userProfileRepository.GetByKey(Userid);
            user.Avator = key;
            this._userProfileRepository.Update(user);
            this._userProfileRepository.UnitOfWork.SaveChanges();
        }

        public string GetUserAvatar(int UserId)
        {
            var user = this._userProfileRepository.GetByKey(UserId);
            if (user == null)
                return null;

            return user.Avator;
        }

        public string GetUserAvatarByUid(decimal Uid)
        {
            var extend = this._userExtendRepository.FindOne(x => x.Uid == Uid);
            if (extend == null) return null;
            return this.GetUserAvatar(extend.UserId);
        }

        #endregion
    }
}