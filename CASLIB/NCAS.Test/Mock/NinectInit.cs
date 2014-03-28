using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCAS.Test.SpringConfig;
using NCAS.jasig;
using NCAS.jasig.authentication;
using NCAS.jasig.authentication.handler;
using NCAS.jasig.authentication.handler.support;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.services;
using NCAS.jasig.ticket;
using NCAS.jasig.ticket.proxy;
using NCAS.jasig.ticket.proxy.support;
using NCAS.jasig.ticket.registry;
using NCAS.jasig.ticket.support;
using NCAS.jasig.util;
using NCAS.jasig.web.flow;
using NCAS.jasig.web.support;
using Ninject;

namespace NCAS.Test.Mock
{
    class NinectInit
    {
        private static IKernel _kernel;

        public static Ninject.IKernel Kernel
        {
            get
            {
                if (_kernel == null)
                    _kernel = init();
                return _kernel;
            }
            set { _kernel = value; }
        }

        public static IKernel init()
        {
            var Kernel = new Ninject.StandardKernel();

            Kernel.Bind<UniqueTicketIdGenerator>().To<DefaultUniqueTicketIdGenerator>();

            Kernel.Bind<AuthenticationViaFormAction>().ToSelf();

            Kernel.Bind<AuthenticationHandler>().To
                <SimpleTestUsernamePasswordAuthenticationHandler>();

            Kernel.Bind<ProxyHandler>().To<Cas20ProxyHandler>();

            Kernel.Bind<ServicesManager>().To<DefaultServicesManagerImpl>();

            Kernel.Bind<AuthenticationManager>().To<AuthenticationManagerImpl>().WithConstructorArgument("authenticationHandlers",
                                                                                                         DeployerConfigContextConfig
                                                                                                             .
                                                                                                             authenticationHandlers)
                .WithConstructorArgument("credentialsToPrincipalResolvers", DeployerConfigContextConfig.credentialsToPrincipalResolvers);


            Kernel.Bind<ServiceRegistryDao>().To<InMemoryServiceRegistryDaoImpl>();

            Kernel.Bind<CentralAuthenticationService>().To<CentralAuthenticationServiceImpl>().WithConstructorArgument(
                "uniqueTicketIdGeneratorsForService", UniqueIdGeneratorsConfig.GetUniqueTicketIdGeneratorsForService())
                .WithConstructorArgument("ticketGrantingTicketExpirationPolicy", TicketExpirationPoliciesConfig.grantingTicketExpirationPolicy)
                .WithConstructorArgument("serviceTicketExpirationPolicy", TicketExpirationPoliciesConfig.serviceTicketExpirationPolicy);

            Kernel.Bind<Credentials>().To<UsernamePasswordCredentials>();

            Kernel.Bind<TicketRegistry>().To<DefaultTicketRegistry>();

            Kernel.Bind<ArgumentExtractor>().To<CasArgumentExtractor>();



            return Kernel;
            //Kernel.Bind<ExpirationPolicy>().To<NeverExpiresExpirationPolicy>().WhenAnyAnchestorNamed("");
        }
    }
}
