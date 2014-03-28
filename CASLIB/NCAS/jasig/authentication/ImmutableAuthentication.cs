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
//package org.jasig.cas.authentication;

//import java.util.Collections;
//import java.util.Date;
//import java.util.HashMap;
//import java.util.Map;

//import org.jasig.cas.authentication.principal.Principal;

/**
 * Default implementation of Authentication interface. ImmutableAuthentication
 * is an immutable object and thus its attributes cannot be changed.
 * <p>
 * Instanciators of the ImmutableAuthentication class must take care that the
 * map they provide is serializable (i.e. HashMap).
 * 
 * @author Dmitriy Kopylenko
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using System;
using System.Collections.Generic;
using NCAS.jasig.authentication.principal;

namespace NCAS.jasig.authentication
{
    public class ImmutableAuthentication : AbstractAuthentication
    {

        /** UID for serializing. */
        private static long serialVersionUID = 3906647483978365235L;

        private static Dictionary<string, Object> EMPTY_MAP = new Dictionary<string, Object>();

        /** The date/time this authentication object became valid. */
        DateTime authenticatedDate;

        /**
     * Constructor that accepts both a principal and a map.
     * 
     * @param principal Principal representing user
     * @param attributes Authentication attributes map.
     * @throws IllegalArgumentException if the principal is null.
     */
        public ImmutableAuthentication(Principal principal,
                                       Dictionary<string, Object> attributes)
            : base(principal, new Dictionary<string, object>())
        {
            //base(principal, attributes == null || attributes.isEmpty()
            //    ? EMPTY_MAP : Collections.unmodifiableMap(attributes));

            this.authenticatedDate = System.DateTime.Now;
        }

        /**
     * Constructor that assumes there are no additional authentication
     * attributes.
     * 
     * @param principal the Principal representing the authenticated entity.
     */
        public ImmutableAuthentication(Principal principal)
            : this(principal, null)
        {
            ;
        }

        public override DateTime getAuthenticatedDate()
        {
            return this.authenticatedDate;
        }

        public override DateTime AuthenticatedDate
        {
            get { return this.authenticatedDate; }
            set { this.authenticatedDate = value; }

        }
    }
}
