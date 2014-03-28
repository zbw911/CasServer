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
//import org.jasig.cas.authentication.handler.AuthenticationException;
//import org.jasig.cas.authentication.principal.Credentials;
//import org.jasig.cas.authentication.principal.Service;
//import org.jasig.cas.ticket.TicketException;
//import org.jasig.cas.web.support.WebUtils;
//import org.springframework.util.StringUtils;
//import org.springframework.webflow.action.AbstractAction;
//import org.springframework.webflow.execution.Event;
//import org.springframework.webflow.execution.HttpContext;

//import javax.validation.constraints.NotNull;

using System;
using System.Web;
using NCAS.jasig.authentication.handler;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.ticket;
using NCAS.jasig.web.MOCK2JAVA;
using NCAS.jasig.web.support;

/**
 * Abstract class to handle the retrieval and authentication of non-interactive
 * credentials such as client certificates, NTLM, etc.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0.4
 */

namespace NCAS.jasig.web.flow
{
    public abstract class AbstractNonInteractiveCredentialsAction :
        AbstractAction
    {

        /** Instance of CentralAuthenticationService. */
        //@NotNull
        private CentralAuthenticationService centralAuthenticationService;

        protected bool isRenewPresent(HttpContext context)
        {
            return StringUtils.hasText(context.getRequestParameters().get("renew"));
        }

        protected Event doExecute(HttpContext context)
        {
            Credentials credentials = this.constructCredentialsFromRequest(context);

            if (credentials == null)
            {
                return this.error();
            }

            string ticketGrantingTicketId = WebUtils.getTicketGrantingTicketId(context);
            Service service = WebUtils.getService(context);

            if (this.isRenewPresent(context)
                && ticketGrantingTicketId != null
                && service != null)
            {

                try
                {
                    string serviceTicketId = this.centralAuthenticationService
                        .grantServiceTicket(ticketGrantingTicketId,
                                            service,
                                            credentials);
                    WebUtils.putServiceTicketInRequestScope(context,
                                                            serviceTicketId);
                    return this.result("warn");
                }
                catch (TicketException e)
                {
                    if (e is AuthenticationException)
                    {
                        this.onError(context, credentials);
                        return this.error();
                    }
                    this.centralAuthenticationService
                        .destroyTicketGrantingTicket(ticketGrantingTicketId);
                    //if (logger.isDebugEnabled())
                    //{
                    //    logger
                    //        .debug(
                    //            "Attempted to generate a ServiceTicket using renew=true with different credentials",
                    //            e);
                    //}
                }
            }

            try
            {
                WebUtils.putTicketGrantingTicketInRequestScope(
                    context,
                    this.centralAuthenticationService
                        .createTicketGrantingTicket(credentials));
                this.onSuccess(context, credentials);
                return this.success();
            }
            catch (TicketException e)
            {
                this.onError(context, credentials);
                return this.error();
            }
        }

        public void setCentralAuthenticationService(
            CentralAuthenticationService centralAuthenticationService)
        {
            this.centralAuthenticationService = centralAuthenticationService;
        }

        /**
     * Hook method to allow for additional processing of the response before
     * returning an error event.
     * 
     * @param context the context for this specific request.
     * @param credentials the credentials for this request.
     */
        protected void onError(HttpContext context,
                                Credentials credentials)
        {
            // default implementation does nothing
        }

        /**
     * Hook method to allow for additional processing of the response before
     * returning a success event.
     * 
     * @param context the context for this specific request.
     * @param credentials the credentials for this request.
     */
        protected void onSuccess(HttpContext context,
                                  Credentials credentials)
        {
            // default implementation does nothing
        }

        /**
     * Abstract method to implement to construct the credentials from the
     * request object.
     * 
     * @param context the context for this request.
     * @return the constructed credentials or null if none could be constructed
     * from the request.
     */
        protected abstract Credentials constructCredentialsFromRequest(
            HttpContext context);
    }
}
