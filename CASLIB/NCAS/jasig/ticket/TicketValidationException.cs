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
//package org.jasig.cas.ticket;

//import org.jasig.cas.authentication.principal.Service;

/**
 * Exception to alert that there was an error validating the ticket.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using NCAS.jasig.authentication.principal;

namespace NCAS.jasig.ticket
{
    public class TicketValidationException : TicketException
    {

        /** Unique Serial ID. */
        private static long serialVersionUID = 3257004341537093175L;

        /** The code description. */
        private static string CODE = "INVALID_SERVICE";

        private Service service;

        /**
     * Constructs a TicketValidationException with the default exception code
     * and the original exception that was thrown.
     * 
     * @param Exception the chained exception
     */
        public TicketValidationException(Service service)
            : base(CODE)
        {
            ;
            this.service = service;
        }

        public Service getOriginalService()
        {
            return this.service;
        }

    }
}
