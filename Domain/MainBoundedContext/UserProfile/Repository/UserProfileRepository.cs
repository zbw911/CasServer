// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.MainBoundedContext/UserProfileRepository.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Data.Entity;
using Dev.Data;

namespace Domain.MainBoundedContext.UserProfile.Repository
{
    public class UserProfileRepository : GenericRepository<Entities.Models.UserProfile>, IUserProfileRepository
    {
        #region C'tors

        /// <summary>
        ///   Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName"> Name of the connection string. </param>
        public UserProfileRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context"> The context. </param>
        public UserProfileRepository(DbContext context)
            : base(context)
        {
        }

        #endregion

        #region IUserProfileRepository Members

        public string GetPhoneByUserId(int userid)
        {
            var user = this.FindOne(x => x.UserId == userid);
            if (user == null)
                return null;
            return user.Phone;
        }

        public void CreatePhonePasswordResetToken(int userid, string code, int tokenExpirationInMinutesFromNow)
        {
            var user = this.FindOne(x => x.UserId == userid);
            if (user == null)
                throw new Exception("不存在的用户");

            user.PhonePasswordResetToken = code;
            user.PhonePasswordResetTokenExpirationDate = System.DateTime.Now.AddMinutes(tokenExpirationInMinutesFromNow);
            user.LastPhonePasswordResetTokenTime = System.DateTime.Now;
            if (user.LastPhonePasswordResetTokenTime == null ||
                user.LastPhonePasswordResetTokenTime.Value.AddHours(1) < System.DateTime.Now)
                user.PhonePasswordResendCount = 0;

            user.PhonePasswordResendCount = user.PhonePasswordResendCount ?? 0;
            user.PhonePasswordResendCount++;
            this.Update(user);
            this.UnitOfWork.SaveChanges();
        }

        public void ResetTokenCount(string userName)
        {
            var user = this.FindOne(x => x.UserName == userName);
            if (user == null)
                throw new Exception("不存在的用户");

            user.PhonePasswordResendCount = 0;
            this.Update(user);
            this.UnitOfWork.SaveChanges();
        }

        #endregion
    }
}