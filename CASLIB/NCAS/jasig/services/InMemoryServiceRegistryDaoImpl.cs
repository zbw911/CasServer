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
////package org.jasig.cas.services;

////import javax.validation.constraints.NotNull;
////import java.util.ArrayList;
////import java.util.List;

/**
 * Default In Memory Service Registry Dao for test/demonstration purposes.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.1 $ $Date: 2005/08/19 18:27:17 $
 * @since 3.1
 *
 */

using System.Collections.Generic;

namespace NCAS.jasig.services
{
    public class InMemoryServiceRegistryDaoImpl : ServiceRegistryDao
    {


        private List<RegisteredService> registeredServices = new List<RegisteredService>();

        public bool delete(RegisteredService registeredService)
        {
            return this.registeredServices.Remove(registeredService);
        }

        public RegisteredService findServiceById(long id)
        {
            foreach (RegisteredService r in this.registeredServices)
            {
                if (r.getId() == id)
                {
                    return r;
                }
            }

            return null;
        }

        public List<RegisteredService> load()
        {
            return this.registeredServices;
        }

        public RegisteredService save(RegisteredService registeredService)
        {
            if (registeredService.getId() == -1)
            {
                ((AbstractRegisteredService)registeredService).setId(this.findHighestId() + 1);
            }

            this.registeredServices.Remove(registeredService);
            this.registeredServices.Add(registeredService);

            return registeredService;
        }

        public void setRegisteredServices(List<RegisteredService> registeredServices)
        {
            this.registeredServices = registeredServices;
        }

        /**
     * This isn't super-fast but I don't expect thousands of services.
     *
     * @return
     */
        private long findHighestId()
        {
            long id = 0;

            foreach (RegisteredService r in this.registeredServices)
            {
                if (r.getId() > id)
                {
                    id = r.getId();
                }
            }

            return id;
        }
    }
}
