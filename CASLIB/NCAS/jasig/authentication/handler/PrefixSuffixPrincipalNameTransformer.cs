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

//package org.jasig.cas.authentication.handler;

/**
 * Transform the user id by adding a prefix or suffix.
 *
 * @author Howard Gilbert
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.3.6
 */

using System.Text;

namespace NCAS.jasig.authentication.handler
{
    public  class PrefixSuffixPrincipalNameTransformer : PrincipalNameTransformer {

        private string prefix;

        private string suffix;

        public string transform( string formUserId) {
            StringBuilder stringBuilder = new StringBuilder();

            if (this.prefix != null) {
                stringBuilder.Append(this.prefix);
            }

            stringBuilder.Append(formUserId);

            if (this.suffix != null) {
                stringBuilder.Append(this.suffix);
            }

            return stringBuilder.ToString();
        }

        public void setPrefix( string prefix) {
            this.prefix = prefix;
        }

        public void setSuffix( string suffix) {
            this.suffix = suffix;
        }
    }
}
