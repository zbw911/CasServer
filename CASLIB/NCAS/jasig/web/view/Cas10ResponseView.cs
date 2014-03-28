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
//package org.jasig.cas.web.view;

//import java.util.Map;

//import javax.servlet.http.HttpRequest;
//import javax.servlet.http.HttpResponse;

//import org.jasig.cas.validation.Assertion;

/**
 * Custom View to Return the CAS 1.0 Protocol Response. Implemented as a view
 * class rather than a JSP (like CAS 2.0 spec) because of the requirement of the
 * line feeds to be "\n".
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCAS.jasig.validation;

namespace NCAS.jasig.web.view
{
    public class Cas10ResponseView : AbstractCasView
    {

        /**
     * Indicate whether this view will be generating the success response or
     * not.
     */
        private bool successResponse;

        protected void renderMergedOutputModel(Dictionary<string, Object> model,
                                               HttpRequest request, HttpResponse response)
        {
            Assertion assertion = getAssertionFrom(model);

            if (this.successResponse)
            {
                response.Write(
                    "yes\n"
                    + assertion.getChainedAuthentications().First().getPrincipal()
                          .getId() + "\n");
            }
            else
            {
                response.Write("no\n\n");
            }
        }

        public void setSuccessResponse(bool successResponse)
        {
            this.successResponse = successResponse;
        }
    }
}
