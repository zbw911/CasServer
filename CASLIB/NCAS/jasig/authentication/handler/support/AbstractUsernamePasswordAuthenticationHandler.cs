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
//package org.jasig.cas.authentication.handler.support;

//import org.jasig.cas.authentication.handler.*;
//import org.jasig.cas.authentication.principal.Credentials;
//import org.jasig.cas.authentication.principal.UsernamePasswordCredentials;

//import javax.validation.constraints.NotNull;

/**
 * Abstract class to override supports so that we don't need to duplicate the
 * check for UsernamePasswordCredentials.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 * <p>
 * This is a published and supported CAS Server 3 API.
 * </p>
 */

using System;
using NCAS.jasig.authentication.principal;

namespace NCAS.jasig.authentication.handler.support
{
    public abstract class AbstractUsernamePasswordAuthenticationHandler :
        AbstractPreAndPostProcessingAuthenticationHandler
    {

        /** Default class to support if one is not supplied. */
        private static Type DEFAULT_CLASS = typeof(UsernamePasswordCredentials);

        /** Class that this instance will support. */
        //@NotNull
        private Type classToSupport = DEFAULT_CLASS;

        /**
     * bool to determine whether to support subclasses of the class to
     * support.
     */
        private bool supportSubClasses = true;

        /**
     * PasswordEncoder to be used by subclasses to encode passwords for
     * comparing against a resource.
     */
        //@NotNull
        private PasswordEncoder passwordEncoder = new PlainTextPasswordEncoder();

        //@NotNull
        private PrincipalNameTransformer principalNameTransformer = new NoOpPrincipalNameTransformer();

        /**
     * Method automatically handles conversion to UsernamePasswordCredentials
     * and delegates to abstract authenticateUsernamePasswordInternal so
     * subclasses do not need to cast.
     */

        protected override bool doAuthentication(Credentials credentials)
        {
            return authenticateUsernamePasswordInternal((UsernamePasswordCredentials)credentials);
        }


        /**
         * Abstract convenience method that assumes the credentials passed in are a
         * subclass of UsernamePasswordCredentials.
         * 
         * @param credentials the credentials representing the Username and Password
         * presented to CAS
         * @return true if the credentials are authentic, false otherwise.
         * @ if authenticity cannot be determined.
         */

        protected abstract bool authenticateUsernamePasswordInternal(
            UsernamePasswordCredentials credentials);

        /**
         * Method to return the PasswordEncoder to be used to encode passwords.
         * 
         * @return the PasswordEncoder associated with this class.
         */

        protected PasswordEncoder getPasswordEncoder()
        {
            return this.passwordEncoder;
        }

        protected PrincipalNameTransformer getPrincipalNameTransformer()
        {
            return this.principalNameTransformer;
        }

        /**
         * Method to set the class to support.
         * 
         * @param classToSupport the class we want this handler to support
         * explicitly.
         */

        public void setClassToSupport(Type classToSupport)
        {
            this.classToSupport = classToSupport;
        }

        /**
         * Method to set whether this handler will support subclasses of the
         * supported class.
         * 
         * @param supportSubClasses bool of whether to support subclasses or not.
         */

        public void setSupportSubClasses(bool supportSubClasses)
        {
            this.supportSubClasses = supportSubClasses;
        }

        /**
         * Sets the PasswordEncoder to be used with this class.
         * 
         * @param passwordEncoder the PasswordEncoder to use when encoding
         * passwords.
         */

        public void setPasswordEncoder(PasswordEncoder passwordEncoder)
        {
            this.passwordEncoder = passwordEncoder;
        }

        public void setPrincipalNameTransformer(PrincipalNameTransformer principalNameTransformer)
        {
            this.principalNameTransformer = principalNameTransformer;
        }

        /**
         * @return true if the credentials are not null and the credentials class is
         * equal to the class defined in classToSupport.
         */

        public override bool supports(Credentials credentials)
        {
            return credentials != null
                   && (this.classToSupport.Equals(credentials.GetType()) || (this.classToSupport
                                                                                .IsAssignableFrom(credentials.GetType()))
                       && this.supportSubClasses);
        }
    }
}
