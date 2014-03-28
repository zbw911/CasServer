using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.util;

namespace NCAS.Test.SpringConfig
{
    public class UniqueIdGeneratorsConfig
    {
        private static string hostname = "hostname";
        static public Dictionary<string, UniqueTicketIdGenerator> GetUniqueTicketIdGeneratorsForService()
        {
            var ticketGrantingTicketUniqueIdGenerator = new DefaultUniqueTicketIdGenerator(50, hostname);
            var serviceTicketUniqueIdGenerator = new DefaultUniqueTicketIdGenerator(20, hostname);
            var loginTicketUniqueIdGenerator = new DefaultUniqueTicketIdGenerator(30);
            var proxy20TicketUniqueIdGenerator = new DefaultUniqueTicketIdGenerator(20, hostname);
            var samlServiceTicketUniqueIdGenerator = new SamlCompliantUniqueTicketIdGenerator("https://localhost:8443");

            //    <entry
            //    key="org.jasig.cas.authentication.principal.SimpleWebApplicationServiceImpl"
            //    value-ref="serviceTicketUniqueIdGenerator" />
            //<entry
            //    key="org.jasig.cas.support.openid.authentication.principal.OpenIdService"
            //    value-ref="serviceTicketUniqueIdGenerator" />
            //<entry key="org.jasig.cas.authentication.principal.SamlService"
            //    value-ref="samlServiceTicketUniqueIdGenerator" />
            //<entry key="org.jasig.cas.authentication.principal.GoogleAccountsService"
            //    value-ref="serviceTicketUniqueIdGenerator" />
            Dictionary<string, UniqueTicketIdGenerator> uniqueTicketIdGeneratorsForService = new Dictionary
                <string, UniqueTicketIdGenerator>
                                                                                                 {
                                                                                                     {typeof(SimpleWebApplicationServiceImpl).FullName,serviceTicketUniqueIdGenerator},
                                                                                                     
                                                                                                 };


            return uniqueTicketIdGeneratorsForService;
        }
    }
}
