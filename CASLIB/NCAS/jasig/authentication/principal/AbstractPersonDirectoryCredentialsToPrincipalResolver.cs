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
//package org.jasig.cas.authentication.principal;

//import java.util.HashMap;
//import java.util.List;
//import java.util.Map;

//import org.jasig.services.persondir.IPersonAttributeDao;
//import org.jasig.services.persondir.IPersonAttributes;
//import org.jasig.services.persondir.support.StubPersonAttributeDao;
//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;

//import javax.validation.constraints.NotNull;

/**
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.1 $ $Date: 2005/08/19 18:27:17 $
 * @since 3.1
 *
 */

using System;
using System.Collections.Generic;
using NCAS.jasig.authentication.principal.support;
using NCAS.jasig.services.persondir;

namespace NCAS.jasig.authentication.principal
{
    public abstract class AbstractPersonDirectoryCredentialsToPrincipalResolver
        : CredentialsToPrincipalResolver
    {

        /** Log instance. */
        //protected  Logger log = LoggerFactory.getLogger(this.getClass());

        private bool returnNullIfNoAttributes = false;

        /** Repository of principal attributes to be retrieved */
        ////@NotNull
        private IPersonAttributeDao attributeRepository = new StubPersonAttributeDao(new Dictionary<string, List<Object>>());

        public Principal resolvePrincipal(Credentials credentials) {
            //if (log.isDebugEnabled()) {
            //    log.debug("Attempting to resolve a principal...");
            //}

            string principalId = this.extractPrincipalId(credentials);
        
            if (principalId == null) {
                return null;
            }
        
            //if (log.isDebugEnabled()) {
            //    log.debug("Creating SimplePrincipal for ["
            //        + principalId + "]");
            //}

            IPersonAttributes personAttributes = this.attributeRepository.getPerson(principalId);
            Dictionary<string, List<Object>> attributes;

            if (personAttributes == null) {
                attributes = null;
            } else {
                attributes = personAttributes.getAttributes();
            }

            if (attributes == null & !this.returnNullIfNoAttributes) {
                return new SimplePrincipal(principalId);
            }

            if (attributes == null) {
                return null;
            }
        
            Dictionary<string, Object> convertedAttributes = new Dictionary<string, Object>();
        
            foreach ( var entry in attributes) {
                string key = entry.Key;
                Object value = entry.Value.Count == 1 ? entry.Value[0] : entry.Value;
                convertedAttributes.Add(key, value);
            }
            return new SimplePrincipal(principalId, convertedAttributes);
        }

        public abstract bool supports(Credentials credentials);

        /**
     * Extracts the id of the user from the provided credentials.
     * 
     * @param credentials the credentials provided by the user.
     * @return the username, or null if it could not be resolved.
     */
        protected abstract string extractPrincipalId(Credentials credentials);

        public void setAttributeRepository(IPersonAttributeDao attributeRepository)
        {
            this.attributeRepository = attributeRepository;
        }

        public void setReturnNullIfNoAttributes(bool returnNullIfNoAttributes)
        {
            this.returnNullIfNoAttributes = returnNullIfNoAttributes;
        }
    }
}
