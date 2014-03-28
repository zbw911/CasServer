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
//package org.jasig.cas.authentication;

//import java.util.Map;
//import javax.validation.constraints.NotNull;
//import javax.validation.constraints.Size;

//import org.jasig.cas.authentication.handler.AuthenticationException;
//import org.jasig.cas.authentication.handler.AuthenticationHandler;
//import org.jasig.cas.authentication.handler.BadCredentialsAuthenticationException;
//import org.jasig.cas.authentication.handler.UnsupportedCredentialsException;
//import org.jasig.cas.authentication.principal.Credentials;
//import org.jasig.cas.authentication.principal.CredentialsToPrincipalResolver;
//import org.jasig.cas.authentication.principal.Principal;

/**
 * Ensures that all authentication handlers are tried, but if one is tried, the associated CredentialsToPrincipalResolver is used.
 *
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.3.5
 */

using System.Collections.Generic;
using System.Linq;
using NCAS.jasig.authentication;
using NCAS.jasig.authentication.handler;
using NCAS.jasig.authentication.principal;
using System;

public class LinkedAuthenticationHandlerAndCredentialsToPrincipalResolverAuthenticationManager : AbstractAuthenticationManager
{

    //@NotNull
    //@Size(min = 1)
    private Dictionary<AuthenticationHandler, CredentialsToPrincipalResolver> linkedHandlers;

    public LinkedAuthenticationHandlerAndCredentialsToPrincipalResolverAuthenticationManager(Dictionary<AuthenticationHandler, CredentialsToPrincipalResolver> linkedHandlers)
    {
        this.linkedHandlers = linkedHandlers;
    }

    //@Override
    protected override Pair<AuthenticationHandler, Principal> authenticateAndObtainPrincipal(Credentials credentials)
    {
        bool foundOneThatWorks = false;
        string handlerName;

        foreach (AuthenticationHandler authenticationHandler in this.linkedHandlers.Keys)
        {
            if (!authenticationHandler.supports(credentials))
            {
                continue;
            }

            foundOneThatWorks = true;
            bool authenticated = false;
            handlerName = authenticationHandler.GetType().FullName;

            try
            {
                authenticated = authenticationHandler.authenticate(credentials);
            }
            catch (Exception e)
            {
                handleError(handlerName, credentials, e);
            }

            if (authenticated)
            {
                //log.info("{} successfully authenticated {}", handlerName, credentials);
                Principal p = this.linkedHandlers.First(x => x.Key == authenticationHandler).Value.resolvePrincipal(credentials);
                return new Pair<AuthenticationHandler, Principal>(authenticationHandler, p);
            }
            //log.info("{} failed to authenticate {}", handlerName, credentials);
        }

        if (foundOneThatWorks)
        {
            throw BadCredentialsAuthenticationException.ERROR;
        }

        throw UnsupportedCredentialsException.ERROR;
    }
}
