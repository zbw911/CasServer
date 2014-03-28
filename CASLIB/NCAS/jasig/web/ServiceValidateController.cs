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

//import java.net.URL;
//import javax.servlet.http.HttpRequest;
//import javax.servlet.http.HttpResponse;
//import javax.validation.constraints.NotNull;

//import org.jasig.cas.CentralAuthenticationService;
//import org.jasig.cas.authentication.principal.Credentials;
//import org.jasig.cas.authentication.principal.HttpBasedServiceCredentials;
//import org.jasig.cas.authentication.principal.WebApplicationService;
//import org.jasig.cas.services.UnauthorizedServiceException;
//import org.jasig.cas.ticket.TicketException;
//import org.jasig.cas.ticket.TicketValidationException;
//import org.jasig.cas.ticket.proxy.ProxyHandler;
//import org.jasig.cas.validation.Assertion;
//import org.jasig.cas.validation.ValidationSpecification;
//import org.jasig.cas.validation.Cas20ProtocolValidationSpecification;
//import org.jasig.cas.web.support.ArgumentExtractor;
//import org.springframework.util.StringUtils;
//import org.springframework.web.bind.ServletRequestDataBinder;
//import org.springframework.web.servlet.ModelAndView;
//import org.springframework.web.servlet.mvc.AbstractController;

using System.Web;
 
