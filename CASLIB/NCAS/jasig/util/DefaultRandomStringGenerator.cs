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

////import java.security.SecureRandom;

/**
 * Implementation of the RandomStringGenerator that allows you to define the
 * length of the random part.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using System;

namespace NCAS.jasig.util
{
    public class DefaultRandomStringGenerator : RandomStringGenerator
    {

        /** The array of printable characters to be used in our random string. */
        private static char[] PRINTABLE_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ012345679"
            .ToCharArray();

        /** The default maximum length. */
        private static int DEFAULT_MAX_RANDOM_LENGTH = 35;

        /** An instance of secure random to ensure randomness is secure. */
        private Random randomizer = new Random();

        /** The maximum length the random string can be. */
        private int maximumRandomLength;

        public DefaultRandomStringGenerator()
        {
            this.maximumRandomLength = DEFAULT_MAX_RANDOM_LENGTH;
        }

        public DefaultRandomStringGenerator(int maxRandomLength)
        {
            this.maximumRandomLength = maxRandomLength;
        }

        public int getMinLength()
        {
            return this.maximumRandomLength;
        }

        public int getMaxLength()
        {
            return this.maximumRandomLength;
        }

        public string getNewString()
        {
            byte[] random = this.getNewStringAsBytes();

            return this.convertBytesToString(random);
        }


        public byte[] getNewStringAsBytes()
        {
            byte[] random = new byte[this.maximumRandomLength];

            this.randomizer.NextBytes(random);

            return random;
        }

        private string convertBytesToString(byte[] random)
        {
            char[] output = new char[random.Length];
            for (int i = 0; i < random.Length; i++)
            {
                int index = Math.Abs(random[i] % PRINTABLE_CHARACTERS.Length);
                output[i] = PRINTABLE_CHARACTERS[index];
            }

            return new string(output);
        }
    }
}
