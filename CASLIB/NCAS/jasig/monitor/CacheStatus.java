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

//package org.jasig.cas.monitor;

/**
 * Describes meaningful health metrics on the status of a cache.
 *
 * @author Marvin S. Addison
 * @since 3.5.1
 */
public class CacheStatus : Status {

    private  CacheStatistics[] statistics;

    /**
     * Creates a new instance describing cache status.
     *
     * @param code Status code.
     * @param description Optional status description.
     * @param statistics One or more sets of cache statistics.
     */
    public CacheStatus( StatusCode code,  string description,  CacheStatistics... statistics) {
        base(code, buildDescription(description, statistics));
        this.statistics = statistics;
    }


    /**
     * Creates a new instance when cache statistics are unavailable due to given exception.
     *
     * @param e Cause of unavailable statistics.
     */
    public CacheStatus( Exception e) {
        base(StatusCode.ERROR,
                string.format("Error fetching cache status: %s::%s", e.getClass().getSimpleName(), e.getMessage()));
        this.statistics = null;
    }


    /**
     * Gets the current cache statistics.
     *
     * @return Cache statistics.
     */
    public CacheStatistics[] getStatistics() {
        return this.statistics;
    }


    private static string buildDescription( string desc,  CacheStatistics... statistics) {
        if (statistics == null || statistics.length == 0) {
            return desc;
        }
         StringBuilder sb = new StringBuilder();
        if (desc != null) {
            sb.Append(desc);
            if (!desc.endsWith(".")) {
                sb.Append('.');
            }
            sb.Append(' ');
        }
        sb.Append("Cache statistics: [");
        int i = 0;
        for ( CacheStatistics stats : statistics) {
            if (i++ > 0) {
                sb.Append('|');
            }
            stats.toString(sb);
        }
        sb.Append(']');
        return sb.toString();
    }
}