using NCAS.jasig.authentication.principal;
using NCAS.jasig.services;
using NCAS.jasig.ticket;
using NCAS.jasig.ticket.proxy;
using NCAS.jasig.validation;
using NCAS.jasig.web.MOCK2JAVA;
using NCAS.jasig.web.support;
using System;
/**
 * Process the /validate and /serviceValidate URL requests.
 * <p>
 * Obtain the Service Ticket and Service information and present them to the CAS
 * validation services. Receive back an Assertion containing the user Principal
 * and (possibly) a chain of Proxy Principals. Store the Assertion in the Model
 * and chain to a View to generate the appropriate response (CAS 1, CAS 2 XML,
 * SAML, ...).
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

namespace NCAS.jasig.web
{
    public class ServiceValidateController : DelegateController
    {

        /** View if Service Ticket Validation Fails. */
        private static string DEFAULT_SERVICE_FAILURE_VIEW_NAME = "casServiceFailureView";

        /** View if Service Ticket Validation Succeeds. */
        private static string DEFAULT_SERVICE_SUCCESS_VIEW_NAME = "casServiceSuccessView";

        /** Constant representing the PGTIOU in the model. */
        private static string MODEL_PROXY_GRANTING_TICKET_IOU = "pgtIou";

        /** Constant representing the Assertion in the model. */
        private static string MODEL_ASSERTION = "assertion";

        /** The CORE which we will delegate all requests to. */
        ////@NotNull
        private CentralAuthenticationService centralAuthenticationService;

        /** The validation protocol we want to use. */
        ////@NotNull
        private Type validationSpecificationClass = typeof(Cas20ProtocolValidationSpecification);

        /** The proxy handler we want to use with the controller. */
        ////@NotNull
        private ProxyHandler proxyHandler;

        /** The view to redirect to on a successful validation. */
        ////@NotNull
        private string successView = DEFAULT_SERVICE_SUCCESS_VIEW_NAME;

        /** The view to redirect to on a validation failure. */
        ////@NotNull
        private string failureView = DEFAULT_SERVICE_FAILURE_VIEW_NAME;

        /** Extracts parameters from Request object. */
        ////@NotNull
        private ArgumentExtractor argumentExtractor;

        /**
     * Overrideable method to determine which credentials to use to grant a
     * proxy granting ticket. Default is to use the pgtUrl.
     * 
     * @param request the HttpRequest object.
     * @return the credentials or null if there was an error or no credentials
     * provided.
     */
        protected Credentials getServiceCredentialsFromRequest(HttpRequest request)
        {
            string pgtUrl = request.getParameter("pgtUrl");
            if (StringUtils.hasText(pgtUrl))
            {
                try
                {
                    return new HttpBasedServiceCredentials(new Uri(pgtUrl));
                }
                catch (Exception e)
                {
                    //logger.error("Error constructing pgtUrl", e);
                }
            }

            return null;
        }

        protected void initBinder(HttpRequest request, ServletRequestDataBinder binder)
        {
            binder.setRequiredFields("renew");
        }

        protected ModelAndView handleRequestInternal(HttpRequest request, HttpResponse response)
        {
            WebApplicationService service = this.argumentExtractor.extractService(request);
            string serviceTicketId = service != null ? service.getArtifactId() : null;

            if (service == null || serviceTicketId == null)
            {
                //if (logger.isDebugEnabled()) {
                //    logger.debug(string.format("Could not process request; Service: %s, Service Ticket Id: %s", service, serviceTicketId));
                //}
                return this.generateErrorView("INVALID_REQUEST", "INVALID_REQUEST", null);
            }

            try
            {
                Credentials serviceCredentials = this.getServiceCredentialsFromRequest(request);
                string proxyGrantingTicketId = null;

                // XXX should be able to validate AND THEN use
                if (serviceCredentials != null)
                {
                    try
                    {
                        proxyGrantingTicketId = this.centralAuthenticationService
                            .delegateTicketGrantingTicket(serviceTicketId,
                                                          serviceCredentials);
                    }
                    catch (TicketException e)
                    {
                        //logger.error("TicketException generating ticket for: "
                        //    + serviceCredentials, e);
                    }
                }

                Assertion assertion = this.centralAuthenticationService.validateServiceTicket(serviceTicketId, service);

                ValidationSpecification validationSpecification = this.getCommandClass();
                ServletRequestDataBinder binder = new ServletRequestDataBinder(validationSpecification, "validationSpecification");
                this.initBinder(request, binder);
                binder.bind(request);

                if (!validationSpecification.isSatisfiedBy(assertion))
                {
                    //if (logger.isDebugEnabled()) {
                    //    logger.debug("ServiceTicket [" + serviceTicketId + "] does not satisfy validation specification.");
                    //}
                    return this.generateErrorView("INVALID_TICKET", "INVALID_TICKET_SPEC", null);
                }

                this.onSuccessfulValidation(serviceTicketId, assertion);

                ModelAndView success = new ModelAndView(this.successView);
                success.addObject(MODEL_ASSERTION, assertion);

                if (serviceCredentials != null && proxyGrantingTicketId != null)
                {
                    string proxyIou = this.proxyHandler.handle(serviceCredentials, proxyGrantingTicketId);
                    success.addObject(MODEL_PROXY_GRANTING_TICKET_IOU, proxyIou);
                }

                //if (logger.isDebugEnabled()) {
                //    logger.debug(string.format("Successfully validated service ticket: %s", serviceTicketId));
                //}

                return success;
            }
            catch (TicketValidationException e)
            {
                return this.generateErrorView(e.getCode(), e.getCode(), new Object[] { serviceTicketId, e.getOriginalService().getId(), service.getId() });
            }
            catch (TicketException te)
            {
                return this.generateErrorView(te.getCode(), te.getCode(),
                                         new Object[] { serviceTicketId });
            }
            catch (UnauthorizedServiceException e)
            {
                //return generateErrorView(e.getMessage(), e.getMessage(), null);
                throw new NotImplementedException();
            }
        }

        protected void onSuccessfulValidation(string serviceTicketId, Assertion assertion)
        {
            // template method with nothing to do.
        }

        private ModelAndView generateErrorView(string code, string description, Object[] args)
        {
            ModelAndView modelAndView = new ModelAndView(this.failureView);
            //string convertedDescription = getMessageSourceAccessor().getMessage(description, args, description);
            modelAndView.addObject("code", code);
            //modelAndView.addObject("description", convertedDescription);

            return modelAndView;
        }

        private ValidationSpecification getCommandClass()
        {
            try
            {
                return (ValidationSpecification)Activator.CreateInstance(this.validationSpecificationClass);
            }
            catch (Exception e)
            {
                throw new NotSupportedException("", e);
            }
        }

        /**
     * {@inheritDoc}
     */
        //@Override
        public override bool canHandle(HttpRequest request, HttpResponse response)
        {
            return true;
        }

        /**
     * @param centralAuthenticationService The centralAuthenticationService to
     * set.
     */
        public void setCentralAuthenticationService(CentralAuthenticationService centralAuthenticationService)
        {
            this.centralAuthenticationService = centralAuthenticationService;
        }

        public void setArgumentExtractor(ArgumentExtractor argumentExtractor)
        {
            this.argumentExtractor = argumentExtractor;
        }

        /**
     * @param validationSpecificationClass The authenticationSpecificationClass
     * to set.
     */
        public void setValidationSpecificationClass(Type validationSpecificationClass)
        {
            this.validationSpecificationClass = validationSpecificationClass;
        }

        /**
     * @param failureView The failureView to set.
     */
        public void setFailureView(string failureView)
        {
            this.failureView = failureView;
        }

        /**
     * @param successView The successView to set.
     */
        public void setSuccessView(string successView)
        {
            this.successView = successView;
        }

        /**
     * @param proxyHandler The proxyHandler to set.
     */
        public void setProxyHandler(ProxyHandler proxyHandler)
        {
            this.proxyHandler = proxyHandler;
        }

    }
}
