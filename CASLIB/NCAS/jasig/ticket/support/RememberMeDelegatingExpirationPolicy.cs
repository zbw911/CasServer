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

//import org.jasig.cas.authentication.principal.RememberMeCredentials;
//import org.jasig.cas.ticket.ExpirationPolicy;
//import org.jasig.cas.ticket.TicketState;

//import javax.validation.constraints.NotNull;

/**
 * Delegates to different expiration policies depending on whether remember me
 * is true or not.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.1 $ $Date: 2005/08/19 18:27:17 $
 * @since 3.2.1
 *
 */

using System.Linq;
using NCAS.jasig.authentication.principal;

namespace NCAS.jasig.ticket.support
{
    public class RememberMeDelegatingExpirationPolicy : ExpirationPolicy
    {

        /** Unique Id for Serialization */
        private static long serialVersionUID = -575145836880428365L;

        ////@NotNull
        private ExpirationPolicy rememberMeExpirationPolicy;

        ////@NotNull
        private ExpirationPolicy sessionExpirationPolicy;

        public bool isExpired(TicketState ticketState)
        {
            bool b =
                (bool)
                ticketState.getAuthentication().getAttributes().FirstOrDefault(
                    x => x.Key == typeof(RememberMeCredentials).FullName).Value;

            if (b == null || b.Equals(false))
            {
                return this.sessionExpirationPolicy.isExpired(ticketState);
            }

            return this.rememberMeExpirationPolicy.isExpired(ticketState);
        }

        public void setRememberMeExpirationPolicy(
            ExpirationPolicy rememberMeExpirationPolicy)
        {
            this.rememberMeExpirationPolicy = rememberMeExpirationPolicy;
        }

        public void setSessionExpirationPolicy(ExpirationPolicy sessionExpirationPolicy)
        {
            this.sessionExpirationPolicy = sessionExpirationPolicy;
        }
    }
}
