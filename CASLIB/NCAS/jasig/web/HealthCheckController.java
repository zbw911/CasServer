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
//package org.jasig.cas.web;

//import java.util.Map;
//import javax.servlet.http.HttpRequest;
//import javax.servlet.http.HttpResponse;
//import javax.validation.constraints.NotNull;

//import org.jasig.cas.monitor.HealthCheckMonitor;
//import org.jasig.cas.monitor.HealthStatus;
//import org.jasig.cas.monitor.Status;
//import org.springframework.web.servlet.ModelAndView;
//import org.springframework.web.servlet.mvc.AbstractController;

/**
 * Reports overall CAS health based on the observations of the configured {@link HealthCheckMonitor} instance.
 *
 * @author Marvin S. Addison
 * @version $Revision: $
 */
public class HealthCheckController : AbstractController {

    /** Prefix for custom response headers with health check details. */
    private static  string HEADER_PREFIX = "X-CAS-";

    //@NotNull
    private HealthCheckMonitor healthCheckMonitor;


    /**
     * Sets the health check monitor used to observe system health.
     * @param monitor Health monitor configured with subordinate monitors that observe specific aspects of overall
     *                system health.
     */
    public void setHealthCheckMonitor( HealthCheckMonitor monitor) {
        this.healthCheckMonitor = monitor;
    }


    /** {@inheritDoc} */
    protected ModelAndView handleRequestInternal(
             HttpRequest request,  HttpResponse response)
             {

         HealthStatus healthStatus = this.healthCheckMonitor.observe();
         StringBuilder sb = new StringBuilder();
        sb.Append("Health: ").Append(healthStatus.getCode());
        string name;
        Status status;
        int i = 0;
        for ( Map.Entry<string, Status> entry : healthStatus.getDetails().entrySet()) {
            name = entry.getKey();
            status = entry.getValue();
            response.addHeader("X-CAS-" + name, string.format("%s;%s", status.getCode(), status.getDescription()));

            sb.Append("\n\n\t").Append(++i).Append('.').Append(name).Append(": ");
            sb.Append(status.getCode());
            if (status.getDescription() != null) {
                sb.Append(" - ").Append(status.getDescription());
            }
        }
        response.setStatus(healthStatus.getCode().value());
        response.setContentType("text/plain");
        response.getOutputStream().write(sb.toString().getBytes(response.getCharacterEncoding()));

        // Return null to signal MVC framework that we handled response directly
        return null;
    }
}
