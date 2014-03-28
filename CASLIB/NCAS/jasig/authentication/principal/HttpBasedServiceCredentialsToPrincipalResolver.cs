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
//package org.jasig.cas.authentication.principal;

/**
 * HttpBasedServiceCredentialsToPrincipalResolver extracts the callbackUrl from
 * the HttpBasedServiceCredentials and constructs a SimpleService with the
 * callbackUrl as the unique Id.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.5 $ $Date: 2007/02/27 19:31:58 $
 * @since 3.0
 */

namespace NCAS.jasig.authentication.principal
{
    public class HttpBasedServiceCredentialsToPrincipalResolver :
        CredentialsToPrincipalResolver
    {

        /**
     * Method to return a simple Service Principal with the identifier set to be
     * the callback url.
     */
        public Principal resolvePrincipal(Credentials credentials)
        {
            HttpBasedServiceCredentials serviceCredentials = (HttpBasedServiceCredentials)credentials;
            return new SimpleWebApplicationServiceImpl(serviceCredentials.getCallbackUrl().ToString());
        }

        /**
     * @return true if the credentials provided are not null and are assignable
     * from HttpBasedServiceCredentials, otherwise returns false.
     */
        public bool supports(Credentials credentials)
        {
            return credentials != null
                   && typeof(HttpBasedServiceCredentials).IsInstanceOfType(credentials);
        }
    }
}
