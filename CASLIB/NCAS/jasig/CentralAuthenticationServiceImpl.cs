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
//package org.jasig.cas;

//import com.github.inspektr.audit.annotation.Audit;

//import org.apache.commons.lang.StringUtils;
//import org.jasig.cas.authentication.Authentication;
//import org.jasig.cas.authentication.AuthenticationManager;
//import org.jasig.cas.authentication.MutableAuthentication;
//import org.jasig.cas.authentication.handler.AuthenticationException;
//import org.jasig.cas.authentication.principal.Credentials;
//import org.jasig.cas.authentication.principal.PersistentIdGenerator;
//import org.jasig.cas.authentication.principal.Principal;
//import org.jasig.cas.authentication.principal.Service;
//import org.jasig.cas.authentication.principal.ShibbolethCompatiblePersistentIdGenerator;
//import org.jasig.cas.authentication.principal.SimplePrincipal;
//import org.jasig.cas.services.RegisteredService;
//import org.jasig.cas.services.ServicesManager;
//import org.jasig.cas.services.UnauthorizedProxyingException;
//import org.jasig.cas.services.UnauthorizedServiceException;
//import org.jasig.cas.services.UnauthorizedSsoServiceException;
//import org.jasig.cas.ticket.ExpirationPolicy;
//import org.jasig.cas.ticket.InvalidTicketException;
//import org.jasig.cas.ticket.ServiceTicket;
//import org.jasig.cas.ticket.TicketCreationException;
//import org.jasig.cas.ticket.TicketException;
//import org.jasig.cas.ticket.TicketGrantingTicket;
//import org.jasig.cas.ticket.TicketGrantingTicketImpl;
//import org.jasig.cas.ticket.TicketValidationException;
//import org.jasig.cas.ticket.registry.TicketRegistry;
//import org.jasig.cas.util.UniqueTicketIdGenerator;
//import org.jasig.cas.validation.Assertion;
//import org.jasig.cas.validation.ImmutableAssertionImpl;
//import org.perf4j.aop.Profiled;
//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;
//import org.springframework.transaction.annotation.Transactional;
//import org.springframework.util.Assert;

//import javax.validation.constraints.NotNull;
//import java.util.ArrayList;
//import java.util.HashMap;
//import java.util.List;
//import java.util.Map;

/**
 * Concrete implementation of a CentralAuthenticationService, and also the
 * central, organizing component of CAS's internal implementation.
 * <p>
 * This class is threadsafe.
 * <p>
 * This class has the following properties that must be set:
 * <ul>
 * <li> <code>ticketRegistry</code> - The Ticket Registry to maintain the list
 * of available tickets.</li>
 * <li> <code>serviceTicketRegistry</code> - Provides an alternative to configure separate registries for TGTs and ST in order to store them
 * in different locations (i.e. long term memory or short-term)</li>
 * <li> <code>authenticationManager</code> - The service that will handle
 * authentication.</li>
 * <li> <code>ticketGrantingTicketUniqueTicketIdGenerator</code> - Plug in to
 * generate unique secure ids for TicketGrantingTickets.</li>
 * <li> <code>serviceTicketUniqueTicketIdGenerator</code> - Plug in to
 * generate unique secure ids for ServiceTickets.</li>
 * <li> <code>ticketGrantingTicketExpirationPolicy</code> - The expiration
 * policy for TicketGrantingTickets.</li>
 * <li> <code>serviceTicketExpirationPolicy</code> - The expiration policy for
 * ServiceTickets.</li>
 * </ul>
 *
 * @author William G. Thompson, Jr.
 * @author Scott Battaglia
 * @author Dmitry Kopylenko
 * @version $Revision: 1.16 $ $Date: 2007/04/24 18:11:36 $
 * @since 3.0
 */

using System;
using System.Collections.Generic;
using System.Linq;

