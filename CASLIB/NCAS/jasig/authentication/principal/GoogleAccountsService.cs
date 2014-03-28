///*
// * Licensed to Jasig under one or more contributor license
// * agreements. See the NOTICE file distributed with this work
// * for additional information regarding copyright ownership.
// * Jasig licenses this file to you under the Apache License,
// * Version 2.0 (the "License"); you may not use this file
// * except in compliance with the License.  You may obtain a
// * copy of the License at the following location:
// *
// *   http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing,
// * software distributed under the License is distributed on an
// * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// * KIND, either express or implied.  See the License for the
// * specific language governing permissions and limitations
// * under the License.
// */
////package org.jasig.cas.authentication.principal;

////import org.jasig.cas.util.SamlUtils;
////import org.jdom.Document;
////import org.springframework.util.StringUtils;

////import javax.servlet.http.HttpRequest;
////import java.io.ByteArrayInputStream;
////import java.io.ByteArrayOutputStream;
////import java.io.UnsupportedEncodingException;
////import java.security.PrivateKey;
////import java.security.PublicKey;
////import java.util.Calendar;
////import java.util.Date;
////import java.util.HashMap;
////import java.util.Map;
////import java.util.Random;
////import java.util.zip.DataFormatException;
////import java.util.zip.Inflater;
////import java.util.zip.InflaterInputStream;

////import org.apache.commons.codec.binary.Base64;

///**
// * Implementation of a Service that supports Google Accounts (eventually a more
// * generic SAML2 support will come).
// * 
// * @author Scott Battaglia
// * @version $Revision: 1.1 $ $Date: 2005/08/19 18:27:17 $
// * @since 3.1
// */

//using System;
//using System.Web;
//using Dev.CasServer.jasig.util;
//using Dev.CasServer.principal;
//using NCAS.jasig.web.MOCK2JAVA;

//namespace NCAS.jasig.authentication.principal
//{
//    public class GoogleAccountsService : AbstractWebApplicationService {

//        /**
//     * Comment for <code>serialVersionUID</code>
//     */
//        private static  long serialVersionUID = 6678711809842282833L;

//        private static Random random = new Random();
    
//        private static  char[] charMapping = {
//                                                 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
//                                                 'p'};

//        private static  string CONST_PARAM_SERVICE = "SAMLRequest";

//        private static  string CONST_RELAY_STATE = "RelayState";

//        private static  string TEMPLATE_SAML_RESPONSE = "<samlp:Response ID=\"<RESPONSE_ID>\" IssueInstant=\"<ISSUE_INSTANT>\" Version=\"2.0\""
//                                                        + " xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\""
//                                                        + " xmlns:samlp=\"urn:oasis:names:tc:SAML:2.0:protocol\""
//                                                        + " xmlns:xenc=\"http://www.w3.org/2001/04/xmlenc#\">"
//                                                        + "<samlp:Status>"
//                                                        + "<samlp:StatusCode Value=\"urn:oasis:names:tc:SAML:2.0:status:Success\" />"
//                                                        + "</samlp:Status>"
//                                                        + "<Assertion ID=\"<ASSERTION_ID>\""
//                                                        + " IssueInstant=\"2003-04-17T00:46:02Z\" Version=\"2.0\""
//                                                        + " xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\">"
//                                                        + "<Issuer>https://www.opensaml.org/IDP</Issuer>"
//                                                        + "<Subject>"
//                                                        + "<NameID Format=\"urn:oasis:names:tc:SAML:2.0:nameid-format:emailAddress\">"
//                                                        + "<USERNAME_STRING>"
//                                                        + "</NameID>"
//                                                        + "<SubjectConfirmation Method=\"urn:oasis:names:tc:SAML:2.0:cm:bearer\">"
//                                                        + "<SubjectConfirmationData Recipient=\"<ACS_URL>\" NotOnOrAfter=\"<NOT_ON_OR_AFTER>\" InResponseTo=\"<REQUEST_ID>\" />"
//                                                        + "</SubjectConfirmation>"
//                                                        + "</Subject>"
//                                                        + "<Conditions NotBefore=\"2003-04-17T00:46:02Z\""
//                                                        + " NotOnOrAfter=\"<NOT_ON_OR_AFTER>\">"
//                                                        + "<AudienceRestriction>"
//                                                        + "<Audience><ACS_URL></Audience>"
//                                                        + "</AudienceRestriction>"
//                                                        + "</Conditions>"
//                                                        + "<AuthnStatement AuthnInstant=\"<AUTHN_INSTANT>\">"
//                                                        + "<AuthnContext>"
//                                                        + "<AuthnContextClassRef>"
//                                                        + "urn:oasis:names:tc:SAML:2.0:ac:classes:Password"
//                                                        + "</AuthnContextClassRef>"
//                                                        + "</AuthnContext>"
//                                                        + "</AuthnStatement>"
//                                                        + "</Assertion></samlp:Response>";

