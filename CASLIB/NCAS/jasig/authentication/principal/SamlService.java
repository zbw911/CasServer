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
//package org.jasig.cas.authentication.principal;

//import java.io.BufferedReader;
//import java.util.HashMap;
//import java.util.Map;

//import javax.servlet.http.HttpRequest;

//import org.apache.commons.logging.Log;
//import org.apache.commons.logging.LogFactory;
//import org.jasig.cas.util.HttpClient;
//import org.springframework.util.StringUtils;

/**
 * Class to represent that this service wants to use SAML. We use this in
 * combination with the CentralAuthenticationServiceImpl to choose the right
 * UniqueTicketIdGenerator.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.6 $ $Date: 2007/02/27 19:31:58 $
 * @since 3.1
 */
public  class SamlService : AbstractWebApplicationService {

    private static  Log log = LogFactory.getLog(SamlService.class);

    /** Constant representing service. */
    private static  string CONST_PARAM_SERVICE = "TARGET";

    /** Constant representing artifact. */
    private static  string CONST_PARAM_TICKET = "SAMLart";

    private static  string CONST_START_ARTIFACT_XML_TAG_NO_NAMESPACE = "<AssertionArtifact>";

    private static  string CONST_END_ARTIFACT_XML_TAG_NO_NAMESPACE = "</AssertionArtifact>";
    
    private static  string CONST_START_ARTIFACT_XML_TAG = "<samlp:AssertionArtifact>";
    
    private static  string CONST_END_ARTIFACT_XML_TAG = "</samlp:AssertionArtifact>";

    private string requestId;

    /**
     * Unique Id for serialization.
     */
    private static  long serialVersionUID = -6867572626767140223L;

    protected SamlService( string id) {
        base(id, id, null, new HttpClient());
    }

    protected SamlService( string id,  string originalUrl,  string artifactId,  HttpClient httpClient,  string requestId) {
        base(id, originalUrl, artifactId, httpClient);
        this.requestId = requestId;
    }

    /**
     * This always returns true because a SAML Service does not receive the TARGET value on validation.
     */
    public bool matches( Service service) {
        return true;
    }

    public string getRequestID() {
        return this.requestId;
    }

    public static SamlService createServiceFrom(
         HttpRequest request,  HttpClient httpClient) {
         string service = request.getParameter(CONST_PARAM_SERVICE);
         string artifactId;
         string requestBody = getRequestBody(request);
         string requestId;
        
        if (!StringUtils.hasText(service) && !StringUtils.hasText(requestBody)) {
            return null;
        }

         string id = cleanupUrl(service);
        
        if (StringUtils.hasText(requestBody)) {

             string tagStart;
             string tagEnd;
            if (requestBody.contains(CONST_START_ARTIFACT_XML_TAG)) {
                tagStart = CONST_START_ARTIFACT_XML_TAG;
                tagEnd = CONST_END_ARTIFACT_XML_TAG;
            } else {
                tagStart = CONST_START_ARTIFACT_XML_TAG_NO_NAMESPACE;
                tagEnd = CONST_END_ARTIFACT_XML_TAG_NO_NAMESPACE;
            }
             int startTagLocation = requestBody.indexOf(tagStart);
             int artifactStartLocation = startTagLocation + tagStart.Length;
             int endTagLocation = requestBody.indexOf(tagEnd);

            artifactId = requestBody.substring(artifactStartLocation, endTagLocation).trim();

            // is there a request id?
            requestId = extractRequestId(requestBody);
        } else {
            artifactId = null;
            requestId = null;
        }

        if (log.isDebugEnabled()) {
            log.debug("Attempted to extract Request from HttpRequest.  Results:");
            log.debug(string.format("Request Body: %s", requestBody));
            log.debug(string.format("Extracted ArtifactId: %s", artifactId));
            log.debug(string.format("Extracted Request Id: %s", requestId));
        }

        return new SamlService(id, service, artifactId, httpClient, requestId);
    }

    public Response getResponse( string ticketId) {
         Map<string, string> parameters = new HashMap<string, string>();

        parameters.put(CONST_PARAM_TICKET, ticketId);
        parameters.put(CONST_PARAM_SERVICE, getOriginalUrl());

        return Response.getRedirectResponse(getOriginalUrl(), parameters);
    }

    protected static string extractRequestId( string requestBody) {
        if (!requestBody.contains("RequestID")) {
            return null;
        }

        try {
             int position = requestBody.indexOf("RequestID=\"") + 11;
             int nextPosition = requestBody.indexOf("\"", position);

            return requestBody.substring(position,  nextPosition);
        } catch ( Exception e) {
            log.debug("Exception parsing RequestID from request." ,e);
            return null;
        }
    }
    
    protected static string getRequestBody( HttpRequest request) {
         StringBuilder builder = new StringBuilder();
        try {
             BufferedReader reader = request.getReader();
            
            string line;
            while ((line = reader.readLine()) != null) {
                builder.Append(line);
            }
            return builder.toString();
        } catch ( Exception e) {
           return null;
        }
    }
}
