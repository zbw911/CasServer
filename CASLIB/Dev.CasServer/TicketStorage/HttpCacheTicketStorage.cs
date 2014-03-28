// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月28日 15:35
// 
// 修改于：2013年02月18日 13:52
// 文件名：HttpCacheTicketStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Web;
using System.Web.Caching;

namespace Dev.CasServer.TicketStorage
{
    internal class HttpCacheTicketStorage : ITicketStorage
    {
        #region Readonly & Static Fields

        private readonly HttpContext _httpContext = HttpContext.Current;

        #endregion

        #region ITicketStorage Members

        public void Add(string strTicket, CasTicket ticket, DateTime dateTime)
        {
            this._httpContext.Cache.Add(strTicket, ticket, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration,
                                        CacheItemPriority.Normal, null);
        }

        public CasTicket Get(string strTicket)
        {
            return this._httpContext.Cache.Get(strTicket) as CasTicket;
        }

        public void Remove(string strTicket)
        {
            this._httpContext.Cache.Remove(strTicket);
        }

        #endregion
    }
}