//        private  string relayState;

//        private  PublicKey publicKey;

//        private  PrivateKey privateKey;
    
//        private  string requestId;

//        private  string alternateUserName;

//        protected GoogleAccountsService( string id,  string relayState,  string requestId,
//                                         PrivateKey privateKey,  PublicKey publicKey,  string alternateUserName) {
//            this(id, id, null, relayState, requestId, privateKey, publicKey, alternateUserName);
//                                         }

//        protected GoogleAccountsService( string id,  string originalUrl,
//                                         string artifactId,  string relayState,  string requestId,
//                                         PrivateKey privateKey,  PublicKey publicKey,  string alternateUserName) {
//            base(id, originalUrl, artifactId, null);
//            this.relayState = relayState;
//            this.privateKey = privateKey;
//            this.publicKey = publicKey;
//            this.requestId = requestId;
//            this.alternateUserName = alternateUserName;
//                                         }

//        public static GoogleAccountsService createServiceFrom(
//            HttpRequest request,  PrivateKey privateKey,
//            PublicKey publicKey,  string alternateUserName) {
//            string relayState = request.getParameter(CONST_RELAY_STATE);

//            string xmlRequest = decodeAuthnRequestXML(request
//                                                          .getParameter(CONST_PARAM_SERVICE));

//            if (!StringUtils.hasText(xmlRequest)) {
//                return null;
//            }

//            Document document = SamlUtils
//                .constructDocumentFromXmlString(xmlRequest);

//            if (document == null) {
//                return null;
//            }

//            string assertionConsumerServiceUrl = document.getRootElement().getAttributeValue("AssertionConsumerServiceURL");
//            string requestId = document.getRootElement().getAttributeValue("ID");

//            return new GoogleAccountsService(assertionConsumerServiceUrl,
//                                             relayState, requestId, privateKey, publicKey, alternateUserName);
//            }

//        public Response getResponse( string ticketId) {
//            Map<string, string> parameters = new HashMap<string, string>();
//            string samlResponse = this.constructSamlResponse();
//            string signedResponse = SamlUtils.signSamlResponse(samlResponse,
//                                                               this.privateKey, this.publicKey);
//            parameters.put("SAMLResponse", signedResponse);
//            parameters.put("RelayState", this.relayState);

//            return Response.getPostResponse(this.getOriginalUrl(), parameters);
//        }

//        /**
//     * Service does not support Single Log Out
//     * 
//     * @see org.jasig.cas.authentication.principal.WebApplicationService#logOutOfService(java.lang.string)
//     */
//        public bool logOutOfService( string sessionIdentifier) {
//            return false;
//        }

//        private string constructSamlResponse() {
//            string samlResponse = TEMPLATE_SAML_RESPONSE;

//            Calendar c = Calendar.getInstance();
//            c.setTime(new Date());
//            c.add(Calendar.YEAR, 1);

//            string userId;

//            if (this.alternateUserName == null) {
//                userId = this.getPrincipal().getId();
//            } else {
//                string attributeValue = (string) this.getPrincipal().getAttributes().get(this.alternateUserName);
//                if (attributeValue == null) {
//                    userId = this.getPrincipal().getId();
//                } else {
//                    userId = attributeValue;
//                }
//            }
        
