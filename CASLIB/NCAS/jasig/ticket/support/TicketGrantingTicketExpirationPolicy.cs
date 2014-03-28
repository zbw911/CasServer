/*
 * Licensed to Jasig under one or more contributor license
 * agreements. See the NOTICE file distributed with this work
 * for additional information regarding copyright ownership.
 * Jasig licenses this file to you under the Apache License,
 * Version 2.0 (the "License"); you may not use this file
 * except in compliance with the License.  You may obtain a
 * copy of the License at the following location:
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
//package org.jasig.cas.ticket.support;

//import org.jasig.cas.ticket.ExpirationPolicy;
//import org.jasig.cas.ticket.TicketState;
//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;
//import org.springframework.beans.factory.InitializingBean;
//import org.springframework.util.Assert;

//import java.util.concurrent.TimeUnit;

/**
 * Provides the Ticket Granting Ticket expiration policy.  Ticket Granting Tickets
 * can be used any number of times, have a fixed lifetime, and an idle timeout.
 *
 * @author William G. Thompson, Jr.
 * @version $Revision$ $Date$
 * @since 3.4.10
 */

namespace NCAS.jasig.ticket.support
{
    public class TicketGrantingTicketExpirationPolicy : ExpirationPolicy
    {

        //private static  Logger log = LoggerFactory.getLogger(TicketGrantingTicketExpirationPolicy.class);

        /** Static ID for serialization. */
        private static long serialVersionUID = 2136490343650084287L;

        /** Maximum time this ticket is valid  */
        private long _maxTimeToLiveInMilliSeconds;

        /** Time to kill in milliseconds. */
        private long _timeToKillInMilliSeconds;

        public TicketGrantingTicketExpirationPolicy(long maxTimeToLiveInMilliSeconds, long timeToKillInMilliSeconds)
        {
            _maxTimeToLiveInMilliSeconds = maxTimeToLiveInMilliSeconds;
            _timeToKillInMilliSeconds = timeToKillInMilliSeconds;
        }


        public void setMaxTimeToLiveInMilliSeconds(long maxTimeToLiveInMilliSeconds)
        {
            this._maxTimeToLiveInMilliSeconds = maxTimeToLiveInMilliSeconds;
        }

        public void setTimeToKillInMilliSeconds(long timeToKillInMilliSeconds)
        {
            this._timeToKillInMilliSeconds = timeToKillInMilliSeconds;
        }

        ///** Convenient virtual property setter to set time in seconds */
        public void setMaxTimeToLiveInSeconds(long maxTimeToLiveInSeconds)
        {
            if (this._maxTimeToLiveInMilliSeconds == 0L)
            {
                this._maxTimeToLiveInMilliSeconds = maxTimeToLiveInSeconds * 1000;
            }
        }

        /** Convenient virtual property setter to set time in seconds */
        public void setTimeToKillInSeconds(long timeToKillInSeconds)
        {
            if (this._timeToKillInMilliSeconds == 0L)
            {
                this._timeToKillInMilliSeconds = timeToKillInSeconds * 1000;
            }
        }

        public void afterPropertiesSet()
        {
            //Assert.isTrue((maxTimeToLiveInMilliSeconds >= timeToKillInMilliSeconds), "maxTimeToLiveInMilliSeconds must be greater than or equal to timeToKillInMilliSeconds.");
        }

        public bool isExpired(TicketState ticketState)
        {
            // Ticket has been used, check maxTimeToLive (hard window)
            if ((System.DateTime.Now.Ticks - ticketState.getCreationTime() >= this._maxTimeToLiveInMilliSeconds))
            {
                //if (log.isDebugEnabled()) {
                //    log.debug("Ticket is expired due to the time since creation being greater than the maxTimeToLiveInMilliSeconds");
                //}
                return true;
            }

            // Ticket is within hard window, check timeToKill (sliding window)
            if ((System.DateTime.Now.Ticks - ticketState.getLastTimeUsed() >= this._timeToKillInMilliSeconds))
            {
                //if (log.isDebugEnabled()) {
                //    log.debug("Ticket is expired due to the time since last use being greater than the timeToKillInMilliseconds");
                //}
                return true;
            }

            return false;
        }

    }
}
