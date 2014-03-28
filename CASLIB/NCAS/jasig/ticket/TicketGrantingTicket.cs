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
////package org.jasig.cas.ticket;

////import java.util.List;

////import org.jasig.cas.authentication.Authentication;
////import org.jasig.cas.authentication.principal.Service;

using System.Collections.Generic;
using System;
using NCAS.jasig.authentication;
using NCAS.jasig.authentication.principal;

/**
 * Interface for a ticket granting ticket. A TicketGrantingTicket is the main
 * access into the CAS service layer. Without a TicketGrantingTicket, a user of
 * CAS cannot do anything.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

namespace NCAS.jasig.ticket
{
    public interface TicketGrantingTicket : Ticket
    {

        /** The prefix to use when generating an id for a TicketGrantingTicket. */
        //string PREFIX = "TGT";
        //string PREFIX { get; }
        /**
     * Method to retrieve the authentication.
     * 
     * @return the authentication
     */
        Authentication getAuthentication();

        /**
     * Grant a ServiceTicket for a specific service.
     * 
     * @param id The unique identifier for this ticket.
     * @param service The service for which we are granting a ticket
     * @return the service ticket granted to a specific service for the
     * principal of the TicketGrantingTicket
     */
        ServiceTicket grantServiceTicket(string id, Service service,
                                         ExpirationPolicy expirationPolicy, bool credentialsProvided);

        /**
     * Explicitly expire a ticket.  This method will log out of any service associated with the
     * Ticket Granting Ticket.
     * 
     */
        void expire();

        /**
     * Convenience method to determine if the TicketGrantingTicket is the root
     * of the hierarchy of tickets.
     * 
     * @return true if it has no parent, false otherwise.
     */
        bool isRoot();

        /**
     * Method to retrieve the chained list of Authentications for this
     * TicketGrantingTicket.
     * 
     * @return the list of principals
     */
        List<Authentication> getChainedAuthentications();
    }
}
