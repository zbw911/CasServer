using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.CasServer
{
    /// <summary>
    /// 无权限，抛出异常
    /// </summary>
    public class ClientNoPermissionException : Exception
    {
        public ClientNoPermissionException(string message)
            : base(message)
        {

        }
    }
}
