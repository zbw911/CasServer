using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NCAS.jasig.authentication;
using NCAS.jasig.authentication.principal;

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


/**
 * Default implementation of the Assertion interface which returns the minimum
 * number of attributes required to conform to the CAS 2 protocol.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */
namespace NCAS.jasig.validation
{
    public class ImmutableAssertionImpl : Assertion
    {

        /** Unique Id for Serialization. */
        private static long serialVersionUID = -1921502350732798866L;

        /** The list of principals. */
        private List<Authentication> principals;

        /** Was this the result of a new login. */
        private bool fromNewLogin;

        /** The service we are asserting this ticket for. */
        private Service service;

        /**
     * Constructs a new ImmutableAssertion out of the given parameters.
     * 
     * @param principals the chain of principals
     * @param service The service we are asserting this ticket for.
     * @param fromNewLogin was the service ticket from a new login.
     * @throws IllegalArgumentException if there are no principals.
     */
        public ImmutableAssertionImpl(List<Authentication> principals, Service service,
                                       bool fromNewLogin)
        {
            //Assert.notNull(principals, "principals cannot be null");
            //Assert.notNull(service, "service cannot be null");
            //Assert.notEmpty(principals, "principals cannot be empty");

            this.principals = principals;
            this.service = service;
            this.fromNewLogin = fromNewLogin;
        }

        public ReadOnlyCollection<Authentication> getChainedAuthentications()
        {
            //return Collections.unmodifiableList(this.principals);

            return this.principals.AsReadOnly();
        }

        public bool isFromNewLogin()
        {
            return this.fromNewLogin;
        }

        public Service getService()
        {
            return this.service;
        }

        public bool equals(Object o)
        {
            if (o == null
                || !this.GetType().IsAssignableFrom(o.GetType()))
            {
                return false;
            }

            Assertion a = (Assertion)o;

            return this.service.Equals(a.getService()) && this.fromNewLogin == a.isFromNewLogin() && this.principals.Equals(a.getChainedAuthentications());
        }

        public int hashCode()
        {
            return 15 * this.service.GetHashCode() ^ this.principals.GetHashCode();
        }

        public string toString()
        {
            return "[principals={" + this.principals.ToString() + "} for service=" + this.service.ToString() + "]";
        }
    }
}
