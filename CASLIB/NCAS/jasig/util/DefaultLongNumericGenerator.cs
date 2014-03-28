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
////package org.jasig.cas.util;

////import java.util.concurrent.atomic.AtomicLong;

/**
 * The default numeric generator for generating long values. Implementation
 * allows for wrapping (to restart count) if the maximum is reached.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

namespace NCAS.jasig.util
{
    public class DefaultLongNumericGenerator : LongNumericGenerator
    {

        /** The maximum length the string can be. */
        private static int MAX_STRING_LENGTH = long.MaxValue.ToString().Length;

        /** The minimum length the string can be. */
        private static int MIN_STRING_LENGTH = 1;

        private long count;

        public DefaultLongNumericGenerator()
            : this(0)
        {
            //System.Threading.Interlocked.Increment()
            // nothing to do
        }

        public DefaultLongNumericGenerator(long initialValue)
        {
            this.count = initialValue;
        }

        public long getNextLong()
        {
            return this.getNextValue();
        }

        public string getNextNumberAsString()
        {
            return this.getNextValue().ToString();
        }

        public int maxLength()
        {
            return DefaultLongNumericGenerator.MAX_STRING_LENGTH;
        }

        public int minLength()
        {
            return DefaultLongNumericGenerator.MIN_STRING_LENGTH;
        }

        protected long getNextValue()
        {
            //if (this.count.compareAndSet(Long.MAX_VALUE, 0))
            //{
            //    return Long.MAX_VALUE;
            //}

            //System.Threading.Interlocked.CompareExchange()
            //return this.count.getAndIncrement();



            //System.Threading.Interlocked.CompareExchange()

            System.Threading.Interlocked.Increment(ref this.count);

            return this.count;
        }
    }
}
