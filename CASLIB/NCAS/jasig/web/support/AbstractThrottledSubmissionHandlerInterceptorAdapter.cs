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
//package org.jasig.cas.web.support;

//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;
//import org.springframework.beans.factory.InitializingBean;
//import org.springframework.web.servlet.handler.HandlerInterceptorAdapter;
//import org.springframework.web.servlet.ModelAndView;
//import org.springframework.webflow.execution.HttpContext;

//import javax.servlet.http.HttpRequest;
//import javax.servlet.http.HttpResponse;
//import javax.validation.constraints.Min;
//import javax.validation.constraints.NotNull;

/**
 * Abstract implementation of the handler that has all of the logic.  Encapsulates the logic in case we get it wrong!
 *
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.3.5
 */

using System;
using System.Web;
using NCAS.jasig.web.MOCK2JAVA;

namespace NCAS.jasig.web.support
{
    public abstract class AbstractThrottledSubmissionHandlerInterceptorAdapter //: HandlerInterceptorAdapter, InitializingBean
    {

        private static int DEFAULT_FAILURE_THRESHOLD = 100;

        private static int DEFAULT_FAILURE_RANGE_IN_SECONDS = 60;

        private static string DEFAULT_USERNAME_PARAMETER = "username";

        private static string SUCCESSFUL_AUTHENTICATION_EVENT = "success";

        //protected  Logger log = LoggerFactory.getLogger(getClass());

        //@Min(0)
        private int failureThreshold = DEFAULT_FAILURE_THRESHOLD;

        //@Min(0)
        private int failureRangeInSeconds = DEFAULT_FAILURE_RANGE_IN_SECONDS;

        //@NotNull
        private string usernameParameter = DEFAULT_USERNAME_PARAMETER;

        private double thresholdRate;


        public void afterPropertiesSet()
        {
            this.thresholdRate = (double)this.failureThreshold / (double)this.failureRangeInSeconds;
        }


        //@Override
        public bool preHandle(HttpRequest request, HttpResponse response, Object o)
        {
            // we only care about post because that's the only instance where we can get anything useful besides IP address.
            if (!"POST".Equals(request.HttpMethod))
            {
                return true;
            }

            if (this.exceedsThreshold(request))
            {
                this.recordThrottle(request);
                response.StatusCode = 403;//

                response.StatusDescription = ("Access Denied for user [" + request.getParameter(this.usernameParameter) + " from IP Address [" + ".." + "]");
                response.Flush();
                return false;
            }

            return true;
        }

        //@Override
        public void postHandle(HttpRequest request, HttpResponse response, Object o, ModelAndView modelAndView)
        {
            if (!"POST".Equals(request.HttpMethod))
            {
                return;
            }

            //HttpContext context = (HttpContext)request.getAttribute("flowRequestContext");

            //HttpContext context = (HttpContext)request.RequestContext;
            //if (context == null) //|| context.getCurrentEvent() == null)
            //{
            //    return;
            //}

            //// User successfully authenticated
            //if (SUCCESSFUL_AUTHENTICATION_EVENT.equals(context.getCurrentEvent().getId()))
            //{
            //    return;
            //}

            // User submitted invalid credentials, so we update the invalid login count
            this.recordSubmissionFailure(request);
        }

        public void setFailureThreshold(int failureThreshold)
        {
            this.failureThreshold = failureThreshold;
        }

        public void setFailureRangeInSeconds(int failureRangeInSeconds)
        {
            this.failureRangeInSeconds = failureRangeInSeconds;
        }

        public void setUsernameParameter(string usernameParameter)
        {
            this.usernameParameter = usernameParameter;
        }

        protected double getThresholdRate()
        {
            return this.thresholdRate;
        }

        protected int getFailureThreshold()
        {
            return this.failureThreshold;
        }

        protected int getFailureRangeInSeconds()
        {
            return this.failureRangeInSeconds;
        }

        protected string getUsernameParameter()
        {
            return this.usernameParameter;
        }

        protected void recordThrottle(HttpRequest request)
        {
            //log.warn("Throttling submission from {}.  More than {} failed login attempts within {} seconds.",
            //        new Object[] { request.getRemoteAddr(), failureThreshold, failureRangeInSeconds });
        }

        protected abstract void recordSubmissionFailure(HttpRequest request);

        protected abstract bool exceedsThreshold(HttpRequest request);
    }
}
