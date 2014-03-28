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
//package org.jasig.cas.web.support;

//import java.util.List;

//import javax.servlet.http.HttpRequest;
//import javax.servlet.http.HttpResponse;

//import org.jasig.cas.authentication.principal.WebApplicationService;
//import org.springframework.util.Assert;
//import org.springframework.webflow.context.servlet.ServletExternalContext;
//import org.springframework.webflow.execution.HttpContext;

/**
 * Common utilities for the web tier.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.1
 */

using System.Collections.Generic;
using System.Web;
using NCAS.jasig.authentication.principal;

namespace NCAS.jasig.web.support
{
    public class WebUtils
    {

        /** Request attribute that contains message key describing details of authorization failure.*/
        public static string CAS_ACCESS_DENIED_REASON = "CAS_ACCESS_DENIED_REASON";

        public static HttpRequest getHttpServletRequest(
            HttpContext context)
        {
            //Assert.isInstanceOf(ServletExternalContext.class, context
            //    .getExternalContext(),
            //    "Cannot obtain HttpRequest from event of type: "
            //        + context.getExternalContext().getClass().getName());

            return context.Request;
            //return (HttpRequest) context.getExternalContext().getNativeRequest();
        }

        public static HttpResponse getHttpServletResponse(
            HttpContext context)
        {
            //Assert.isInstanceOf(ServletExternalContext.class, context
            //    .getExternalContext(),
            //    "Cannot obtain HttpResponse from event of type: "
            //        + context.getExternalContext().getClass().getName());
            //return (HttpResponse) context.getExternalContext()
            //    .getNativeResponse();

            return context.Response;
        }

        public static WebApplicationService getService(
            List<ArgumentExtractor> argumentExtractors,
            HttpRequest request)
        {
            foreach (ArgumentExtractor argumentExtractor in argumentExtractors)
            {
                WebApplicationService service = argumentExtractor
                    .extractService(request);

                if (service != null)
                {
                    return service;
                }
            }

            return null;
        }

        public static WebApplicationService getService(
            List<ArgumentExtractor> argumentExtractors,
            HttpContext context)
        {
            HttpRequest request = WebUtils.getHttpServletRequest(context);
            return getService(argumentExtractors, request);
        }

        public static WebApplicationService getService(
            HttpContext context)
        {
            //context.Items[]
            return (WebApplicationService)context.Session["service"];
        }

        public static void putTicketGrantingTicketInRequestScope(
            HttpContext context, string ticketValue)
        {
            context.Items["ticketGrantingTicketId"] = ticketValue;


        }

        public static string getTicketGrantingTicketId(
            HttpContext context)
        {
            string tgtFromRequest = (string)context.Items["ticketGrantingTicketId"];
            string tgtFromFlow = (string)context.Items["ticketGrantingTicketId"];

            return tgtFromRequest != null ? tgtFromRequest : tgtFromFlow;

        }

        public static void putServiceTicketInRequestScope(
            HttpContext context, string ticketValue)
        {
            context.Items["serviceTicketId"] = ticketValue;
        }

        public static string getServiceTicketFromRequestScope(
            HttpContext context)
        {
            return context.Items["serviceTicketId"].ToString();
        }

        public static void putLoginTicket(HttpContext context, string ticket)
        {
            context.Session["loginTicket"] = ticket;
        }

        public static string getLoginTicketFromFlowScope(HttpContext context)
        {
            // Getting the saved LT destroys it in support of one-time-use
            // See section 3.5.1 of http://www.jasig.org/cas/protocol
            string lt = (string)context.Session["loginTicket"];//.remove("loginTicket");
            context.Session.Remove("loginTicket");
            return lt != null ? lt : "";
        }

        public static string getLoginTicketFromRequest(HttpContext context)
        {
            return context.Request.Params["lt"] ?? "";
            //return context.getRequestParameters().get("lt");
        }
    }
}
