// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.MainBoundedContext/IUserProfileRepository.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using Dev.Data.Infras;

namespace Domain.MainBoundedContext.UserProfile.Repository
{
    public interface IUserProfileRepository : IRepository<Entities.Models.UserProfile>
    {
        #region Instance Methods

        void CreatePhonePasswordResetToken(int userid, string code, int tokenExpirationInMinutesFromNow = 1440);
        string GetPhoneByUserId(int userid);
        void ResetTokenCount(string userName);

        #endregion
    }
}