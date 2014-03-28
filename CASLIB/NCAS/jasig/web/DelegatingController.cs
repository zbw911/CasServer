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

//import org.springframework.web.servlet.ModelAndView;
//import org.springframework.web.servlet.mvc.AbstractController;
//import org.springframework.web.servlet.mvc.Controller;

//import javax.servlet.http.HttpRequest;
//import javax.servlet.http.HttpResponse;
//import javax.validation.constraints.NotNull;
//import java.util.List;

/**
 * Delegating controller.
 * Tries to find a controller among its delegates, that can handle the current request.
 * If none is found, an error is generated.
 * @author Frederic Esnault
 * @version $Id$
 * @since 3.5
 */

using System;
using System.Web;
using System.Collections.Generic;

namespace NCAS.jasig.web
{
    public class DelegatingController : AbstractController
    {
        List<DelegateController> delegates;
        /** View if Service Ticket Validation Fails. */
        private static string DEFAULT_ERROR_VIEW_NAME = "casServiceFailureView";

        /** The view to redirect if no delegate can handle the request. */
        ////@NotNull
        private string failureView = DEFAULT_ERROR_VIEW_NAME;

        /**
     * Handles the request.
     * Ask all delegates if they can handle the current request.
     * The first to answer true is elected as the delegate that will process the request.
     * If no controller answers true, we redirect to the error page.
     * @param request the request to handle
     * @param response the response to write to
     * @return the model and view object
     * @ if an error occurs during request handling
     */
        protected ModelAndView handleRequestInternal(HttpRequest request, HttpResponse response)
        {
            foreach (DelegateController d in this.delegates)
            {
                if (d.canHandle(request, response))
                {
                    return d.handleRequest(request, response);
                }
            }
            return this.generateErrorView("INVALID_REQUEST", "INVALID_REQUEST", null);
        }

        private ModelAndView generateErrorView(string code, string description, Object[] args)
        {
            ModelAndView modelAndView = new ModelAndView(this.failureView);
            //string convertedDescription = getMessageSourceAccessor().getMessage(description, args, description);
            modelAndView.addObject("code", code);
            //modelAndView.addObject("description", convertedDescription);

            return modelAndView;
        }

     

        /**
     * @param delegates the delegate controllers to set
     */
        ////@NotNull
        public void setDelegates(List<DelegateController> delegates)
        {
            this.delegates = delegates;
        }

        /**
     * @param failureView The failureView to set.
     */
        public void setFailureView(string failureView)
        {
            this.failureView = failureView;
        }
    }
}