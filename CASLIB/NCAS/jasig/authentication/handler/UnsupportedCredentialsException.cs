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
 * The exception thrown when a Handler does not know how to determine the
 * validity of the credentials based on the fact that it does not know what to
 * do with the credentials presented.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using System;

namespace NCAS.jasig.authentication.handler
{
    public class UnsupportedCredentialsException :
        AuthenticationException
    {

        /** Static instance of UnsupportedCredentialsException. */
        public static UnsupportedCredentialsException ERROR = new UnsupportedCredentialsException();

        /** Unique ID for serializing. */
        private static long serialVersionUID = 3977861752513837361L;

        /** The code description of this exception. */
        private static string CODE = "error.authentication.credentials.unsupported";

        /**
     * Default constructor that does not allow the chaining of exceptions and
     * uses the default code as the error code for this exception.
     */
        public UnsupportedCredentialsException()
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
        public UnsupportedCredentialsException(Exception Exception)
            : base(CODE, Exception)
        {
            ;
        }
    }
}