using NCAS.jasig.authentication;
using NCAS.jasig.authentication.handler;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.services;
using NCAS.jasig.ticket;
using NCAS.jasig.ticket.registry;
using NCAS.jasig.util;
using NCAS.jasig.validation;

namespace NCAS.jasig
{
    public class CentralAuthenticationServiceImpl : CentralAuthenticationService
    {

        /** Log instance for logging events, info, warnings, errors, etc. */
        //private  Logger log = LoggerFactory.getLogger(this.getClass());

        /** TicketRegistry for storing and retrieving tickets as needed. */
        //// //@NotNull
        private TicketRegistry ticketRegistry;

        /** New Ticket Registry for storing and retrieving services tickets. Can point to the same one as the ticketRegistry variable. */
        // //@NotNull
        private TicketRegistry serviceTicketRegistry;

        /**
     * AuthenticationManager for authenticating credentials for purposes of
     * obtaining tickets.
     */
        // //@NotNull
        private AuthenticationManager authenticationManager;

        /**
     * UniqueTicketIdGenerator to generate ids for TicketGrantingTickets
     * created.
     */
        // //@NotNull
        private UniqueTicketIdGenerator ticketGrantingTicketUniqueTicketIdGenerator;

        /** Map to contain the mappings of service->UniqueTicketIdGenerators */
        // //@NotNull
        private Dictionary<string, UniqueTicketIdGenerator> uniqueTicketIdGeneratorsForService;

        /** Expiration policy for ticket granting tickets. */
        // //@NotNull
        private ExpirationPolicy ticketGrantingTicketExpirationPolicy;

        /** ExpirationPolicy for Service Tickets. */
        // //@NotNull
        private ExpirationPolicy serviceTicketExpirationPolicy;

        /** Implementation of Service Manager */
        // //@NotNull
        private ServicesManager servicesManager;

        /** Encoder to generate PseudoIds. */
        // //@NotNull
        private PersistentIdGenerator persistentIdGenerator = new ShibbolethCompatiblePersistentIdGenerator();


        //public CentralAuthenticationServiceImpl(AuthenticationManager authenticationManager,
        //                                        TicketRegistry ticketRegistry,
        //                                        TicketRegistry serviceTicketRegistry,
        //                                        UniqueTicketIdGenerator ticketGrantingTicketUniqueTicketIdGenerator,
        //                                        ExpirationPolicy ticketGrantingTicketExpirationPolicy,
        //                                        ExpirationPolicy serviceTicketExpirationPolicy,
        //                                        ServicesManager servicesManager)
        //    : this(authenticationManager, ticketRegistry, serviceTicketRegistry, ticketGrantingTicketUniqueTicketIdGenerator, null, ticketGrantingTicketExpirationPolicy, serviceTicketExpirationPolicy, servicesManager)
        //{
        //}

        public CentralAuthenticationServiceImpl(AuthenticationManager authenticationManager,
            TicketRegistry ticketRegistry,
            TicketRegistry serviceTicketRegistry,
            UniqueTicketIdGenerator ticketGrantingTicketUniqueTicketIdGenerator,
            Dictionary<string, UniqueTicketIdGenerator> uniqueTicketIdGeneratorsForService,
            ExpirationPolicy ticketGrantingTicketExpirationPolicy,
            ExpirationPolicy serviceTicketExpirationPolicy,
            ServicesManager servicesManager)
        {
            this.authenticationManager = authenticationManager;
            this.ticketRegistry = ticketRegistry;
            this.serviceTicketRegistry = serviceTicketRegistry;
            this.ticketGrantingTicketUniqueTicketIdGenerator = ticketGrantingTicketUniqueTicketIdGenerator;
            this.uniqueTicketIdGeneratorsForService = uniqueTicketIdGeneratorsForService;
            this.ticketGrantingTicketExpirationPolicy = ticketGrantingTicketExpirationPolicy;
            this.serviceTicketExpirationPolicy = serviceTicketExpirationPolicy;
            this.servicesManager = servicesManager;
        }


