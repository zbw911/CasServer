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

////import javax.persistence.Column;
////import javax.persistence.Entity;
////import javax.persistence.Lob;
////import javax.persistence.Table;

////import org.jasig.cas.authentication.Authentication;
////import org.jasig.cas.authentication.principal.Service;
////import org.springframework.util.Assert;

/**
 * Domain object representing a Service Ticket. A service ticket grants specific
 * access to a particular service. It will only work for a particular service.
 * Generally, it is a one time use Ticket, but the specific expiration policy
 * can be anything.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */
//@Entity
//@Table(name="SERVICETICKET")

using System;
using NCAS.jasig.authentication;
using NCAS.jasig.authentication.principal;

namespace NCAS.jasig.ticket
{
    public class ServiceTicketImpl : AbstractTicket,
                                                         ServiceTicket
    {

        /** Unique Id for serialization. */
        private static long serialVersionUID = -4223319704861765405L;

        /** The service this ticket is valid for. */
        //@Lob
        //    @Column(name="SERVICE",nullable=false)
        private Service service;

        /** Is this service ticket the result of a new login. */
        //@Column(name="FROM_NEW_LOGIN",nullable=false)
        private bool fromNewLogin;

        //@Column(name="TICKET_ALREADY_GRANTED",nullable=false)
        private bool grantedTicketAlready = false;

        public ServiceTicketImpl()
        {
            // exists for JPA purposes
        }

        /**
     * Constructs a new ServiceTicket with a Unique Id, a TicketGrantingTicket,
     * a Service, Expiration Policy and a flag to determine if the ticket
     * creation was from a new Login or not.
     * 
     * @param id the unique identifier for the ticket.
     * @param ticket the TicketGrantingTicket parent.
     * @param service the service this ticket is for.
     * @param fromNewLogin is it from a new login.
     * @param policy the expiration policy for the Ticket.
     * @throws IllegalArgumentException if the TicketGrantingTicket or the
     * Service are null.
     */
        public ServiceTicketImpl(string id,
                                     TicketGrantingTicketImpl ticket, Service service,
                                     bool fromNewLogin, ExpirationPolicy policy)
            : base(id, ticket, policy)
        {


            //Assert.notNull(ticket, "ticket cannot be null");
            //Assert.notNull(service, "service cannot be null");

            this.service = service;
            this.fromNewLogin = fromNewLogin;
        }

        public bool isFromNewLogin()
        {
            return this.fromNewLogin;
        }

        public string PREFIX
        {
            get
            {
                return "ST";
            }
        }

        public Service getService()
        {
            return this.service;
        }

        public bool isValidFor(Service serviceToValidate)
        {
            this.updateState();
            return serviceToValidate.matches(this.service);
        }

        public TicketGrantingTicket grantTicketGrantingTicket(
            string id, Authentication authentication,
            ExpirationPolicy expirationPolicy)
        {
            {
                if (this.grantedTicketAlready)
                {
                    throw new Exception(
                        "TicketGrantingTicket already generated for this ServiceTicket.  Cannot grant more than one TGT for ServiceTicket");
                }
                this.grantedTicketAlready = true;
            }

            return new TicketGrantingTicketImpl(id, (TicketGrantingTicketImpl)this.getGrantingTicket(),
                                                authentication, expirationPolicy);
        }

        public override Authentication getAuthentication()
        {
            return null;
        }

        public bool equals(Object obj)
        {
            if (obj == null
                          || !(obj is ServiceTicket))
            {
                return false;
            }

            Ticket serviceTicket = (Ticket)obj;

            return serviceTicket.getId().Equals(this.getId());
        }
    }
}
