// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.MainBoundedContext/IUserExtendRepository.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using Dev.Data.Infras;

namespace Domain.MainBoundedContext.UserExtend.Repository
{
    public interface IUserExtendRepository : IRepository<Entities.Models.UserExtend>
    {
    }
}