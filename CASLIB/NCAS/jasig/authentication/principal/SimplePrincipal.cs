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

//import java.util.Collections;
//import java.util.HashMap;
//import java.util.Map;

//import org.springframework.util.Assert;

/**
 * Simple implementation of a AttributePrincipal that exposes an unmodifiable
 * map of attributes.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.3 $ $Date: 2007/04/19 20:13:01 $
 * @since 3.1
 */

using System;
using System.Collections.Generic;

namespace NCAS.jasig.authentication.principal
{
    public class SimplePrincipal : Principal
    {

        private static Dictionary<string, Object> EMPTY_MAP = new Dictionary<string, Object>();

        /**
     * Unique Id for Serialization.
     */
        private static long serialVersionUID = -5265620187476296219L;

        /** The unique identifier for the principal. */
        private string id;

        /** Map of attributes for the Principal. */
        private Dictionary<string, Object> attributes;

        public SimplePrincipal(string id)
            : this(id, null)
        {
            //this(id, null);

        }

        public SimplePrincipal(string id, Dictionary<string, Object> attributes)
        {
            //Assert.notNull(id, "id cannot be null");
            this.id = id;

            this.attributes = attributes == null || attributes.Count == 0
                                  ? EMPTY_MAP : attributes;
        }

        /**
     * Returns an immutable map.
     */
        public Dictionary<string, Object> getAttributes()
        {
            return this.attributes;
        }

        public string toString()
        {
            return this.id;
        }

        public int hashCode()
        {
            return base.GetHashCode() ^ this.id.GetHashCode();
        }

        public string getId()
        {
            return this.id;
        }

        public bool equals(Object o)
        {
            if (o == null || !this.GetType().Equals(o.GetType()))
            {
                return false;
            }

            SimplePrincipal p = (SimplePrincipal)o;

            return this.id.Equals(p.getId());
        }
    }
}
