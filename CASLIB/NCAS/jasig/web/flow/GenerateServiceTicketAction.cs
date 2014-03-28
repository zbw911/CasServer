using System.Web;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.ticket;
using NCAS.jasig.web.MOCK2JAVA;
using NCAS.jasig.web.support;

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
//import org.jasig.cas.authentication.principal.Service;
//import org.jasig.cas.ticket.TicketException;
//import org.jasig.cas.web.support.WebUtils;
//import org.springframework.util.StringUtils;
//import org.springframework.webflow.action.AbstractAction;
//import org.springframework.webflow.execution.Event;
//import org.springframework.webflow.execution.HttpContext;

//import javax.validation.constraints.NotNull;

/**
 * Action to generate a service ticket for a given Ticket Granting Ticket and
 * Service.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0.4
 */
namespace NCAS.jasig.web.flow
{
    public class GenerateServiceTicketAction : AbstractAction
    {

        /** Instance of CentralAuthenticationService. */
        ////@NotNull
        private CentralAuthenticationService centralAuthenticationService;

        protected Event doExecute(HttpContext context)
        {
            Service service = WebUtils.getService(context);
            string ticketGrantingTicket = WebUtils.getTicketGrantingTicketId(context);

            try
            {
                string serviceTicketId = this.centralAuthenticationService
                    .grantServiceTicket(ticketGrantingTicket,
                                        service);
                WebUtils.putServiceTicketInRequestScope(context,
                                                        serviceTicketId);
                return this.success();
            }
            catch (TicketException e)
            {
                if (this.isGatewayPresent(context))
                {
                    return this.result("gateway");
                }
            }

            return this.error();
        }



        public void setCentralAuthenticationService(
            CentralAuthenticationService centralAuthenticationService)
        {
            this.centralAuthenticationService = centralAuthenticationService;
        }

        protected bool isGatewayPresent(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Request.QueryString[("gateway")]);
        }
    }
}
