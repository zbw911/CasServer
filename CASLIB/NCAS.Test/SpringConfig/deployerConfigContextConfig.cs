using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCAS.jasig.authentication.handler;
using NCAS.jasig.authentication.handler.support;
using NCAS.jasig.authentication.principal;

namespace NCAS.Test.SpringConfig
{
    class DeployerConfigContextConfig
    {
        //public static CredentialsToPrincipalResolver UsernamePasswordCredentialsToPrincipalResolver =
        //    new UsernamePasswordCredentialsToPrincipalResolver();

        //private List<AuthenticationHandler> authenticationHandlers;
        public static List<CredentialsToPrincipalResolver> credentialsToPrincipalResolvers
        {
            get
            {
                UsernamePasswordCredentialsToPrincipalResolver usernamePasswordCredentialsToPrincipalResolver =
                   new UsernamePasswordCredentialsToPrincipalResolver();

                HttpBasedServiceCredentialsToPrincipalResolver httpBasedServiceCredentialsToPrincipalResolver =
                    new HttpBasedServiceCredentialsToPrincipalResolver();

                //usernamePasswordCredentialsToPrincipalResolver.setAttributeRepository()
                return new List<CredentialsToPrincipalResolver>
                           {
                               usernamePasswordCredentialsToPrincipalResolver,
                               httpBasedServiceCredentialsToPrincipalResolver
                           };
            }
        }

        public static List<AuthenticationHandler> authenticationHandlers
        {
            get
            {
                HttpBasedServiceCredentialsAuthenticationHandler httpBasedServiceCredentialsAuthenticationHandler =
                    new HttpBasedServiceCredentialsAuthenticationHandler();

                SimpleTestUsernamePasswordAuthenticationHandler simpleTestUsernamePasswordAuthenticationHandler =
                    new SimpleTestUsernamePasswordAuthenticationHandler();

                return new List<AuthenticationHandler>
                           {
                               httpBasedServiceCredentialsAuthenticationHandler,
                               simpleTestUsernamePasswordAuthenticationHandler
                           };

            }
        }
    }
}
