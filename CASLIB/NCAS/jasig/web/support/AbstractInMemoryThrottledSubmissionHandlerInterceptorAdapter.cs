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

//import java.util.DateTime;
//import java.util.Iterator;
//import java.util.Set;
//import java.util.concurrent.ConcurrentHashMap;
//import java.util.concurrent.ConcurrentMap;
//import java.util.concurrent.atomic.AtomicInteger;

//import javax.servlet.http.HttpRequest;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/**
 * Implementation of a HandlerInterceptorAdapter that keeps track of a mapping
 * of IP Addresses to number of failures to authenticate.
 * <p>
 * Note, this class relies on an external method for decrementing the counts (i.e. a Quartz Job) and runs independent of the
 * threshold of the parent.

 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0.5
 */

namespace NCAS.jasig.web.support
{
    public abstract class AbstractInMemoryThrottledSubmissionHandlerInterceptorAdapter : AbstractThrottledSubmissionHandlerInterceptorAdapter
    {

        private IDictionary<string, DateTime> ipMap = new ConcurrentDictionary<string, DateTime>();

        //@Override
        protected override bool exceedsThreshold(HttpRequest request)
        {
            DateTime last = this.ipMap.First(x => x.Key == this.constructKey(request)).Value;
            if (last == null)
            {
                return false;
            }
            return this.submissionRate(new DateTime(), last) > this.getThresholdRate();
        }

        //@Override
        protected override void recordSubmissionFailure(HttpRequest request)
        {
            this.ipMap.Add(this.constructKey(request), DateTime.Now);
        }

        protected abstract string constructKey(HttpRequest request);

        /**
     * This class relies on an external configuration to clean it up. It ignores the threshold data in the parent class.
     */
        public void decrementCounts()
        {
            ICollection<string> keys = this.ipMap.Keys;
            //log.debug("Decrementing counts for throttler.  Starting key count: " + keys.size());

            DateTime now = new DateTime();
            //string key;
            //for (iterator<string> iter = keys.GetEnumerator(); iter.hasNext(); )
            //{
            //    key = iter.next();
            //    if (submissionRate(now, this.ipMap.get(key)) < getThresholdRate())
            //    {
            //        log.trace("Removing entry for key {}", key);
            //        iter.remove();
            //    }
            //}

            foreach (var key in keys)
            {
                if (this.submissionRate(now, this.ipMap.First(x => x.Key == key).Value) < this.getThresholdRate())
                {
                    //log.trace("Removing entry for key {}", key);
                    //iter.remove();

                    this.ipMap.Remove(key);
                }
            }
            //log.debug("Done decrementing count for throttler.");
        }

        /**
     * Computes the instantaneous rate in between two given dates corresponding to two submissions.
     *
     * @param a First DateTime.
     * @param b Second DateTime.
     *
     * @return  Instantaneous submission rate in submissions/sec, e.g. <code>a - b</code>.
     */
        private double submissionRate(DateTime a, DateTime b)
        {
            return 1000.0 / (a.Ticks - b.Ticks);
        }
    }
}
