// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月28日 14:49
// 
// 修改于：2013年02月18日 13:52
// 文件名：ITicketStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;

namespace Dev.CasServer.TicketStorage
{
    /// <summary>
    ///   Ticket 存储接口,
    ///   created by zbw911
    /// </summary>
    internal interface ITicketStorage
    {
        #region Instance Methods

        /// <summary>
        ///   存储 ticket
        /// </summary>
        /// <param name="strTicket"> </param>
        /// <param name="ticket"> </param>
        /// <param name="dateTime"> </param>
        void Add(string strTicket, CasTicket ticket, DateTime dateTime);

        /// <summary>
        ///   取得 ticket
        /// </summary>
        /// <param name="strTicket"> </param>
        /// <returns> </returns>
        CasTicket Get(string strTicket);

        /// <summary>
        ///   移除 ticket
        /// </summary>
        /// <param name="strTicket"> </param>
        void Remove(string strTicket);

        #endregion
    }
}