        /**
     * Implementation of destoryTicketGrantingTicket expires the ticket provided
     * and removes it from the TicketRegistry.
     *
     * @throws IllegalArgumentException if the TicketGrantingTicket ID is null.
     */
        //@Audit(
        //    action="TICKET_GRANTING_TICKET_DESTROYED",
        //    actionResolverName="DESTROY_TICKET_GRANTING_TICKET_RESOLVER",
        //    resourceResolverName="DESTROY_TICKET_GRANTING_TICKET_RESOURCE_RESOLVER")
        //@Profiled(tag = "DESTROY_TICKET_GRANTING_TICKET",logFailuresSeparately = false)
        //@Transactional(readOnly = false)
        public void destroyTicketGrantingTicket(string ticketGrantingTicketId)
        {
            //Assert.notNull(ticketGrantingTicketId);

            //if (log.isDebugEnabled()) {
            //    log.debug("Removing ticket [" + ticketGrantingTicketId + "] from registry.");
            //}
            TicketGrantingTicket ticket = (TicketGrantingTicket)this.ticketRegistry.getTicket(ticketGrantingTicketId, typeof(TicketGrantingTicket));

            if (ticket == null)
            {
                return;
            }

            //if (log.isDebugEnabled()) {
            //    log.debug("Ticket found.  Expiring and then deleting.");
            //}
            ticket.expire();
            this.ticketRegistry.deleteTicket(ticketGrantingTicketId);
        }

        /**
     * @throws IllegalArgumentException if TicketGrantingTicket ID, Credentials
     * or Service are null.
     */
        //@Audit(
        //    action="SERVICE_TICKET",
        //    actionResolverName="GRANT_SERVICE_TICKET_RESOLVER",
        //    resourceResolverName="GRANT_SERVICE_TICKET_RESOURCE_RESOLVER")
        //@Profiled(tag="GRANT_SERVICE_TICKET", logFailuresSeparately = false)
        //@Transactional(readOnly = false)
        public string grantServiceTicket(string ticketGrantingTicketId, Service service, Credentials credentials)
        {

            //Assert.notNull(ticketGrantingTicketId, "ticketGrantingticketId cannot be null");
            //Assert.notNull(service, "service cannot be null");

            TicketGrantingTicket ticketGrantingTicket;
            ticketGrantingTicket = (TicketGrantingTicket)this.ticketRegistry.getTicket(ticketGrantingTicketId, typeof(TicketGrantingTicket));

            if (ticketGrantingTicket == null)
            {
                throw new InvalidTicketException();
            }

            lock (ticketGrantingTicket)
            {
                if (ticketGrantingTicket.isExpired())
                {
                    this.ticketRegistry.deleteTicket(ticketGrantingTicketId);
                    throw new InvalidTicketException();
                }
            }

            RegisteredService registeredService = this.servicesManager
                .findServiceBy(service);

            if (registeredService == null || !registeredService.isEnabled())
            {
                //log.warn("ServiceManagement: Unauthorized Service Access. Service [" + service.getId() + "] not found in Service Registry.");
                throw new UnauthorizedServiceException();
            }

            if (!registeredService.isSsoEnabled() && credentials == null
                && ticketGrantingTicket.getCountOfUses() > 0)
            {
                //log.warn("ServiceManagement: Service Not Allowed to use SSO.  Service [" + service.getId() + "]");
                throw new UnauthorizedSsoServiceException();
            }

            //CAS-1019
            List<Authentication> authns = ticketGrantingTicket.getChainedAuthentications();
            if (authns.Count > 1)
            {
                if (!registeredService.isAllowedToProxy())
                {
                    string message = string.Format("ServiceManagement: Service Attempted to Proxy, but is not allowed. Service: [%s] | Registered Service: [%s]", service.getId(), registeredService.ToString());
                    //log.warn(message);
                    throw new UnauthorizedProxyingException(message);
                }
            }

            if (credentials != null)
            {
                try
                {
                    Authentication authentication = this.authenticationManager
                        .authenticate(credentials);
                    Authentication originalAuthentication = ticketGrantingTicket.getAuthentication();

                    if (!(authentication.getPrincipal().Equals(originalAuthentication.getPrincipal()) && authentication.getAttributes().Equals(originalAuthentication.getAttributes())))
                    {
                        throw new TicketCreationException();
                    }
                }
                catch (AuthenticationException e)
                {
                    throw new TicketCreationException(e);
                }
            }

            // this code is a bit brittle by depending on the class name.  Future versions (i.e. CAS4 will know inherently how to identify themselves)
            UniqueTicketIdGenerator serviceTicketUniqueTicketIdGenerator = this.uniqueTicketIdGeneratorsForService
                .FirstOrDefault(x => x.Key == service.GetType().FullName).Value;

            ServiceTicket serviceTicket = ticketGrantingTicket
                .grantServiceTicket(serviceTicketUniqueTicketIdGenerator
                                        .getNewTicketId(TicketPrefix.ServiceTicket_PREFIX), service,
                                    this.serviceTicketExpirationPolicy, credentials != null);

            this.serviceTicketRegistry.addTicket(serviceTicket);

            //if (log.isInfoEnabled()) {
            //     List<Authentication> authentications = serviceTicket.getGrantingTicket().getChainedAuthentications();
            //     string formatString = "Granted %s ticket [%s] for service [%s] for user [%s]";
            //     string type;
            //     string principalId = authentications.get(authentications.size()-1).getPrincipal().getId();

            //    if (authentications.size() == 1) {
            //        type = "service";

            //    } else {
            //        type = "proxy";
            //    }

            //    log.info(string.format(formatString, type, serviceTicket.getId(), service.getId(), principalId));
            //}

            return serviceTicket.getId();
        }