//            samlResponse = samlResponse.replace("<USERNAME_STRING>", userId);
//            samlResponse = samlResponse.replace("<RESPONSE_ID>", createID());
//            samlResponse = samlResponse.replace("<ISSUE_INSTANT>", SamlUtils
//                                                                       .getCurrentDateAndTime());
//            samlResponse = samlResponse.replace("<AUTHN_INSTANT>", SamlUtils
//                                                                       .getCurrentDateAndTime());
//            samlResponse = samlResponse.replaceAll("<NOT_ON_OR_AFTER>", SamlUtils
//                                                                            .getFormattedDateAndTime(c.getTime()));
//            samlResponse = samlResponse.replace("<ASSERTION_ID>", createID());
//            samlResponse = samlResponse.replaceAll("<ACS_URL>", this.getId());
//            samlResponse = samlResponse.replace("<REQUEST_ID>", this.requestId);

//            return samlResponse;
//        }
    
//        private static string createID() {
//            byte[] bytes = new byte[20]; // 160 bits
//            random.nextBytes(bytes);

//            char[] chars = new char[40];

//            for (int i = 0; i < bytes.length; i++) {
//                int left = (bytes[i] >> 4) & 0x0f;
//                int right = bytes[i] & 0x0f;
//                chars[i * 2] = charMapping[left];
//                chars[i * 2 + 1] = charMapping[right];
//            }

//            return string.valueOf(chars);
//        }

//        private static string decodeAuthnRequestXML(
//            string encodedRequestXmlString) {
//            if (encodedRequestXmlString == null) {
//                return null;
//            }

//            byte[] decodedBytes = base64Decode(encodedRequestXmlString);

//            if (decodedBytes == null) {
//                return null;
//            }

//            string inflated = inflate(decodedBytes);

//            if (inflated != null) {
//                return inflated;
//            }

//            return zlibDeflate(decodedBytes);
//            }

//        private static string zlibDeflate( byte[] bytes) {
//            ByteArrayInputStream bais = new ByteArrayInputStream(bytes);
//            ByteArrayOutputStream baos = new ByteArrayOutputStream();
//            InflaterInputStream iis = new InflaterInputStream(bais);
//            byte[] buf = new byte[1024];

//            try {
//                int count = iis.read(buf);
//                while (count != -1) {
//                    baos.write(buf, 0, count);
//                    count = iis.read(buf);
//                }
//                return new string(baos.toByteArray());
//            } catch ( Exception e) {
//                return null;
//            } ly {
//                try {
//                    iis.close();
//                } catch ( Exception e) {
//                    // nothing to do
//                }
//            }
//        }

//        private static byte[] base64Decode( string xml) {
//            try {
//                byte[] xmlBytes = xml.getBytes("UTF-8");
//                return Base64.decodeBase64(xmlBytes);
//            } catch ( Exception e) {
//                return null;
//            }
//        }

//        private static string inflate( byte[] bytes) {
//            Inflater inflater = new Inflater(true);
//            byte[] xmlMessageBytes = new byte[10000];
        
//            byte[] extendedBytes = new byte[bytes.length + 1];
//            System.arraycopy(bytes, 0, extendedBytes, 0, bytes.length);
//            extendedBytes[bytes.length] = 0;
        
//            inflater.setInput(extendedBytes);

//            try {
//                int resultLength = inflater.inflate(xmlMessageBytes);
//                inflater.end();

//                if (!inflater.finished()) {
//                    throw new RuntimeException("buffer not large enough.");
//                }

//                inflater.end();
//                return new string(xmlMessageBytes, 0, resultLength, "UTF-8");
//            } catch ( DataFormatException e) {
//                return null;
//            } catch ( UnsupportedEncodingException e) {
//                throw new RuntimeException("Cannot find encoding: UTF-8", e);
//            }
//        }
//    }
//}
