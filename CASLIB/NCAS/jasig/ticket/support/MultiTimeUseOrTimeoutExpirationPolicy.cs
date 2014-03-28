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
//import org.springframework.util.Assert;

//import java.util.concurrent.TimeUnit;

/**
 * ExpirationPolicy that is based on certain number of uses of a ticket or a
 * certain time period for a ticket to exist.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

namespace NCAS.jasig.ticket.support
{
    public class MultiTimeUseOrTimeoutExpirationPolicy :
        ExpirationPolicy
    {

        /** Serializable Unique ID. */
        private static long serialVersionUID = 3257844372614558261L;

        /** The time to kill in millseconds. */
        private long timeToKillInMilliSeconds;

        /** The maximum number of uses before expiration. */
        private int numberOfUses;

        public MultiTimeUseOrTimeoutExpirationPolicy(int numberOfUses,
                                                     long timeToKillInMilliSeconds)
        {
            this.timeToKillInMilliSeconds = timeToKillInMilliSeconds;
            this.numberOfUses = numberOfUses;
            //Assert.isTrue(this.numberOfUses > 0, "numberOfUsers must be greater than 0.");
            //Assert.isTrue(this.timeToKillInMilliSeconds > 0, "timeToKillInMilliseconds must be greater than 0.");

        }

        public MultiTimeUseOrTimeoutExpirationPolicy(int numberOfUses, long timeToKill, long timeUnit)
            : this(numberOfUses, timeToKill)
        {
            ;
        }

        public bool isExpired(TicketState ticketState)
        {
            return (ticketState == null)
                   || (ticketState.getCountOfUses() >= this.numberOfUses)
                   || (System.DateTime.Now.Ticks - ticketState.getLastTimeUsed() >= this.timeToKillInMilliSeconds);
        }
    }
}
