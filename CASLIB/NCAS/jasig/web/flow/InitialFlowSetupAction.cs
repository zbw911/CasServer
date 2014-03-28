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

//import java.util.List;

//import javax.servlet.http.HttpRequest;
//import javax.validation.constraints.NotNull;
//import javax.validation.constraints.Size;

//import org.jasig.cas.authentication.principal.Service;
//import org.jasig.cas.web.support.ArgumentExtractor;
//import org.jasig.cas.web.support.CookieRetrievingCookieGenerator;
//import org.jasig.cas.web.support.WebUtils;
//import org.springframework.util.StringUtils;
//import org.springframework.webflow.action.AbstractAction;
//import org.springframework.webflow.execution.Event;
//import org.springframework.webflow.execution.HttpContext;

using System;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.web.MOCK2JAVA;
using NCAS.jasig.web.support;
using System.Collections.Generic;
using System.Web;
/**
 * Class to automatically set the paths for the CookieGenerators.
 * <p>
 * Note: This is technically not threadsafe, but because its overriding with a
 * constant value it doesn't matter.
 * <p>
 * Note: As of CAS 3.1, this is a required class that retrieves and exposes the
 * values in the two cookies for subclasses to use.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.1
 */

namespace NCAS.jasig.web.flow
{
    public class InitialFlowSetupAction : AbstractAction
    {

        /** CookieGenerator for the Warnings. */
        ////@NotNull
        private CookieRetrievingCookieGenerator warnCookieGenerator;

        /** CookieGenerator for the TicketGrantingTickets. */
        ////@NotNull
        private CookieRetrievingCookieGenerator ticketGrantingTicketCookieGenerator;

        /** Extractors for finding the service. */
        ////@NotNull
        //@Size(min=1)
        private List<ArgumentExtractor> argumentExtractors;

        /** bool to note whether we've set the values on the generators or not. */
        private bool pathPopulated = false;

        protected Event doExecute(HttpContext context)
        {
            HttpRequest request = WebUtils.getHttpServletRequest(context);
            if (!this.pathPopulated)
            {
                string contextPath = context.Request.Path;//().getContextPath();
                string cookiePath = StringUtils.hasText(contextPath) ? contextPath + "/" : "/";
                //logger.info("Setting path for cookies to: "
                //    + cookiePath);
                this.warnCookieGenerator.setCookiePath(cookiePath);
                this.ticketGrantingTicketCookieGenerator.setCookiePath(cookiePath);
                this.pathPopulated = true;
            }



            context.Session.Add(
                "ticketGrantingTicketId", this.ticketGrantingTicketCookieGenerator.retrieveCookieValue(request));
            context.Session.Add(
                "warnCookieValue",
                bool.Parse(this.warnCookieGenerator.retrieveCookieValue(request)));

            Service service = WebUtils.getService(this.argumentExtractors,
                                                  context);

            //if (service != null && logger.isDebugEnabled())
            //{
            //    logger.debug("Placing service in FlowScope: " + service.getId());
            //}

            context.Session.Add("service", service);

            return this.result("success");
        }

        public void setTicketGrantingTicketCookieGenerator(
            CookieRetrievingCookieGenerator ticketGrantingTicketCookieGenerator)
        {
            this.ticketGrantingTicketCookieGenerator = ticketGrantingTicketCookieGenerator;
        }

        public void setWarnCookieGenerator(CookieRetrievingCookieGenerator warnCookieGenerator)
        {
            this.warnCookieGenerator = warnCookieGenerator;
        }

        public void setArgumentExtractors(
            List<ArgumentExtractor> argumentExtractors)
        {
            this.argumentExtractors = argumentExtractors;
        }
    }
}