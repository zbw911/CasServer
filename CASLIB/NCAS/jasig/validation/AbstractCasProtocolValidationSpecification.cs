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
////package org.jasig.cas.validation;

/**
 * Base validation specification for the CAS protocol. This specification checks
 * for the presence of renew=true and if requested, succeeds only if ticket
 * validation is occurring from a new login.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

namespace NCAS.jasig.validation
{
    public abstract class AbstractCasProtocolValidationSpecification :
        ValidationSpecification
    {

        /** The default value for the renew attribute is false. */
        private static bool DEFAULT_RENEW = false;

        /** Denotes whether we should always authenticate or not. */
        private bool renew;

        public AbstractCasProtocolValidationSpecification()
        {
            this.renew = DEFAULT_RENEW;
        }

        public AbstractCasProtocolValidationSpecification(bool renew)
        {
            this.renew = renew;
        }

        /**
     * Method to set the renew requirement.
     * 
     * @param renew The renew value we want.
     */
        public void setRenew(bool renew)
        {
            this.renew = renew;
        }

        /**
     * Method to determine if we require renew to be true.
     * 
     * @return true if renew is required, false otherwise.
     */
        public bool isRenew()
        {
            return this.renew;
        }

        public bool isSatisfiedBy(Assertion assertion)
        {
            return this.isSatisfiedByInternal(assertion)
                   && ((!this.renew) || (assertion.isFromNewLogin() && this.renew));
        }

        /**
     * Template method to allow for additional checks by subclassed methods
     * without needing to call super.isSatisfiedBy(...).
     */
        protected abstract bool isSatisfiedByInternal(Assertion assertion);
    }
}
