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
////package org.jasig.cas.authentication.handler;

/**
 * The most generic type of authentication exception that one can catch if not
 * sure what specific implementation will be thrown. Top of the tree of all
 * other AuthenticationExceptions.
 *
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using System;

namespace NCAS.jasig.authentication.handler
{
    public abstract class AuthenticationException : Exception
    {

        /** Serializable ID. */
        private static long serialVersionUID = 3906648604830611762L;

        /** The code to return for resolving to a message description. */
        private string code;

        /** The error type that provides additional info about the nature of the exception cause **/
        private string type = "error";

        /**
     * Constructor that takes a code description of the error. These codes
     * normally have a corresponding entries in the messages file for the
     * internationalization of error messages.
     *
     * @param code The short unique identifier for this error.
     */
        public AuthenticationException(string code)
        {
            this.code = code;
        }

        /**
     * Constructor that takes a <code>code</code> description of the error along with the exception
     * <code>msg</code> generally for logging purposes. These codes normally have a corresponding
     * entries in the messages file for the internationalization of error messages.
     *
     * @param code The short unique identifier for this error.
     * @param msg The error message associated with this exception for additional logging purposes.
     */
        public AuthenticationException(string code, string msg)
            : base(msg)
        {
            //base(msg);
            this.code = code;
        }

        /**
     * Constructor that takes a <code>code</code> description of the error along with the exception
     * <code>msg</code> generally for logging purposes and the <code>type</code> of the error that originally caused the exception.
     * These codes normally have a corresponding entries in the messages file for the internationalization of error messages.
     *
     * @param code The short unique identifier for this error.
     * @param msg The error message associated with this exception for additional logging purposes.
     * @param type The type of the error message that caused the exception to be thrown. By default,
     * all errors are considered of <code>error</code>.
     */
        public AuthenticationException(string code, string msg, string type)
            : base(msg)
        {
            ;
            this.code = code;
            this.type = type;
        }

        /**
     * Constructor that takes a code description of the error and the chained
     * exception. These codes normally have a corresponding entries in the
     * messages file for the internationalization of error messages.
     *
     * @param code The short unique identifier for this error.
     * @param Exception The chained exception for this AuthenticationException
     */
        public AuthenticationException(string code, Exception Exception)
            : base(code, Exception)
        {
            ;
            this.code = code;
        }

        /**
     * Method to return the error type of this exception
     *
     * @return the string identifier for the cause of this error.
     */
        public string getType()
        {
            return this.type;
        }

        /**
     * Method to return the unique identifier for this error type.
     *
     * @return the string identifier for this error type.
     */
        public string getCode()
        {
            return this.code;
        }


        public string toString()
        {
            string msg = this.getCode();

            if (this.Message != null && this.Message.Trim().Length > 0)
                msg = ":" + this.Message;
            return msg;
        }

    }
}
