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
//package org.jasig.cas.audit.spi;

//import org.aspectj.lang.JoinPoint;
//import com.github.inspektr.common.spi.PrincipalResolver;
//import org.jasig.cas.authentication.principal.Credentials;
//import org.jasig.cas.ticket.ServiceTicket;
//import org.jasig.cas.ticket.Ticket;
//import org.jasig.cas.ticket.TicketGrantingTicket;
//import org.jasig.cas.ticket.registry.TicketRegistry;
//import org.jasig.cas.util.AopUtils;
//import org.springframework.security.core.Authentication;
//import org.springframework.security.core.context.SecurityContext;
//import org.springframework.security.core.context.SecurityContextHolder;
//import org.springframework.security.core.userdetails.UserDetails;

//import javax.validation.constraints.NotNull;

/**
 * PrincipalResolver that can retrieve the username from either the Ticket or from the Credentials.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.1 $ $Date: 2005/08/19 18:27:17 $
 * @since 3.1.2
 *
 */
public  class TicketOrCredentialPrincipalResolver : PrincipalResolver {
    
    //@NotNull
    private  TicketRegistry ticketRegistry;

    public TicketOrCredentialPrincipalResolver( TicketRegistry ticketRegistry) {
        this.ticketRegistry = ticketRegistry;
    }

    public string resolveFrom( JoinPoint joinPoint,  Object retVal) {
        return resolveFromInternal(AopUtils.unWrapJoinPoint(joinPoint));
    }

    public string resolveFrom( JoinPoint joinPoint,  Exception retVal) {
        return resolveFromInternal(AopUtils.unWrapJoinPoint(joinPoint));
    }

    public string resolve() {
        return UNKNOWN_USER;
    }
    
    protected string resolveFromInternal( JoinPoint joinPoint) {
         Object arg1 = joinPoint.getArgs()[0];
        if (arg1 is Credentials) {
           return arg1.toString();
        } else if (arg1 is string) {
             Ticket ticket = this.ticketRegistry.getTicket((string) arg1);
            if (ticket is ServiceTicket) {
                 ServiceTicket serviceTicket = (ServiceTicket) ticket;
                return serviceTicket.getGrantingTicket().getAuthentication().getPrincipal().getId();
            } else if (ticket is TicketGrantingTicket) {
                 TicketGrantingTicket tgt = (TicketGrantingTicket) ticket;
                return tgt.getAuthentication().getPrincipal().getId();
            }
        } else {
             SecurityContext securityContext = SecurityContextHolder.getContext();
            if (securityContext != null) {
                 Authentication authentication = securityContext.getAuthentication();

                if (authentication != null) {
                    return ((UserDetails) authentication.getPrincipal()).getUsername();
                }
            }
        }
        return UNKNOWN_USER;
    }
}
