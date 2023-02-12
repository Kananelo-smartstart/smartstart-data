using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using SL.DataFixes.Interfaces;
using System;
using System.Configuration;
using System.ServiceModel.Description;

namespace SL.DataFixes.Strategies
{
    public class CrmAuthenticator : IAuthentication<IOrganizationService>
    {
        public IOrganizationService Connect(string connString, string username, string password, string domain)
        {
            CrmServiceClient conn = new CrmServiceClient(connString);
            var _orgService = (IOrganizationService)conn.OrganizationServiceProxy;
            Guid userid = ((WhoAmIResponse)_orgService.Execute(new WhoAmIRequest())).UserId;
            return _orgService;
        }
    }
}
