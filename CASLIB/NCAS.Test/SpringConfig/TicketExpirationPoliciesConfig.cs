using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCAS.jasig.ticket;
using NCAS.jasig.ticket.support;

namespace NCAS.Test.SpringConfig
{
    class TicketExpirationPoliciesConfig
    {
        //      <bean id="serviceTicketExpirationPolicy" class="org.jasig.cas.ticket.support.MultiTimeUseOrTimeoutExpirationPolicy"
        //      c:numberOfUses="1" c:timeToKill="${st.timeToKillInSeconds:10}" c:timeUnit-ref="SECONDS"/>
        public static ExpirationPolicy serviceTicketExpirationPolicy = new MultiTimeUseOrTimeoutExpirationPolicy(1, 10);
        //<!-- TicketGrantingTicketExpirationPolicy: Default as of 3.5 -->
        //<!-- Provides both idle and hard timeouts, for instance 2 hour sliding window with an 8 hour max lifetime -->
        //<bean id="grantingTicketExpirationPolicy" class="org.jasig.cas.ticket.support.TicketGrantingTicketExpirationPolicy"
        //      p:maxTimeToLiveInSeconds="${tgt.maxTimeToLiveInSeconds:28800}"
        //      p:timeToKillInSeconds="${tgt.timeToKillInSeconds:7200}"/>

        public static ExpirationPolicy grantingTicketExpirationPolicy = new TicketGrantingTicketExpirationPolicy(28800, 7200);



    }
}