        //@Audit(
        //    action="SERVICE_TICKET",
        //    actionResolverName="GRANT_SERVICE_TICKET_RESOLVER",
        //    resourceResolverName="GRANT_SERVICE_TICKET_RESOURCE_RESOLVER")
        //@Profiled(tag = "GRANT_SERVICE_TICKET",logFailuresSeparately = false)
        //@Transactional(readOnly = false)
        public string grantServiceTicket(string ticketGrantingTicketId,
                                          Service service)
        {
            return this.grantServiceTicket(ticketGrantingTicketId, service, null);
        }

        /**
     * @throws IllegalArgumentException if the ServiceTicketId or the
     * Credentials are null.
     */
        //@Audit(
        //    action="PROXY_GRANTING_TICKET",
        //    actionResolverName="GRANT_PROXY_GRANTING_TICKET_RESOLVER",
        //    resourceResolverName="GRANT_PROXY_GRANTING_TICKET_RESOURCE_RESOLVER")
        //@Profiled(tag="GRANT_PROXY_GRANTING_TICKET",logFailuresSeparately = false)
        //@Transactional(readOnly = false)
        public string delegateTicketGrantingTicket(string serviceTicketId,
                                                    Credentials credentials)
        {

            //Assert.notNull(serviceTicketId, "serviceTicketId cannot be null");
            //Assert.notNull(credentials, "credentials cannot be null");

            try
            {
                Authentication authentication = this.authenticationManager
                    .authenticate(credentials);

                ServiceTicket serviceTicket;
                serviceTicket = (ServiceTicket)this.serviceTicketRegistry.getTicket(serviceTicketId, typeof(ServiceTicket));

                if (serviceTicket == null || serviceTicket.isExpired())
                {
                    throw new InvalidTicketException();
                }

                RegisteredService registeredService = this.servicesManager
                    .findServiceBy(serviceTicket.getService());

                if (registeredService == null || !registeredService.isEnabled()
                    || !registeredService.isAllowedToProxy())
                {
                    //log.warn("ServiceManagement: Service Attempted to Proxy, but is not allowed.  Service: [" + serviceTicket.getService().getId() + "]");
                    throw new UnauthorizedProxyingException();
                }

                TicketGrantingTicket ticketGrantingTicket = serviceTicket
                    .grantTicketGrantingTicket(
                        this.ticketGrantingTicketUniqueTicketIdGenerator
                            .getNewTicketId(TicketPrefix.TicketGrantingTicket_PREFIX),
                        authentication, this.ticketGrantingTicketExpirationPolicy);

                this.ticketRegistry.addTicket(ticketGrantingTicket);

                return ticketGrantingTicket.getId();
            }
            catch (AuthenticationException e)
            {
                throw new TicketCreationException(e);
            }
        }

