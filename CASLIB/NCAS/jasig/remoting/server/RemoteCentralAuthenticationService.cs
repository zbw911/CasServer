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
//package org.jasig.cas.remoting.server;

//import org.jasig.cas.CentralAuthenticationService;
//import org.jasig.cas.authentication.principal.Credentials;
//import org.jasig.cas.authentication.principal.Service;
//import org.jasig.cas.ticket.TicketException;
//import org.jasig.cas.validation.Assertion;
//import org.springframework.util.Assert;

//import javax.validation.*;
//import javax.validation.constraints.NotNull;
//import java.util.Set;

/**
 * Wrapper implementation around a CentralAuthenticationService that does
 * completes the marshalling of parameters from the web-service layer to the
 * service layer. Typically the only thing that is done is to validate the
 * parameters (as you would in the web tier) and then delegate to the service
 * layer.
 * <p>
 * The following properties are required:
 * </p>
 * <ul>
 * <li>centralAuthenticationService - the service layer we are delegating to.</li>
 * </ul>
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using NCAS.jasig.authentication.principal;
using NCAS.jasig.validation;

namespace NCAS.jasig.remoting.server
{
    public class RemoteCentralAuthenticationService : CentralAuthenticationService
    {

        /** The CORE to delegate to. */
        //@NotNull
        private CentralAuthenticationService centralAuthenticationService;

        /** The validators to check the Credentials. */
        //@NotNull
        //private Validator validator = Validation.buildDefaultValidatorFactory().getValidator();

        /**
     * @throws IllegalArgumentException if the Credentials are null or if given
     * invalid credentials.
     */
        public string createTicketGrantingTicket(Credentials credentials)
        {
            //Assert.notNull(credentials, "credentials cannot be null");
            this.checkForErrors(credentials);

            return this.centralAuthenticationService.createTicketGrantingTicket(credentials);
        }

        public string grantServiceTicket(string ticketGrantingTicketId, Service service)
        {
            return this.centralAuthenticationService.grantServiceTicket(ticketGrantingTicketId, service);
        }

        /**
     * @throws IllegalArgumentException if given invalid credentials
     */
        public string grantServiceTicket(string ticketGrantingTicketId, Service service, Credentials credentials)
        {
            this.checkForErrors(credentials);

            return this.centralAuthenticationService.grantServiceTicket(ticketGrantingTicketId, service, credentials);
        }

        public Assertion validateServiceTicket(string serviceTicketId, Service service)
        {
            return this.centralAuthenticationService.validateServiceTicket(serviceTicketId, service);
        }

        public void destroyTicketGrantingTicket(string ticketGrantingTicketId)
        {
            this.centralAuthenticationService.destroyTicketGrantingTicket(ticketGrantingTicketId);
        }

        /**
     * @throws IllegalArgumentException if the credentials are invalid.
     */
        public string delegateTicketGrantingTicket(string serviceTicketId, Credentials credentials)
        {
            this.checkForErrors(credentials);

            return this.centralAuthenticationService.delegateTicketGrantingTicket(serviceTicketId, credentials);
        }

        private void checkForErrors(Credentials credentials)
        {
            if (credentials == null)
            {
                return;
            }

            //Set<ConstraintViolation<Credentials>> errors = this.validator.validate(credentials);
            //if (!errors.isEmpty())
            //{
            //    throw new IllegalArgumentException("Error validating credentials: " + errors.toString());
            //}
        }

        /**
     * Set the CentralAuthenticationService.
     * 
     * @param centralAuthenticationService The CentralAuthenticationService to
     * set.
     */
        public void setCentralAuthenticationService(
            CentralAuthenticationService centralAuthenticationService)
        {
            this.centralAuthenticationService = centralAuthenticationService;
        }

        ///**
        // * Set the list of validators.
        // * 
        // * @param validator The array of validators to use.
        // */
        //public void setValidator(Validator validator)
        //{
        //    this.validator = validator;
        //}
    }
}
