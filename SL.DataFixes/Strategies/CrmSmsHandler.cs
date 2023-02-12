using Microsoft.Xrm.Sdk;
using SL.DataFixes.Interfaces;
using System;
namespace SL.DataFixes.Strategies
{
    /// <summary>
    /// Instance of an <see cref="ISmsHandler<IOrganizationService, Entity>"/>
    /// </summary>
    public class CrmSmsHandler : ISmsHandler<IOrganizationService, Entity>
    {
        #region Implemenations
        /// <summary>
        /// Creates and Sms record for the given <paramref name="orgService"/> and <paramref name="sms"/>
        /// </summary>
        /// <param name="orgService"><see cref="IOrganizationService"/></param>
        /// <param name="sms"><see cref="Entity"/></param>
        /// <returns><see cref="Guid"/></returns>
        public Guid CreateSms(IOrganizationService orgService, Entity sms)
        {
            return orgService.Create(sms);
        }
        #endregion
    }
}
