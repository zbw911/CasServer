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
//package org.jasig.cas.ticket.registry;

//import org.jasig.cas.monitor.TicketRegistryState;
//import org.jasig.cas.ticket.Ticket;
//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;
//import org.springframework.util.Assert;

/**
 * @author Scott Battaglia
 * @since 3.0.4
 * <p>
 * This is a published and supported CAS Server 3 API.
 * </p>
 */

using System;
using System.Collections.Generic;
using NCAS.jasig.monitor;

namespace NCAS.jasig.ticket.registry
{
    public abstract class AbstractTicketRegistry : TicketRegistry, TicketRegistryState
    {

        /** The Commons Logging log instance. */
        //protected  Logger log = LoggerFactory.getLogger(getClass());

        /**
     * @throws IllegalArgumentException if class is null.
     * @throws ClassCastException if class does not match requested ticket
     * class.
     */
        public abstract void addTicket(Ticket ticket);

        public Ticket getTicket(string ticketId,
                                Type clazz)
        {
            //Assert.notNull(clazz, "clazz cannot be null");

            Ticket ticket = this.getTicket(ticketId);

            if (ticket == null)
            {
                return null;
            }

            if (!clazz.IsInstanceOfType(ticket))
            {
                //throw new ClassCastException("Ticket [" + ticket.getId()
                //    + " is of type " + ticket.getClass()
                //    + " when we were expecting " + clazz);

                throw new NotSupportedException();
            }

            return ticket;
        }

        public abstract Ticket getTicket(string ticketId);
        public abstract bool deleteTicket(string ticketId);
        public abstract List<Ticket> getTickets();

        public int sessionCount()
        {
            //log.debug("sessionCount() operation is not implemented by the ticket registry instance {}. Returning unknown as {}",
            //          this.getClass().getName(), Integer.MIN_VALUE);
            return int.MinValue;
        }

        public int serviceTicketCount()
        {
            //log.debug("serviceTicketCount() operation is not implemented by the ticket registry instance {}. Returning unknown as {}",
            //          this.getClass().getName(), Integer.MIN_VALUE);
            return int.MinValue;
        }
    }
}
