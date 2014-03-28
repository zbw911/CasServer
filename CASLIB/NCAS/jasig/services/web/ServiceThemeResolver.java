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
//package org.jasig.cas.services.web;

//import org.jasig.cas.authentication.principal.Service;
//import org.jasig.cas.services.RegisteredService;
//import org.jasig.cas.services.ServicesManager;
//import org.jasig.cas.web.support.ArgumentExtractor;
//import org.jasig.cas.web.support.WebUtils;
//import org.springframework.util.StringUtils;
//import org.springframework.web.servlet.theme.AbstractThemeResolver;

//import javax.servlet.http.HttpRequest;
//import javax.servlet.http.HttpResponse;
//import java.util.*;
//import java.util.regex.Pattern;

/**
 * ThemeResolver to determine the theme for CAS based on the service provided.
 * The theme resolver will extract the service parameter from the Request object
 * and attempt to match the URL provided to a Service Id. If the service is
 * found, the theme associated with it will be used. If not, these is associated
 * with the service or the service was not found, a default theme will be used.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */
public  class ServiceThemeResolver : AbstractThemeResolver {

    /** The ServiceRegistry to look up the service. */
    private ServicesManager servicesManager;

    private List<ArgumentExtractor> argumentExtractors;

    private Map<Pattern,string> overrides = new HashMap<Pattern,string>();

    public string resolveThemeName( HttpRequest request) {
        if (this.servicesManager == null) {
            return getDefaultThemeName();
        }

         Service service = WebUtils.getService(this.argumentExtractors, request);

         RegisteredService rService = this.servicesManager.findServiceBy(service);

        // retrieve the user agent string from the request
        string userAgent = request.getHeader("User-Agent");

        if (userAgent == null) {
            return getDefaultThemeName();
        }

        for ( Map.Entry<Pattern,string> entry : this.overrides.entrySet()) {
            if (entry.getKey().matcher(userAgent).matches()) {
                request.setAttribute("isMobile","true");
                request.setAttribute("browserType", entry.getValue());
                break;
            }
        }

        return service != null && rService != null && StringUtils.hasText(rService.getTheme()) ? rService.getTheme() : getDefaultThemeName();
    }

    public void setThemeName( HttpRequest request,  HttpResponse response,  string themeName) {
        // nothing to do here
    }

    public void setServicesManager( ServicesManager servicesManager) {
        this.servicesManager = servicesManager;
    }

    public void setArgumentExtractors( List<ArgumentExtractor> argumentExtractors) {
        this.argumentExtractors = argumentExtractors;
    }

    /**
     * Sets the map of mobile browsers.  This sets a flag on the request called "isMobile" and also
     * provides the custom flag called browserType which can be mapped into the theme.
     * <p>
     * Themes that understand isMobile should provide an alternative stylesheet.
     *
     * @param mobileOverrides the list of mobile browsers.
     */
    public void setMobileBrowsers( Map<string,string> mobileOverrides) {
        // initialize the overrides variable to an empty map
        this.overrides = new HashMap<Pattern,string>();

        for ( Map.Entry<string,string> entry : mobileOverrides.entrySet()) {
            this.overrides.put(Pattern.compile(entry.getKey()), entry.getValue());
        }
    }
}
