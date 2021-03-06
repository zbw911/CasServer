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
//package org.jasig.cas.services;

/**
 * Exception thrown when a service attempts to proxy when it is not allowed to.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.1
 */

using System;

namespace NCAS.jasig.services
{
    public class UnauthorizedProxyingException : UnauthorizedServiceException
    {

        /**
     * Comment for <code>serialVersionUID</code>
     */
        private static long serialVersionUID = -7307803750894078575L;

        /** The code description. */
        private static string CODE = "service.not.authorized.proxy";

        public UnauthorizedProxyingException()
            : base(CODE)
        {
            ;
        }

        public UnauthorizedProxyingException(string message, Exception cause)
            : base(message, cause)
        {

        }

        public UnauthorizedProxyingException(string message)
            : base(message)
        {
            ;
        }
    }
}
