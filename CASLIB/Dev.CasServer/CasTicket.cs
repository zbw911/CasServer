// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月26日 11:30
// 
// 修改于：2013年02月18日 13:52
// 文件名：CasTicket.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Web;
using Dev.CasServer.TicketStorage;

namespace Dev.CasServer
{
    /// <summary>
    ///   Representation of a temporary CAS service validation ticket
    /// </summary>
    public class CasTicket
    {
        #region Readonly & Static Fields

        private static uint _nCounter;

        private readonly string _strService;
        private readonly string _strUserName;
        private static readonly ITicketStorage TicketStorage;

        #endregion

        #region C'tors

        static CasTicket()
        {
            TicketStorage = new HttpCacheTicketStorage();
        }

        private CasTicket(string strUserName, string strService)
        {
            this._strUserName = strUserName;
            this._strService = strService;
        }

        #endregion

        #region Class Methods

        /// <summary>
        ///   Read a ticket from cache, check and immedeately invalidate (punch) it
        /// </summary>
        /// <param name="strTicket"> </param>
        /// <param name="strService"> Returns the corresponding service </param>
        /// <returns> Returns the corresponding user name </returns>
        public static string CheckAndPunch(string strTicket, ref string strService)
        {
            var ticket = TicketStorage.Get(strTicket);

            if (ticket == null) return "";

            TicketStorage.Remove(strTicket);

            // return ticket data
            strService = ticket._strService;
            return ticket._strUserName;
        }

        /// <summary>
        ///   Issue a CAS ticket and store it temporarily in the context cache
        /// </summary>
        /// <param name="strUserName"> </param>
        /// <param name="strService"> </param>
        /// <returns> Returns the corresponding ticket string. </returns>
        public static string Issue(string strUserName, string strService)
        {
            // create the ticket
            var ticket = new CasTicket(strUserName, strService);

            // create 120 bit random data
            var rdata = new byte[15];
            var random = new Random(DateTime.Now.Millisecond);
            random.NextBytes(rdata);

            // convert random data to an URL save token of 20 characters length
            var strToken = HttpServerUtility.UrlTokenEncode(rdata);

            // build the CAS ticket string
            var strTicket = "ST-" + (++_nCounter).ToString() + "-" + strToken + "-cas";


            TicketStorage.Add(strTicket, ticket, DateTime.Now.AddMinutes(1));
            return strTicket;
        }

        #endregion
    }
}