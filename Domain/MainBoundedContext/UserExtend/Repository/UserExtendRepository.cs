﻿// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.MainBoundedContext/UserProfileRepository.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Data.Entity;
using Dev.Data;

namespace Domain.MainBoundedContext.UserExtend.Repository
{
    public class UserExtendRepository : GenericRepository<Entities.Models.UserExtend>, IUserExtendRepository
    {
        #region C'tors

        /// <summary>
        ///   Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName"> Name of the connection string. </param>
        public UserExtendRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context"> The context. </param>
        public UserExtendRepository(DbContext context)
            : base(context)
        {
        }

        #endregion
    }
}