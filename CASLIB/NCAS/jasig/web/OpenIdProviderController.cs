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

//import org.springframework.web.servlet.ModelAndView;
//import org.springframework.web.servlet.mvc.AbstractController;

/**
 * Maps requests for usernames to a page that displays the Login URL for an
 * OpenId Identity Provider.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.1 $ $Date: 2005/08/19 18:27:17 $
 * @since 3.1
 */

using System;
using System.Web;

namespace NCAS.jasig.web
{
    public class OpenIdProviderController : AbstractController
    {

        ////@NotNull
        private string loginUrl;

        protected ModelAndView handleRequestInternal(HttpRequest request,
                                                      HttpResponse response)
        {
            return new ModelAndView("openIdProviderView", "openid_server", this.loginUrl);
        }

        public void setLoginUrl(string loginUrl)
        {
            this.loginUrl = loginUrl;
        }
    }
}
