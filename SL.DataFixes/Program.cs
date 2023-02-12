using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SL.DataFixes.Factories;
using SL.DataFixes.Properties;
using SL.DataFixes.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SL.DataFixes
{
    public class Program
    {
        static void Main(string[] args)
        {
            var crmSvc = new CrmAuthenticationFactory().BuildIAuthentication().Connect(Settings.Default.SLConnString, "adm_crm", "xh@nt1lomziXXX2019", "ngo"); //new CrmServiceClient(Settings.Default.Url);//
            if (crmSvc != null)
                FixCaregivers(crmSvc);
        }
        #region Handlers
        private static void SendSms(IOrganizationService crmSvc)
        {
            var query = new QueryExpression("iq_club") { ColumnSet = new ColumnSet("iq_clubleader") };
            query.Criteria = new FilterExpression(LogicalOperator.And);
            query.Criteria.AddCondition("iq_clubleader", ConditionOperator.NotNull);
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            var link = new LinkEntity("iq_club", "iq_clubleader", "iq_clubleader", "iq_clubleaderid", JoinOperator.Inner)
            {
                Columns = new ColumnSet("iq_franchisee", "iq_personalnumber"),
                EntityAlias = "cl"
            };
            query.LinkEntities.Add(link);
            var clubs = crmSvc.RetrieveMultiple(query).Entities;
            if (clubs.Count > 0)
            {
                var clubLeaders = new List<EntityReference>();
                foreach (var club in clubs)
                {
                    if (club.Contains("cl.iq_franchisee"))
                    {
                        if (clubLeaders.Count(x => x.Id == ((EntityReference)((AliasedValue)club["cl.iq_franchisee"]).Value).Id) == 0)
                            clubLeaders.Add(((EntityReference)((AliasedValue)club["cl.iq_franchisee"]).Value));
                    }
                }
                foreach (var clubLeader in clubLeaders)
                {
                    var sms = new Entity("iq_sms");
                    sms.Attributes.Add("subject", "Smartstarters Unite 2023");
                    sms.Attributes.Add("iq_smstype", new OptionSetValue(261120001));
                    sms.Attributes.Add("regardingobjectid", clubLeader);
                    sms.Attributes.Add("iq_message", $"Hello {clubLeader.Name.Split(' ')[0]} :) Your club is invited to the Virtual SmartStarter Unite Event happening on 11 Mar 2023. To confirm your club's participation, please use this USSD *134*48333*13#. Club Leaders will receive vouchers to buy snacks for the club on the day of the event. If you're experiencing any challenges, please call 0800 014 817. SmartStart");
                    crmSvc.Create(sms);
                }
            }
        }
        private static void FixCaregivers(IOrganizationService crmSvc)
        {
            QueryExpression query = GetCaregivers();
            var carevigers = crmSvc.RetrieveMultiple(query);
            if (carevigers.Entities.Count > 0)
            {
                foreach (var caregiver in carevigers.Entities)
                {
                    var children = GetChildrenByCaregiverId(crmSvc, caregiver.Id);
                    if (children.Count > 0)
                    {
                        var update = new Entity(caregiver.LogicalName, caregiver.Id);
                        update.Attributes.Add("iq_franchisee", (EntityReference)children.FirstOrDefault()["parentcustomerid"]);
                        try
                        {
                            crmSvc.Update(update);
                        }
                        catch (System.Exception ex)
                        {
                        }

                    }
                }
            }
        }
        #endregion
        #region Methods
        private static QueryExpression GetCaregivers()
        {
            var query = new QueryExpression("iq_primarycaregiver") { ColumnSet = new ColumnSet("iq_name") };
            query.Criteria = new FilterExpression(LogicalOperator.And);
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            query.Criteria.AddCondition("iq_franchisee", ConditionOperator.Null);
            query.Criteria.AddCondition("iq_numberofchildren", ConditionOperator.GreaterThan, 0);

            return query;
        }
        private static DataCollection<Entity> GetChildrenByCaregiverId(IOrganizationService crmSvc, Guid caregiverId)
        {
            var childQuery = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet("fullname", "parentcustomerid", "createdon"),
                Criteria = new FilterExpression(LogicalOperator.And),
            };
            childQuery.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            childQuery.Criteria.AddCondition("iq_primarycaregiver", ConditionOperator.Equal, caregiverId);
            childQuery.AddOrder("createdon", OrderType.Descending);
            childQuery.TopCount = 1;
            var children = crmSvc.RetrieveMultiple(childQuery).Entities;
            return children;
        }
        #endregion

    }
}
