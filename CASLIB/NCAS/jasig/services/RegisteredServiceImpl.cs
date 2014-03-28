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
////package org.jasig.cas.services;

////import org.jasig.cas.authentication.principal.Service;
////import org.springframework.util.AntPathMatcher;
////import org.springframework.util.PathMatcher;

////import javax.persistence.DiscriminatorValue;
////import javax.persistence.Entity;

/**
 * Mutable registered service that uses Ant path patterns for service matching.
 * 
 * @author Scott Battaglia
 * @author Marvin S. Addison
 * @version $Revision$ $Date$
 * @since 3.1
 */
//@Entity
//@DiscriminatorValue("ant")

using System;
using NCAS.jasig.authentication.principal;

namespace NCAS.jasig.services
{
    public class RegisteredServiceImpl : AbstractRegisteredService
    {

        /** Unique Id for serialization. */
        private static long serialVersionUID = -5906102762271197627L;

        //private static PathMatcher PATH_MATCHER = new AntPathMatcher();

        public override void setServiceId(string id)
        {
            this.serviceId = id;
        }

        public override bool matches(Service service)
        {
            throw new NotImplementedException();
            //return service != null && PATH_MATCHER.match(this.serviceId.toLowerCase(), service.getId().toLowerCase());
        }

        protected override AbstractRegisteredService newInstance()
        {
            return new RegisteredServiceImpl();
        }
    }
}

