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
//package org.jasig.cas.web;

//import javax.servlet.http.HttpRequest;
//import javax.servlet.http.HttpResponse;
//import javax.validation.constraints.NotNull;

//import org.jasig.cas.CentralAuthenticationService;
//import org.jasig.cas.authentication.principal.Service;
//import org.jasig.cas.authentication.principal.SimpleWebApplicationServiceImpl;
//import org.jasig.cas.services.UnauthorizedServiceException;
//import org.jasig.cas.ticket.TicketException;
//import org.springframework.util.StringUtils;
//import org.springframework.web.servlet.ModelAndView;
//import org.springframework.web.servlet.mvc.AbstractController;

/**
 * The ProxyController is involved with returning a Proxy Ticket (in CAS 2
 * terms) to the calling application. In CAS 3, a Proxy Ticket is just a Service
 * Ticket granted to a service.
 * <p>
 * The ProxyController requires the following property to be set:
 * </p>
 * <ul>
 * <li> centralAuthenticationService - the service layer</li>
 * <li> casArgumentExtractor - the assistant for extracting parameters</li>
 * </ul>
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using System;
using System.Web;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.services;
using NCAS.jasig.ticket;
using NCAS.jasig.web.MOCK2JAVA;

namespace NCAS.jasig.web
{
    public  class ProxyController : AbstractController {

        /** View for if the creation of a "Proxy" Ticket Fails. */
        private static  string CONST_PROXY_FAILURE = "casProxyFailureView";

        /** View for if the creation of a "Proxy" Ticket Succeeds. */
        private static  string CONST_PROXY_SUCCESS = "casProxySuccessView";

        /** Key to use in model for service tickets. */
        private static  string MODEL_SERVICE_TICKET = "ticket";

        /** CORE to delegate all non-web tier functionality to. */
        ////@NotNull    ////@NotNull
        private CentralAuthenticationService centralAuthenticationService;

        public ProxyController() {
            this.setCacheSeconds(0);
        }

        /**
     * @return ModelAndView containing a view name of either
     * <code>casProxyFailureView</code> or <code>casProxySuccessView</code>
     */
        protected ModelAndView handleRequestInternal(
            HttpRequest request,  HttpResponse response)
        {
            string ticket = request.getParameter("pgt");
            Service targetService = this.getTargetService(request);

            if (!StringUtils.hasText(ticket) || targetService == null) {
                return this.generateErrorView("INVALID_REQUEST",
                                         "INVALID_REQUEST_PROXY", null);
            }

            try {
                return new ModelAndView(CONST_PROXY_SUCCESS, MODEL_SERVICE_TICKET,
                                        this.centralAuthenticationService.grantServiceTicket(ticket,
                                                                                             targetService));
            } catch (TicketException e) {
                return this.generateErrorView(e.getCode(), e.getCode(),
                                         new Object[] {ticket});
            } catch ( UnauthorizedServiceException e) {
                return this.generateErrorView("UNAUTHORIZED_SERVICE",
                                         "UNAUTHORIZED_SERVICE_PROXY", new Object[] {targetService});
            }
        }

        private Service getTargetService( HttpRequest request) {
            return SimpleWebApplicationServiceImpl.createServiceFrom(request);
        }

        private ModelAndView generateErrorView( string code,
                                                string description,  Object[] args) {
            ModelAndView modelAndView = new ModelAndView(CONST_PROXY_FAILURE);
            modelAndView.addObject("code", code);
            //modelAndView.addObject("description", getMessageSourceAccessor()
            //    .getMessage(description, args, description));

            return modelAndView;
                                                }

        /**
     * @param centralAuthenticationService The centralAuthenticationService to
     * set.
     */
        public void setCentralAuthenticationService(
            CentralAuthenticationService centralAuthenticationService) {
            this.centralAuthenticationService = centralAuthenticationService;
            }
    }
}
