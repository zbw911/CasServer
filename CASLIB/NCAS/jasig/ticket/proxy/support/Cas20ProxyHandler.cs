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
//package org.jasig.cas.ticket.proxy.support;

//import org.jasig.cas.authentication.principal.Credentials;
//import org.jasig.cas.authentication.principal.HttpBasedServiceCredentials;
//import org.jasig.cas.ticket.proxy.ProxyHandler;
//import org.jasig.cas.util.DefaultUniqueTicketIdGenerator;
//import org.jasig.cas.util.HttpClient;
//import org.jasig.cas.util.UniqueTicketIdGenerator;
//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;

//import javax.validation.constraints.NotNull;

/**
 * Proxy Handler to handle the default callback functionality of CAS 2.0.
 * <p>
 * The default behavior as defined in the CAS 2 Specification is to callback the
 * URL provided and give it a pgtIou and a pgtId.
 * </p>
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using System.Text;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.util;

namespace NCAS.jasig.ticket.proxy.support
{
    public class Cas20ProxyHandler : ProxyHandler
    {

        /** The Commons Logging instance. */
        //private  Logger log = LoggerFactory.getLogger(getClass());

        /** The PGTIOU ticket prefix. */
        private static string PGTIOU_PREFIX = "PGTIOU";

        /** Generate unique ids. */
        //@NotNull
        private UniqueTicketIdGenerator uniqueTicketIdGenerator = new DefaultUniqueTicketIdGenerator();

        /** Instance of Apache Commons HttpClient */
        //@NotNull
        private HttpClient httpClient;

        public Cas20ProxyHandler(HttpClient httpClient, UniqueTicketIdGenerator uniqueTicketIdGenerator)
        {
            this.httpClient = httpClient;
            this.uniqueTicketIdGenerator = uniqueTicketIdGenerator;
        }

        public Cas20ProxyHandler(HttpClient httpClient)
            : this(httpClient, new DefaultUniqueTicketIdGenerator())
        {
            this.httpClient = httpClient;

        }

        public string handle(Credentials credentials,
                             string proxyGrantingTicketId)
        {
            HttpBasedServiceCredentials serviceCredentials = (HttpBasedServiceCredentials)credentials;
            string proxyIou = this.uniqueTicketIdGenerator
                .getNewTicketId(PGTIOU_PREFIX);
            string serviceCredentialsAsString = serviceCredentials.getCallbackUrl().ToString();
            StringBuilder stringBuffer = new StringBuilder(
                serviceCredentialsAsString.Length + proxyIou.Length
                + proxyGrantingTicketId.Length + 15);

            stringBuffer.Append(serviceCredentialsAsString);

            if (serviceCredentials.getCallbackUrl().Query != null)
            {
                stringBuffer.Append("&");
            }
            else
            {
                stringBuffer.Append("?");
            }

            stringBuffer.Append("pgtIou=");
            stringBuffer.Append(proxyIou);
            stringBuffer.Append("&pgtId=");
            stringBuffer.Append(proxyGrantingTicketId);

            if (this.httpClient.isValidEndPoint(stringBuffer.ToString()))
            {
                //if (log.isDebugEnabled())
                //{
                //    log.debug("Sent ProxyIou of " + proxyIou + " for service: "
                //        + serviceCredentials.toString());
                //}
                return proxyIou;
            }

            //if (log.isDebugEnabled())
            //{
            //    log.debug("Failed to send ProxyIou of " + proxyIou
            //        + " for service: " + serviceCredentials.toString());
            //}
            return null;
        }

        /**
     * @param uniqueTicketIdGenerator The uniqueTicketIdGenerator to set.
     */
        //public void setUniqueTicketIdGenerator(
        //    UniqueTicketIdGenerator uniqueTicketIdGenerator)
        //{
        //    this.uniqueTicketIdGenerator = uniqueTicketIdGenerator;
        //}

        //public void setHttpClient(HttpClient httpClient)
        //{
        //    this.httpClient = httpClient;
        //}
    }
}
