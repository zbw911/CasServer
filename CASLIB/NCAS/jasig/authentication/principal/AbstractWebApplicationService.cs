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
////package org.jasig.cas.authentication.principal;

////import java.util.Collections;
////import java.util.HashMap;
////import java.util.Map;

////import org.jasig.cas.util.DefaultUniqueTicketIdGenerator;
////import org.jasig.cas.util.HttpClient;
////import org.jasig.cas.util.SamlUtils;
////import org.jasig.cas.util.UniqueTicketIdGenerator;
////import org.slf4j.Logger;
////import org.slf4j.LoggerFactory;

/**
 * Abstract implementation of a WebApplicationService.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.3 $ $Date: 2007/04/19 20:13:01 $
 * @since 3.1
 *
 */

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NCAS.jasig.util;

namespace NCAS.jasig.authentication.principal
{
    public abstract class AbstractWebApplicationService : WebApplicationService
    {

        //protected static  Logger LOG = LoggerFactory.getLogger(SamlService.class);

        private static Dictionary<string, Object> EMPTY_MAP = new Dictionary<string, object>();

        private static UniqueTicketIdGenerator GENERATOR = new DefaultUniqueTicketIdGenerator();

        /** The id of the service. */
        private string id;

        /** The original url provided, used to reconstruct the redirect url. */
        private string originalUrl;

        private string artifactId;

        private Principal principal;

        private bool loggedOutAlready = false;

        private HttpClient httpClient;

        protected AbstractWebApplicationService(string id, string originalUrl, string artifactId, HttpClient httpClient)
        {
            this.id = id;
            this.originalUrl = originalUrl;
            this.artifactId = artifactId;
            this.httpClient = httpClient;
        }

        public string toString()
        {
            return this.id;
        }

        public string getId()
        {
            return this.id;
        }

        public abstract Response getResponse(string ticketId);

        public string getArtifactId()
        {
            return this.artifactId;
        }

        public Dictionary<string, Object> getAttributes()
        {
            return EMPTY_MAP;
        }

        protected static string cleanupUrl(string url)
        {
            if (url == null)
            {
                return null;
            }

            int jsessionPosition = url.IndexOf(";jsession");

            if (jsessionPosition == -1)
            {
                return url;
            }

            int questionMarkPosition = url.IndexOf("?");

            if (questionMarkPosition < jsessionPosition)
            {
                return url.Substring(0, url.IndexOf(";jsession"));
            }

            return url.Substring(0, jsessionPosition)
                   + url.Substring(questionMarkPosition);
        }

        protected string getOriginalUrl()
        {
            return this.originalUrl;
        }

        protected HttpClient getHttpClient()
        {
            return this.httpClient;
        }

        public bool equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Service)
            {
                Service service = (Service)obj;

                return this.getId().Equals(service.getId());
            }

            return false;
        }

        public int hashCode()
        {
            int prime = 41;
            int result = 1;
            result = prime * result
                     + ((this.id == null) ? 0 : this.id.GetHashCode());
            return result;
        }

        protected Principal getPrincipal()
        {
            return this.principal;
        }

        public void setPrincipal(Principal principal)
        {
            this.principal = principal;
        }

        public bool matches(Service service)
        {
            return this.id.Equals(service.getId());
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool logOutOfService(string sessionIdentifier)
        {
            if (this.loggedOutAlready)
            {
                return true;
            }

            Dev.Log.Loger.Debug("Sending logout request for: " + this.getId());

            string logoutRequest = "<samlp:LogoutRequest xmlns:samlp=\"urn:oasis:names:tc:SAML:2.0:protocol\" ID=\""
                                   + GENERATOR.getNewTicketId("LR")
                                   + "\" Version=\"2.0\" IssueInstant=\"" + SamlUtils.getCurrentDateAndTime()
                                   + "\"><saml:NameID xmlns:saml=\"urn:oasis:names:tc:SAML:2.0:assertion\">@NOT_USED@</saml:NameID><samlp:SessionIndex>"
                                   + sessionIdentifier + "</samlp:SessionIndex></samlp:LogoutRequest>";

            this.loggedOutAlready = true;

            if (this.httpClient != null)
            {
                return this.httpClient.sendMessageToEndPoint(this.getOriginalUrl(), logoutRequest, true);
            }

            return false;
        }
    }
}
