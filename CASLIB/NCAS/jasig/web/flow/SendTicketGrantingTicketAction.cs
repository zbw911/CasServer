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
//package org.jasig.cas.web.flow;

//import org.jasig.cas.CentralAuthenticationService;
//import org.jasig.cas.web.support.CookieRetrievingCookieGenerator;
//import org.jasig.cas.web.support.WebUtils;
//import org.springframework.webflow.action.AbstractAction;
//import org.springframework.webflow.execution.Event;
//import org.springframework.webflow.execution.HttpContext;

//import javax.validation.constraints.NotNull;

using System;
using System.Web;
 
using NCAS.jasig.web.MOCK2JAVA;
using NCAS.jasig.web.support;

/**
 * Action that handles the TicketGrantingTicket creation and destruction. If the
 * action is given a TicketGrantingTicket and one also already exists, the old
 * one is destroyed and replaced with the new one. This action always returns
 * "success".
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0.4
 */

namespace NCAS.jasig.web.flow
{
    public class SendTicketGrantingTicketAction : AbstractAction
    {

        ////@NotNull
        private CookieRetrievingCookieGenerator ticketGrantingTicketCookieGenerator;

        /** Instance of CentralAuthenticationService. */
        ////@NotNull
        private CentralAuthenticationService centralAuthenticationService;

        protected Event doExecute(HttpContext context)
        {
            string ticketGrantingTicketId = WebUtils.getTicketGrantingTicketId(context);
            string ticketGrantingTicketValueFromCookie = (string)context.Session[("ticketGrantingTicketId")];

            if (ticketGrantingTicketId == null)
            {
                return this.success();
            }

            this.ticketGrantingTicketCookieGenerator.addCookie(WebUtils.getHttpServletRequest(context), WebUtils
                                                                                                            .getHttpServletResponse(context), ticketGrantingTicketId);

            if (ticketGrantingTicketValueFromCookie != null && !ticketGrantingTicketId.Equals(ticketGrantingTicketValueFromCookie))
            {
                this.centralAuthenticationService
                    .destroyTicketGrantingTicket(ticketGrantingTicketValueFromCookie);
            }

            return this.success();
        }

        public void setTicketGrantingTicketCookieGenerator(CookieRetrievingCookieGenerator ticketGrantingTicketCookieGenerator)
        {
            this.ticketGrantingTicketCookieGenerator = ticketGrantingTicketCookieGenerator;
        }

        public void setCentralAuthenticationService(
            CentralAuthenticationService centralAuthenticationService)
        {
            this.centralAuthenticationService = centralAuthenticationService;
        }
    }
}
