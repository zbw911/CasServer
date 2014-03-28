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

//import javax.servlet.http.HttpRequest;
//import javax.servlet.http.HttpResponse;
//import javax.validation.constraints.NotNull;

//import org.jasig.cas.CentralAuthenticationService;
//import org.jasig.cas.authentication.handler.AuthenticationException;
//import org.jasig.cas.authentication.principal.Credentials;
//import org.jasig.cas.authentication.principal.Service;
//import org.jasig.cas.ticket.TicketException;
//import org.jasig.cas.web.bind.CredentialsBinder;
//import org.jasig.cas.web.support.WebUtils;
//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;
//import org.springframework.binding.message.MessageBuilder;
//import org.springframework.binding.message.MessageContext;
//import org.springframework.util.StringUtils;
//import org.springframework.web.util.CookieGenerator;
//import org.springframework.webflow.execution.HttpContext;

/**
 * Action to authenticate credentials and retrieve a TicketGrantingTicket for
 * those credentials. If there is a request for renew, then it also generates
 * the Service Ticket required.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0.4
 */

using System;
using System.Web;

using NCAS.jasig.authentication.handler;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.ticket;
using NCAS.jasig.web.MOCK2JAVA;
using NCAS.jasig.web.bind;
using NCAS.jasig.web.support;

namespace NCAS.jasig.web.flow
{
    public class AuthenticationViaFormAction
    {

        /**
     * Binder that allows additional binding of form object beyond Spring
     * defaults.
     */
        private CredentialsBinder _credentialsBinder;

        /** Core we delegate to for handling all ticket related tasks. */
        ////@NotNull
        private CentralAuthenticationService _centralAuthenticationService;

        ////@NotNull
        private CookieGenerator _warnCookieGenerator;

        public AuthenticationViaFormAction(CentralAuthenticationService centralAuthenticationService,
                                           CookieGenerator warnCookieGenerator)
            : this(null, centralAuthenticationService, warnCookieGenerator)
        {
        }

        private AuthenticationViaFormAction(CredentialsBinder credentialsBinder,
            CentralAuthenticationService centralAuthenticationService,
            CookieGenerator warnCookieGenerator)
        {
            _credentialsBinder = credentialsBinder;
            _centralAuthenticationService = centralAuthenticationService;
            _warnCookieGenerator = warnCookieGenerator;
        }




        //protected Logger logger = LoggerFactory.getLogger(getClass());

        public void doBind(HttpContext context, Credentials credentials)
        {
            HttpRequest request = WebUtils.getHttpServletRequest(context);

            if (this._credentialsBinder != null && this._credentialsBinder.supports(credentials.GetType()))
            {
                this._credentialsBinder.bind(request, credentials);
            }
        }

        public string submit(HttpContext context, Credentials credentials, MessageContext messageContext)
        {
            // Validate login ticket
            string authoritativeLoginTicket = WebUtils.getLoginTicketFromFlowScope(context);
            string providedLoginTicket = WebUtils.getLoginTicketFromRequest(context);
            if (!authoritativeLoginTicket.Equals(providedLoginTicket))
            {
                //this.logger.warn("Invalid login ticket " + providedLoginTicket);
                string code = "INVALID_TICKET";
                //messageContext.addMessage(
                //    new MessageBuilder().error().code(code).arg(providedLoginTicket).defaultText(code).build());
                return "error";
            }

            string ticketGrantingTicketId = WebUtils.getTicketGrantingTicketId(context);
            Service service = WebUtils.getService(context);
            if (!string.IsNullOrEmpty(context.Request.QueryString["renew"]) && ticketGrantingTicketId != null && service != null)
            {

                try
                {
                    string serviceTicketId = this._centralAuthenticationService.grantServiceTicket(ticketGrantingTicketId, service, credentials);
                    WebUtils.putServiceTicketInRequestScope(context, serviceTicketId);
                    this.putWarnCookieIfRequestParameterPresent(context);
                    return "warn";
                }
                catch (TicketException e)
                {
                    if (this.isCauseAuthenticationException(e))
                    {
                        this.populateErrorsInstance(e, messageContext);
                        return this.getAuthenticationExceptionEventId(e);
                    }

                    this._centralAuthenticationService.destroyTicketGrantingTicket(ticketGrantingTicketId);
                    //if (logger.isDebugEnabled()) {
                    //    logger.debug("Attempted to generate a ServiceTicket using renew=true with different credentials", e);
                    //}
                }
            }

            try
            {
                var ticketValue = this._centralAuthenticationService.createTicketGrantingTicket(credentials);
                WebUtils.putTicketGrantingTicketInRequestScope(context, ticketValue);
                this.putWarnCookieIfRequestParameterPresent(context);
                return "success";
            }
            catch (TicketException e)
            {
                this.populateErrorsInstance(e, messageContext);
                if (this.isCauseAuthenticationException(e))
                    return this.getAuthenticationExceptionEventId(e);
                return "error";
            }
        }


        private void populateErrorsInstance(TicketException e, MessageContext messageContext)
        {

            //try
            //{
            //    messageContext.addMessage(new MessageBuilder().error().code(e.getCode()).defaultText(e.getCode()).build());
            //}
            //catch (Exception fe)
            //{
            //    logger.error(fe.getMessage(), fe);
            //}
        }

        private void putWarnCookieIfRequestParameterPresent(HttpContext context)
        {
            HttpResponse response = WebUtils.getHttpServletResponse(context);

            if (!string.IsNullOrEmpty(context.Request["warn"]))
            {
                this._warnCookieGenerator.addCookie(response, "true");
            }
            else
            {
                this._warnCookieGenerator.removeCookie(response);
            }
        }

        private AuthenticationException getAuthenticationExceptionAsCause(TicketException e)
        {
            throw new NotImplementedException();
            //return (AuthenticationException)e;
        }

        private string getAuthenticationExceptionEventId(TicketException e)
        {
            AuthenticationException authEx = this.getAuthenticationExceptionAsCause(e);

            //if (this.logger.isDebugEnabled())
            //    this.logger.debug("An authentication error has occurred. Returning the event id " + authEx.getType());

            return authEx.getType();
        }

        private bool isCauseAuthenticationException(TicketException e)
        {
            return e.getCode() != null && typeof(AuthenticationException).IsAssignableFrom(e.GetType());
        }

        //public void setCentralAuthenticationService(CentralAuthenticationService centralAuthenticationService)
        //{
        //    this._centralAuthenticationService = centralAuthenticationService;
        //}

        /**
     * Set a CredentialsBinder for additional binding of the HttpRequest
     * to the Credentials instance, beyond our default binding of the
     * Credentials as a Form Object in Spring WebMVC parlance. By the time we
     * invoke this CredentialsBinder, we have already engaged in default binding
     * such that for each HttpRequest parameter, if there was a JavaBean
     * property of the Credentials implementation of the same name, we have set
     * that property to be the value of the corresponding request parameter.
     * This CredentialsBinder plugin point exists to allow consideration of
     * things other than HttpRequest parameters in populating the
     * Credentials (or more sophisticated consideration of the
     * HttpRequest parameters).
     *
     * @param credentialsBinder the credentials binder to set.
     */
        //public void setCredentialsBinder(CredentialsBinder credentialsBinder)
        //{
        //    this._credentialsBinder = credentialsBinder;
        //}

        //public void setWarnCookieGenerator(CookieGenerator warnCookieGenerator)
        //{
        //    this._warnCookieGenerator = warnCookieGenerator;
        //}
    }
}
