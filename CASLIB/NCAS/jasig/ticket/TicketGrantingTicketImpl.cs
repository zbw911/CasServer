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

////import java.util.ArrayList;
////import java.util.Collections;
////import java.util.HashMap;
////import java.util.List;
////import java.util.Map.Entry;

////import javax.persistence.Column;
////import javax.persistence.Entity;
////import javax.persistence.Lob;
////import javax.persistence.Table;

////import org.jasig.cas.authentication.Authentication;
////import org.jasig.cas.authentication.principal.Service;
////import org.slf4j.Logger;
////import org.slf4j.LoggerFactory;
////import org.springframework.util.Assert;

/**
 * Concrete implementation of a TicketGrantingTicket. A TicketGrantingTicket is
 * the global identifier of a principal into the system. It grants the Principal
 * single-sign on access to any service that opts into single-sign on.
 * Expiration of a TicketGrantingTicket is controlled by the ExpirationPolicy
 * specified as object creation.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.3 $ $Date: 2007/02/20 14:41:04 $
 * @since 3.0
 */
////@Entity
//@Table(name="TICKETGRANTINGTICKET")

using System;
using System.Collections.Generic;
using System.Linq;
using NCAS.jasig.authentication;
using NCAS.jasig.authentication.principal;

namespace NCAS.jasig.ticket
{
    public class TicketGrantingTicketImpl : AbstractTicket,
                                            TicketGrantingTicket
    {

        /** Unique Id for serialization. */
        private static long serialVersionUID = -5197946718924166491L;

        //private static  Logger LOG = LoggerFactory.getLogger(TicketGrantingTicketImpl.class);

        /** The authenticated object for which this ticket was generated for. */
        //@Lob
        //@Column(name="AUTHENTICATION", nullable=false)
        private Authentication authentication;

        /** Flag to enforce manual expiration. */
        //@Column(name="EXPIRED", nullable=false)
        private bool expired = false;

        //@Lob
        //@Column(name="SERVICES_GRANTED_ACCESS_TO", nullable=false)
        private Dictionary<string, Service> services = new Dictionary<string, Service>();

        public TicketGrantingTicketImpl()
        {
            // nothing to do
        }

        /**
     * Constructs a new TicketGrantingTicket.
     * 
     * @param id the id of the Ticket
     * @param ticketGrantingTicket the parent ticket
     * @param authentication the Authentication request for this ticket
     * @param policy the expiration policy for this ticket.
     * @throws IllegalArgumentException if the Authentication object is null
     */
        public TicketGrantingTicketImpl(string id,
                                        TicketGrantingTicketImpl ticketGrantingTicket,
                                        Authentication authentication, ExpirationPolicy policy)
            : base(id, ticketGrantingTicket, policy)
        {
            //base(id, ticketGrantingTicket, policy);

            //Assert.notNull(authentication, "authentication cannot be null");

            this.authentication = authentication;
        }

        /**
     * Constructs a new TicketGrantingTicket without a parent
     * TicketGrantingTicket.
     * 
     * @param id the id of the Ticket
     * @param authentication the Authentication request for this ticket
     * @param policy the expiration policy for this ticket.
     */
        public TicketGrantingTicketImpl(string id,
                                        Authentication authentication, ExpirationPolicy policy)
            : this(id, null, authentication, policy)
        {
            ;
        }

        public string PREFIX
        {
            get { return "TGT"; }

        }

        public override Authentication getAuthentication()
        {
            return this.authentication;
        }

        public ServiceTicket grantServiceTicket(string id,
                                                Service service, ExpirationPolicy expirationPolicy,
                                                bool credentialsProvided)
        {
            ServiceTicket serviceTicket = new ServiceTicketImpl(id, this,
                                                                service, this.getCountOfUses() == 0 || credentialsProvided,
                                                                expirationPolicy);

            this.updateState();

            List<Authentication> authentications = this.getChainedAuthentications();
            service.setPrincipal(principal: authentications.Last().getPrincipal());

            this.services.Add(id, service);

            return serviceTicket;
        }

        private void logOutOfServices()
        {
            foreach (var entry in this.services)
            {

                if (!entry.Value.logOutOfService(entry.Key))
                {
                    //LOG.warn("Logout message not sent to [" + entry.Value.getId() + "]; Continuing processing...");   
                }
            }
        }

        public bool isRoot()
        {
            return this.getGrantingTicket() == null;
        }

        public void expire()
        {
            this.expired = true;
            this.logOutOfServices();
        }

        public bool isExpiredInternal()
        {
            return this.expired;
        }

        public List<Authentication> getChainedAuthentications()
        {
            List<Authentication> list = new List<Authentication>();

            if (this.getGrantingTicket() == null)
            {
                list.Add(this.getAuthentication());
                return list;
            }

            list.Add(this.getAuthentication());
            list.AddRange(this.getGrantingTicket().getChainedAuthentications());

            return list;// Collections.unmodifiableList(list);
        }

        public bool equals(Object obj)
        {
            if (obj == null
                || !(obj is TicketGrantingTicket))
            {
                return false;
            }

            Ticket ticket = (Ticket)obj;

            return ticket.getId().Equals(this.getId());
        }
    }
}
