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
//package org.jasig.cas.authentication.handler;

/**
 * Generic Bad Credentials Exception. This can be thrown when the system knows
 * the credentials are not valid specificially because they are bad. Subclasses
 * can be specific to a certain type of Credentials
 * (BadUsernamePassowrdCredentials).
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using System;

namespace NCAS.jasig.authentication.handler
{
    public class BadCredentialsAuthenticationException :
        AuthenticationException
    {

        /**
     * Static instance of class to prevent cost incurred by creating new
     * instance.
     */
        public static BadCredentialsAuthenticationException ERROR = new BadCredentialsAuthenticationException();

        /** UID for serializable objects. */
        private static long serialVersionUID = 3256719585087797044L;

        /**
     * Default constructor that does not allow the chaining of exceptions and
     * uses the default code as the error code for this exception.
     */
        public static string CODE = "error.authentication.credentials.bad";

        /**
     * Default constructor that does not allow the chaining of exceptions and
     * uses the default code as the error code for this exception.
     */
        public BadCredentialsAuthenticationException()
            : base(CODE)
        {
            ;
        }

        /**
     * Constructor to allow for the chaining of exceptions. Constructor defaults
     * to default code.
     * 
     * @param Exception the chainable exception.
     */
        public BadCredentialsAuthenticationException(Exception Exception)
            : base(CODE, Exception)
        {
            ;
        }

        /**
     * Constructor method to allow for providing a custom code to associate with
     * this exception.
     * 
     * @param code the code to use.
     */
        public BadCredentialsAuthenticationException(string code)
            : base(code)
        {
            ;
        }

        /**
     * Constructor to allow for the chaining of exceptions and use of a
     * non-default code.
     * 
     * @param code the user-specified code.
     * @param Exception the chainable exception.
     */
        public BadCredentialsAuthenticationException(string code,
                                                      Exception Exception)
            : base(code, Exception)
        {
            ;
        }
    }
}