        /**
     * @throws IllegalArgumentException if the ServiceTicketId or the Service
     * are null.
     */
        //@Audit(
        //    action="SERVICE_TICKET_VALIDATE",
        //    actionResolverName="VALIDATE_SERVICE_TICKET_RESOLVER",
        //    resourceResolverName="VALIDATE_SERVICE_TICKET_RESOURCE_RESOLVER")
        //@Profiled(tag="VALIDATE_SERVICE_TICKET",logFailuresSeparately = false)
        //@Transactional(readOnly = false)
        public Assertion validateServiceTicket(string serviceTicketId, Service service)
        {
            //Assert.notNull(serviceTicketId, "serviceTicketId cannot be null");
            //Assert.notNull(service, "service cannot be null");

            ServiceTicket serviceTicket = (ServiceTicket)this.serviceTicketRegistry.getTicket(serviceTicketId, typeof(ServiceTicket));

            RegisteredService registeredService = this.servicesManager.findServiceBy(service);

            if (registeredService == null || !registeredService.isEnabled())
            {
                //log.warn("ServiceManagement: Service does not exist is not enabled, and thus not allowed to validate tickets.   Service: [" + service.getId() + "]");
                throw new UnauthorizedServiceException("Service not allowed to validate tickets.");
            }

            if (serviceTicket == null)
            {
                //log.info("ServiceTicket [" + serviceTicketId + "] does not exist.");
                throw new InvalidTicketException();
            }

            try
            {
                lock (serviceTicket)
                {
                    if (serviceTicket.isExpired())
                    {
                        //log.info("ServiceTicket [" + serviceTicketId + "] has expired.");
                        throw new InvalidTicketException();
                    }

                    if (!serviceTicket.isValidFor(service))
                    {
                        //log.error("ServiceTicket [" + serviceTicketId + "] with service [" + serviceTicket.getService().getId() + " does not match supplied service [" + service + "]");
                        throw new TicketValidationException(serviceTicket.getService());
                    }
                }

                List<Authentication> chainedAuthenticationsList = serviceTicket.getGrantingTicket().getChainedAuthentications();
                Authentication authentication = chainedAuthenticationsList.ElementAt(chainedAuthenticationsList.Count() - 1);
                Principal principal = authentication.getPrincipal();

                string principalId = this.determinePrincipalIdForRegisteredService(principal, registeredService, serviceTicket);
                Authentication authToUse;

                if (!registeredService.isIgnoreAttributes())
                {
                    Dictionary<string, Object> attributes = new Dictionary<string, Object>();

                    foreach (string attribute in registeredService.getAllowedAttributes())
                    {
                        Object value = principal.getAttributes().FirstOrDefault(x => x.Key == attribute).Value;

                        if (value != null)
                        {
                            attributes.Add(attribute, value);
                        }
                    }

                    Principal modifiedPrincipal = new SimplePrincipal(principalId, attributes);
                    MutableAuthentication mutableAuthentication = new MutableAuthentication(modifiedPrincipal, authentication.getAuthenticatedDate());

                    var mutableAuthenticationattributes = mutableAuthentication.getAttributes();

                    var U = mutableAuthentication.getAttributes().Concat(authentication.getAttributes());



                    mutableAuthentication.Attributes = U.ToDictionary(x => x.Key, x => x.Value);

                    mutableAuthentication.AuthenticatedDate = authentication.getAuthenticatedDate();
                    //mutableAuthentication.getAuthenticatedDate() = authentication.getAuthenticatedDate();

                    authToUse = mutableAuthentication;
                }
                else
                {
                    Principal modifiedPrincipal = new SimplePrincipal(principalId, principal.getAttributes());
                    authToUse = new MutableAuthentication(modifiedPrincipal, authentication.getAuthenticatedDate());
                }

                List<Authentication> authentications = new List<Authentication>();

                for (int i = 0; i < chainedAuthenticationsList.Count() - 1; i++)
                {
                    authentications.Add(serviceTicket.getGrantingTicket().getChainedAuthentications().ElementAt(i));
                }
                authentications.Add(authToUse);

                return new ImmutableAssertionImpl(authentications, serviceTicket.getService(), serviceTicket.isFromNewLogin());
            }

            finally
            {
                if (serviceTicket.isExpired())
                {
                    this.serviceTicketRegistry.deleteTicket(serviceTicketId);
                }
            }
        }

