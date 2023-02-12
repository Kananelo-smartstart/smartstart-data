using Microsoft.Xrm.Sdk;
using SL.DataFixes.Factories.Interfaces;
using SL.DataFixes.Interfaces;
using SL.DataFixes.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.DataFixes.Factories
{
    public class CrmAuthenticationFactory : IAuthenticationFactory<IOrganizationService>
    {
        public IAuthentication<IOrganizationService> BuildIAuthentication()
        {
            return new CrmAuthenticator();
        }
    }
}
