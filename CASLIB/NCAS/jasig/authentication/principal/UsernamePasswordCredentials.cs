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
////package org.jasig.cas.authentication.principal;

////import javax.validation.constraints.NotNull;
////import javax.validation.constraints.Size;

/**
 * UsernamePasswordCredentials respresents the username and password that a user
 * may provide in order to prove the authenticity of who they say they are.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.2 $ $Date: 2007/01/22 20:35:26 $
 * @since 3.0
 * <p>
 * This is a published and supported CAS Server 3 API.
 * </p>
 */

using System;

namespace NCAS.jasig.authentication.principal
{
    public class UsernamePasswordCredentials : Credentials
    {

        /** Unique ID for serialization. */
        private static long serialVersionUID = -8343864967200862794L;

        /** The username. */
        ////@NotNull
        //@Size(min=1,message = "required.username")
        private string username;

        /** The password. */
        ////@NotNull
        //@Size(min=1, message = "required.password")
        private string password;

        /**
     * @return Returns the password.
     */
        public string getPassword()
        {
            return this.password;
        }

        /**
     * @param password The password to set.
     */
        public void setPassword(string password)
        {
            this.password = password;
        }

        /**
     * @return Returns the userName.
     */
        public string getUsername()
        {
            return this.username;
        }

        /**
     * @param userName The userName to set.
     */
        public void setUsername(string userName)
        {
            this.username = userName;
        }

        public string ToString()
        {
            return "[username: " + this.username + "]";
        }

        //@Override
        public bool equals(Object o)
        {
            if (this == o) return true;
            if (o == null || this.GetType() != o.GetType()) return false;

            UsernamePasswordCredentials that = (UsernamePasswordCredentials)o;

            if (this.password != null ? !this.password.Equals(that.password) : that.password != null) return false;
            if (this.username != null ? !this.username.Equals(that.username) : that.username != null) return false;

            return true;
        }

        //@Override
        public int hashCode()
        {
            int result = this.username != null ? this.username.GetHashCode() : 0;
            result = 31 * result + (this.password != null ? this.password.GetHashCode() : 0);
            return result;
        }
    }
}