        /**
     * Determines the principal id to use for a {@link RegisteredService} using the following rules: 
     * 
     * <ul>
     *  <li> If the service is marked to allow anonymous access, a persistent id is returned. </li>
     *  <li> If the attribute name matches {@link RegisteredService#DEFAULT_USERNAME_ATTRIBUTE}, then the default principal id is returned.</li>
     *  <li>If the service is set to ignore attributes, or the username attribute exists in the allowed attributes for the service, 
     *      the corresponding attribute value will be returned.
     *  </li>
     *   <li>Otherwise, the default principal's id is returned as the username attribute with an additional warning.</li>
     * </ul>
     * 
     * @param principal The principal object to be validated and constructed
     * @param registeredService Requesting service for which a principal is being validated. 
     * @param serviceTicket An instance of the service ticket used for validation
     * 
     * @return The principal id to use for the requesting registered service
     */
        private string determinePrincipalIdForRegisteredService(Principal principal, RegisteredService registeredService,
                                                                 ServiceTicket serviceTicket)
        {
            string principalId = null;
            string serviceUsernameAttribute = registeredService.getUsernameAttribute();

            if (registeredService.isAnonymousAccess())
            {
                principalId = this.persistentIdGenerator.generate(principal, serviceTicket.getService());
            }
            else if (string.IsNullOrEmpty(serviceUsernameAttribute))
            {
                principalId = principal.getId();
            }
            else
            {
                if ((registeredService.isIgnoreAttributes() || registeredService.getAllowedAttributes().Contains(serviceUsernameAttribute)) &&
                    principal.getAttributes().ContainsKey(serviceUsernameAttribute))
                {
                    principalId = principal.getAttributes().First(x => x.Key == registeredService.getUsernameAttribute()).Value.ToString();
                }
                else
                {
                    principalId = principal.getId();
                    Object[] errorLogParameters = new Object[] { principalId, registeredService.getUsernameAttribute(),
                                                                     principal.getAttributes(), registeredService.getServiceId(), principalId };
                    //log.warn("Principal [{}] did not have attribute [{}] among attributes [{}] so CAS cannot "
                    //        + "provide on the validation response the user attribute the registered service [{}] expects. "
                    //        + "CAS will instead return the default username attribute [{}]", errorLogParameters);
                }

            }

            //log.debug("Principal id to return for service [{}] is [{}]. The default principal id is [{}].", 
            //          new Object[] {registeredService.getName(), principal.getId(), principalId});
            return principalId;
        }

