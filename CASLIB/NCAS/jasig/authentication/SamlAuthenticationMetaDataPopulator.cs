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
//package org.jasig.cas.authentication;

//import java.util.HashMap;
//import java.util.Map;

//import org.jasig.cas.authentication.principal.Credentials;
//import org.jasig.cas.authentication.principal.HttpBasedServiceCredentials;
//import org.jasig.cas.authentication.principal.UsernamePasswordCredentials;

/**
 * AuthenticationMetaDataPopulator to retrieve the Authentication Type.
 * <p>
 * Note: Authentication Methods are exposed under the key:
 * <code>samlAuthenticationStatement::authMethod</code> in the Authentication
 * attributes map.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.1
 */

using System.Collections.Generic;
using System.Linq;
using NCAS.jasig.authentication.principal;

namespace NCAS.jasig.authentication
{
    public class SamlAuthenticationMetaDataPopulator :
        AuthenticationMetaDataPopulator
    {

        public static string ATTRIBUTE_AUTHENTICATION_METHOD = "samlAuthenticationStatementAuthMethod";

        public static string AUTHN_METHOD_PASSWORD = "urn:oasis:names:tc:SAML:1.0:am:password";

        public static string AUTHN_METHOD_SSL_TLS_CLIENT = "urn:ietf:rfc:2246";

        public static string AUTHN_METHOD_X509_PUBLICKEY = "urn:oasis:names:tc:SAML:1.0:am:X509-PKI";

        public static string AUTHN_METHOD_UNSPECIFIED = "urn:oasis:names:tc:SAML:1.0:am:unspecified";

        private Dictionary<string, string> authenticationMethods = new Dictionary<string, string>();

        public SamlAuthenticationMetaDataPopulator()
        {
            this.authenticationMethods.Add(
                typeof(HttpBasedServiceCredentials).FullName,
                AUTHN_METHOD_SSL_TLS_CLIENT);
            this.authenticationMethods.Add(
                typeof(UsernamePasswordCredentials).FullName,
            AUTHN_METHOD_PASSWORD);

            // Next two classes are in other modules, so avoid using Class#getName() to prevent circular dependency
            this.authenticationMethods.Add(
                "org.jasig.cas.adaptors.trusted.authentication.principal.PrincipalBearingCredentials",
                AUTHN_METHOD_UNSPECIFIED);
            this.authenticationMethods.Add(
                "org.jasig.cas.adaptors.x509.authentication.principal.X509CertificateCredentials",
                AUTHN_METHOD_X509_PUBLICKEY);
        }

        public Authentication populateAttributes(Authentication authentication, Credentials credentials)
        {

            string credentialsClass = credentials.GetType().FullName;
            string authenticationMethod = this.authenticationMethods.First(x => x.Key == credentialsClass).Value;

            authentication.getAttributes().Add(ATTRIBUTE_AUTHENTICATION_METHOD, authenticationMethod);

            return authentication;
        }

        /**
     * Map of user-defined mappings. Note it is possible to over-ride the
     * defaults. Mapping should be of the following type:
     * <p>(<string version of //package/Class Name> <SAML Type>)
     * <p>
     * Example: (<"org.jasig.cas.authentication.principal.HttpBasedServiceCredentials">
     * <SAMLAuthenticationStatement.AuthenticationMethod_SSL_TLS_Client>)
     * 
     * @param userDefinedMappings map of user defined authentication types.
     */
        public void setUserDefinedMappings(Dictionary<string, string> userDefinedMappings)
        {
            authenticationMethods = this.authenticationMethods.Union(userDefinedMappings).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
