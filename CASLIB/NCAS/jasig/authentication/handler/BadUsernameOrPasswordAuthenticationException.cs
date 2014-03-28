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
 * Exception to throw when we know the credentials provided were
 * username/password and the combination is wrong.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using System;

namespace NCAS.jasig.authentication.handler
{
    public class BadUsernameOrPasswordAuthenticationException :
        BadCredentialsAuthenticationException
    {

        /** Static instance of BadUsernameOrPasswordAuthenticationException. */
        public static BadUsernameOrPasswordAuthenticationException ERROR = new BadUsernameOrPasswordAuthenticationException();

        /** Unique ID for serializing. */
        private static long serialVersionUID = 3977861752513837361L;

        /** The default code for this exception used for message resolving. */
        private static string CODE = "error.authentication.credentials.bad.usernameorpassword";

        /**
     * Default constructor that does not allow the chaining of exceptions and
     * uses the default code as the error code for this exception.
     */
        public BadUsernameOrPasswordAuthenticationException()
            : base(CODE)
        {
            ;
        }

        /**
     * Constructor that allows for the chaining of exceptions. Defaults to the
     * default code provided for this exception.
     * 
     * @param Exception the chained exception.
     */
        public BadUsernameOrPasswordAuthenticationException(
            Exception Exception)
            : base(CODE, Exception)
        {
            ;
        }

        /**
     * Constructor that allows for providing a custom error code for this class.
     * Error codes are often used to resolve exceptions into messages. Providing
     * a custom error code allows the use of a different message.
     * 
     * @param code the custom code to use with this exception.
     */
        public BadUsernameOrPasswordAuthenticationException(string code)
            : base(code)
        {
            ;
        }

        /**
     * Constructor that allows for chaining of exceptions and a custom error
     * code.
     * 
     * @param code the custom error code to use in message resolving.
     * @param Exception the chained exception.
     */
        public BadUsernameOrPasswordAuthenticationException(string code,
                                                            Exception Exception)
            : base(code, Exception)
        {
            ;
        }
    }
}