        /**
     * @throws IllegalArgumentException if the credentials are null.
     */
        //@Audit(
        //    action="TICKET_GRANTING_TICKET",
        //    actionResolverName="CREATE_TICKET_GRANTING_TICKET_RESOLVER",
        //    resourceResolverName="CREATE_TICKET_GRANTING_TICKET_RESOURCE_RESOLVER")
        //@Profiled(tag = "CREATE_TICKET_GRANTING_TICKET", logFailuresSeparately = false)
        //@Transactional(readOnly = false)
        public string createTicketGrantingTicket(Credentials credentials)
        {
            //Assert.notNull(credentials, "credentials cannot be null");

            try
            {
                Authentication authentication = this.authenticationManager
                    .authenticate(credentials);

                TicketGrantingTicket ticketGrantingTicket = new TicketGrantingTicketImpl(
                    this.ticketGrantingTicketUniqueTicketIdGenerator
                        .getNewTicketId(TicketPrefix.TicketGrantingTicket_PREFIX),
                    authentication, this.ticketGrantingTicketExpirationPolicy);

                this.ticketRegistry.addTicket(ticketGrantingTicket);
                return ticketGrantingTicket.getId();
            }
            catch (AuthenticationException e)
            {
                throw new TicketCreationException(e);
            }
        }

        //   /**
        //* Method to set the TicketRegistry.
        //*
        //* @param ticketRegistry the TicketRegistry to set.
        //*/
        //   public void setTicketRegistry(TicketRegistry ticketRegistry)
        //   {
        //       this.ticketRegistry = ticketRegistry;

        //       if (this.serviceTicketRegistry == null)
        //       {
        //           this.serviceTicketRegistry = ticketRegistry;
        //       }
        //   }

        //   public void setServiceTicketRegistry(TicketRegistry serviceTicketRegistry)
        //   {
        //       this.serviceTicketRegistry = serviceTicketRegistry;
        //   }

        /**
     * Method to inject the AuthenticationManager into the class.
     *
     * @param authenticationManager The authenticationManager to set.
     */
        //public void setAuthenticationManager(
        //    AuthenticationManager authenticationManager)
        //{
        //    this.authenticationManager = authenticationManager;
        //}

        /**
     * Method to inject the TicketGrantingTicket Expiration Policy.
     *
     * @param ticketGrantingTicketExpirationPolicy The
     * ticketGrantingTicketExpirationPolicy to set.
     */
        //   public void setTicketGrantingTicketExpirationPolicy(
        //       ExpirationPolicy ticketGrantingTicketExpirationPolicy)
        //   {
        //       this.ticketGrantingTicketExpirationPolicy = ticketGrantingTicketExpirationPolicy;
        //   }

        //   /**
        //* Method to inject the Unique Ticket Id Generator into the class.
        //*
        //* @param uniqueTicketIdGenerator The uniqueTicketIdGenerator to use
        //*/
        //   public void setTicketGrantingTicketUniqueTicketIdGenerator(
        //       UniqueTicketIdGenerator uniqueTicketIdGenerator)
        //   {
        //       this.ticketGrantingTicketUniqueTicketIdGenerator = uniqueTicketIdGenerator;
        //   }

        //   /**
        //* Method to inject the TicketGrantingTicket Expiration Policy.
        //*
        //* @param serviceTicketExpirationPolicy The serviceTicketExpirationPolicy to
        //* set.
        //*/
        //   public void setServiceTicketExpirationPolicy(
        //       ExpirationPolicy serviceTicketExpirationPolicy)
        //   {
        //       this.serviceTicketExpirationPolicy = serviceTicketExpirationPolicy;
        //   }

        //   public void setUniqueTicketIdGeneratorsForService(
        //       Dictionary<string, UniqueTicketIdGenerator> uniqueTicketIdGeneratorsForService)
        //   {
        //       this.uniqueTicketIdGeneratorsForService = uniqueTicketIdGeneratorsForService;
        //   }

        //   public void setServicesManager(ServicesManager servicesManager)
        //   {
        //       this.servicesManager = servicesManager;
        //   }

        //   public void setPersistentIdGenerator(
        //       PersistentIdGenerator persistentIdGenerator)
        //   {
        //       this.persistentIdGenerator = persistentIdGenerator;
        //   }
    }
}
