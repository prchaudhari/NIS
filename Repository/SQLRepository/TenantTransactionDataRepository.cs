﻿// <copyright file="TenantTransactionDataRepository.cs" company="Websym Solutions Pvt. Ltd.">
// Copyright (c) 2018 Websym Solutions Pvt. Ltd..
// </copyright>
//-----------------------------------------------------------------------

namespace nIS
{
    #region References
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using System.Text.RegularExpressions;
    using Unity;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    #endregion

    public class TenantTransactionDataRepository: ITenantTransactionDataRepository
    {
        #region Private Members

        /// <summary>
        /// The validation engine object
        /// </summary>
        IValidationEngine validationEngine = null;

        /// <summary>
        /// The connection string
        /// </summary>
        private string connectionString = string.Empty;

        /// <summary>
        /// The unity container
        /// </summary>
        private IUnityContainer unityContainer = null;

        /// <summary>
        /// The utility object
        /// </summary>
        private IConfigurationUtility configurationutility = null;

        #endregion

        #region Constructor

        public TenantTransactionDataRepository(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
            this.validationEngine = new ValidationEngine();
            this.configurationutility = new ConfigurationUtility(this.unityContainer);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method gets the specified list of customer master from tenant transaction data repository.
        /// </summary>
        /// <param name="customerSearchParameter">The subscription master search parameter</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns the list of customer master
        /// </returns>
        public IList<CustomerMaster> Get_TTD_CustomerMasters(CustomerSearchParameter customerSearchParameter, string tenantCode)
        {
            IList<CustomerMaster> customerMasters = new List<CustomerMaster>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGeneratorForCustomer(customerSearchParameter, tenantCode);
                var customerMasterRecords = new List<TTD_CustomerMasterRecord>();
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    if (customerSearchParameter.PagingParameter.PageIndex > 0 && customerSearchParameter.PagingParameter.PageSize > 0)
                    {
                        customerMasterRecords = nISEntitiesDataContext.TTD_CustomerMasterRecord
                        .OrderBy(customerSearchParameter.SortParameter.SortColumn + " " + customerSearchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((customerSearchParameter.PagingParameter.PageIndex - 1) * customerSearchParameter.PagingParameter.PageSize)
                        .Take(customerSearchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else
                    {
                        customerMasterRecords = nISEntitiesDataContext.TTD_CustomerMasterRecord
                        .Where(whereClause)
                        .OrderBy(customerSearchParameter.SortParameter.SortColumn + " " + customerSearchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }

                    if (customerMasterRecords != null && customerMasterRecords.Count > 0)
                    {
                        customerMasterRecords.ForEach(item =>
                        {
                            customerMasters.Add(new CustomerMaster()
                            {
                                Identifier = item.Id,
                                BatchId = item.BatchId,
                                CustomerCode = item.CustomerCode,
                                FirstName = item.FirstName,
                                MiddleName = item.MiddleName,
                                LastName = item.LastName,
                                AddressLine1 = item.AddressLine1,
                                AddressLine2 = item.AddressLine2,
                                City = item.City,
                                State = item.State,
                                Country = item.Country,
                                Zip = item.Zip,
                                StatementDate = item.StatementDate,
                                StatementPeriod = item.StatementPeriod,
                                TenantCode = item.TenantCode
                            });
                        });
                    }
                }
                return customerMasters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method gets the specified list of subscription master from tenant transaction data repository.
        /// </summary>
        /// <param name="subscriptionMasterSearchParameter">The subscription master search parameter</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns the list of subscription master
        /// </returns>
        public IList<SubscriptionMaster> Get_TTD_SubscriptionMasters(SubscriptionMasterSearchParameter subscriptionMasterSearchParameter, string tenantCode)
        {
            IList<SubscriptionMaster> subscriptionMasters = new List<SubscriptionMaster>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGenerator(subscriptionMasterSearchParameter, tenantCode);
                var subscriptionMasterRecords = new List<TTD_SubscriptionMasterRecord>();
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    if (subscriptionMasterSearchParameter.PagingParameter.PageIndex > 0 && subscriptionMasterSearchParameter.PagingParameter.PageSize > 0)
                    {
                        subscriptionMasterRecords = nISEntitiesDataContext.TTD_SubscriptionMasterRecord
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((subscriptionMasterSearchParameter.PagingParameter.PageIndex - 1) * subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .Take(subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else 
                    {
                        subscriptionMasterRecords = nISEntitiesDataContext.TTD_SubscriptionMasterRecord
                        .Where(whereClause)
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }

                    if (subscriptionMasterRecords != null && subscriptionMasterRecords.Count > 0)
                    {
                        subscriptionMasterRecords.ForEach(item =>
                        {
                            subscriptionMasters.Add(new SubscriptionMaster()
                            {
                                Identifier = item.Id,
                                BatchId = item.BatchId,
                                CustomerId = item.CustomerId,
                                CustomerCode = item.CustomerCode,
                                VendorName = item.VendorName,
                                Subscription = item.Subscription,
                                EmailId = item.Email,
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                TenantCode = item.TenantCode
                            });
                        });
                    }
                }
                return subscriptionMasters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method gets the specified list of subscription usage from tenant transaction data repository.
        /// </summary>
        /// <param name="subscriptionMasterSearchParameter">The subscription master search parameter</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns the list of subscription usage
        /// </returns>
        public IList<SubscriptionUsage> Get_TTD_SubscriptionUsages(SubscriptionMasterSearchParameter subscriptionMasterSearchParameter, string tenantCode)
        {
            IList<SubscriptionUsage> subscriptionUsages = new List<SubscriptionUsage>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGenerator(subscriptionMasterSearchParameter, tenantCode);
                var subscriptionUsageRecords = new List<TTD_SubscriptionUsageRecord>();
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    if (subscriptionMasterSearchParameter.PagingParameter.PageIndex > 0 && subscriptionMasterSearchParameter.PagingParameter.PageSize > 0)
                    {
                        subscriptionUsageRecords = nISEntitiesDataContext.TTD_SubscriptionUsageRecord
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((subscriptionMasterSearchParameter.PagingParameter.PageIndex - 1) * subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .Take(subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else
                    {
                        subscriptionUsageRecords = nISEntitiesDataContext.TTD_SubscriptionUsageRecord
                        .Where(whereClause)
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }

                    if (subscriptionUsageRecords != null && subscriptionUsageRecords.Count > 0)
                    {
                        subscriptionUsageRecords.ForEach(item =>
                        {
                            subscriptionUsages.Add(new SubscriptionUsage()
                            {
                                Identifier = item.Id,
                                BatchId = item.BatchId,
                                CustomerId = item.CustomerId,
                                VendorName = item.VendorName,
                                Subscription = item.Subscription,
                                Email = item.Email,
                                Usage = item.Usage,
                                Emails = item.Emails,
                                Meetings = item.Meetings,
                                TenantCode = item.TenantCode
                            });
                        });
                    }
                }
                return subscriptionUsages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method gets the specified list of subscription summaries from tenant transaction data repository.
        /// </summary>
        /// <param name="subscriptionMasterSearchParameter">The subscription master search parameter</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns the list of subscription summeries
        /// </returns>
        public IList<SubscriptionSummary> Get_TTD_SubscriptionSummaries(SubscriptionMasterSearchParameter subscriptionMasterSearchParameter, string tenantCode)
        {
            IList<SubscriptionSummary> subscriptionSummaries = new List<SubscriptionSummary>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGenerator(subscriptionMasterSearchParameter, tenantCode);
                var subscriptionSummaryRecords = new List<TTD_SubscriptionSummaryRecord>();
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    if (subscriptionMasterSearchParameter.PagingParameter.PageIndex > 0 && subscriptionMasterSearchParameter.PagingParameter.PageSize > 0)
                    {
                        subscriptionSummaryRecords = nISEntitiesDataContext.TTD_SubscriptionSummaryRecord
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((subscriptionMasterSearchParameter.PagingParameter.PageIndex - 1) * subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .Take(subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else
                    {
                        subscriptionSummaryRecords = nISEntitiesDataContext.TTD_SubscriptionSummaryRecord
                        .Where(whereClause)
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }

                    if (subscriptionSummaryRecords != null && subscriptionSummaryRecords.Count > 0)
                    {
                        subscriptionSummaryRecords.ForEach(item =>
                        {
                            subscriptionSummaries.Add(new SubscriptionSummary()
                            {
                                Identifier = item.Id,
                                BatchId = item.BatchId,
                                CustomerId = item.CustomerId,
                                VendorName = item.VendorName,
                                Subscription = item.Subscription,
                                Total = item.Total,
                                AverageSpend = item.AverageSpend,
                                TenantCode = item.TenantCode
                            });
                        });
                    }
                }
                return subscriptionSummaries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method gets the specified list of subscription spends from tenant transaction data repository.
        /// </summary>
        /// <param name="subscriptionMasterSearchParameter">The subscription master search parameter</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns the list of subscription spends
        /// </returns>
        public IList<SubscriptionSpend> Get_TTD_SubscriptionSpends(SubscriptionMasterSearchParameter subscriptionMasterSearchParameter, string tenantCode)
        {
            IList<SubscriptionSpend> subscriptionSpends = new List<SubscriptionSpend>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGenerator(subscriptionMasterSearchParameter, tenantCode);
                var subscriptionSpendRecords = new List<TTD_SubscriptionSpendRecord>();
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    if (subscriptionMasterSearchParameter.PagingParameter.PageIndex > 0 && subscriptionMasterSearchParameter.PagingParameter.PageSize > 0)
                    {
                        subscriptionSpendRecords = nISEntitiesDataContext.TTD_SubscriptionSpendRecord
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((subscriptionMasterSearchParameter.PagingParameter.PageIndex - 1) * subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .Take(subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else
                    {
                        subscriptionSpendRecords = nISEntitiesDataContext.TTD_SubscriptionSpendRecord
                        .Where(whereClause)
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }

                    if (subscriptionSpendRecords != null && subscriptionSpendRecords.Count > 0)
                    {
                        subscriptionSpendRecords.ForEach(item =>
                        {
                            subscriptionSpends.Add(new SubscriptionSpend()
                            {
                                Identifier = item.Id,
                                BatchId = item.BatchId,
                                CustomerId = item.CustomerId,
                                Month = item.Month,
                                Year = item.Year,
                                Microsoft = item.Microsoft,
                               Zoom = item.Zoom,
                               TenantCode = item.TenantCode
                            });
                        });
                    }
                }
                return subscriptionSpends;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method gets the specified list of user subscription from tenant transaction data repository.
        /// </summary>
        /// <param name="subscriptionMasterSearchParameter">The subscription master search parameter</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns the list of user subscriptions
        /// </returns>
        public IList<UserSubscription> Get_TTD_UserSubscriptions(SubscriptionMasterSearchParameter subscriptionMasterSearchParameter, string tenantCode)
        {
            IList<UserSubscription> userSubscriptions = new List<UserSubscription>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGenerator(subscriptionMasterSearchParameter, tenantCode);
                var userSubscriptionRecords = new List<TTD_UserSubscriptionsRecord>();
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    if (subscriptionMasterSearchParameter.PagingParameter.PageIndex > 0 && subscriptionMasterSearchParameter.PagingParameter.PageSize > 0)
                    {
                        userSubscriptionRecords = nISEntitiesDataContext.TTD_UserSubscriptionsRecord
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((subscriptionMasterSearchParameter.PagingParameter.PageIndex - 1) * subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .Take(subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else
                    {
                        userSubscriptionRecords = nISEntitiesDataContext.TTD_UserSubscriptionsRecord
                        .Where(whereClause)
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }

                    if (userSubscriptionRecords != null && userSubscriptionRecords.Count > 0)
                    {
                        userSubscriptionRecords.ForEach(item =>
                        {
                            userSubscriptions.Add(new UserSubscription()
                            {
                                Identifier = item.Id,
                                BatchId = item.BatchId,
                                CustomerId = item.CustomerId,
                                CountOfSubscription = item.CountOfSubscription,
                                TenantCode = item.TenantCode
                            });
                        });
                    }
                }
                return userSubscriptions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method gets the specified list of vendor subscription from tenant transaction data repository.
        /// </summary>
        /// <param name="subscriptionMasterSearchParameter">The subscription master search parameter</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns the list of vendor subscriptions
        /// </returns>
        public IList<VendorSubscription> Get_TTD_VendorSubscriptions(SubscriptionMasterSearchParameter subscriptionMasterSearchParameter, string tenantCode)
        {
            IList<VendorSubscription> vendorSubscriptions = new List<VendorSubscription>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGenerator(subscriptionMasterSearchParameter, tenantCode);
                var vendorSubscriptionRecords = new List<TTD_VendorSubscriptionRecord>();
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    if (subscriptionMasterSearchParameter.PagingParameter.PageIndex > 0 && subscriptionMasterSearchParameter.PagingParameter.PageSize > 0)
                    {
                        vendorSubscriptionRecords = nISEntitiesDataContext.TTD_VendorSubscriptionRecord
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((subscriptionMasterSearchParameter.PagingParameter.PageIndex - 1) * subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .Take(subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else
                    {
                        vendorSubscriptionRecords = nISEntitiesDataContext.TTD_VendorSubscriptionRecord
                        .Where(whereClause)
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }

                    if (vendorSubscriptionRecords != null && vendorSubscriptionRecords.Count > 0)
                    {
                        vendorSubscriptionRecords.ForEach(item =>
                        {
                            vendorSubscriptions.Add(new VendorSubscription()
                            {
                                Identifier = item.Id,
                                BatchId = item.BatchId,
                                CustomerId = item.CustomerId,
                                VenderName = item.VenderName,
                                CountOfSubscription = item.CountOfSubscription,
                                TenantCode = item.TenantCode
                            });
                        });
                    }
                }
                return vendorSubscriptions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method gets the specified list of data usages from tenant transaction data repository.
        /// </summary>
        /// <param name="subscriptionMasterSearchParameter">The subscription master search parameter</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns the list of data usages
        /// </returns>
        public IList<DataUsage> Get_TTD_DataUsages(SubscriptionMasterSearchParameter subscriptionMasterSearchParameter, string tenantCode)
        {
            IList<DataUsage> dataUsages = new List<DataUsage>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGenerator(subscriptionMasterSearchParameter, tenantCode);
                var dataUsageRecords = new List<TTD_DataUsageRecord>();
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    if (subscriptionMasterSearchParameter.PagingParameter.PageIndex > 0 && subscriptionMasterSearchParameter.PagingParameter.PageSize > 0)
                    {
                        dataUsageRecords = nISEntitiesDataContext.TTD_DataUsageRecord
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((subscriptionMasterSearchParameter.PagingParameter.PageIndex - 1) * subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .Take(subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else
                    {
                        dataUsageRecords = nISEntitiesDataContext.TTD_DataUsageRecord
                        .Where(whereClause)
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }

                    if (dataUsageRecords != null && dataUsageRecords.Count > 0)
                    {
                        dataUsageRecords.ForEach(item =>
                        {
                            dataUsages.Add(new DataUsage()
                            {
                                Identifier = item.Id,
                                BatchId = item.BatchId,
                                CustomerId = item.CustomerId,
                                Month = item.Month,
                                Year = item.Year,
                                Microsoft = item.Microsoft,
                                Zoom = item.Zoom,
                                TenantCode = item.TenantCode
                            });
                        });
                    }
                }
                return dataUsages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method gets the specified list of meeting usages from tenant transaction data repository.
        /// </summary>
        /// <param name="subscriptionMasterSearchParameter">The subscription master search parameter</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns the list of meeting usages
        /// </returns>
        public IList<MeetingUsage> Get_TTD_MeetingUsages(SubscriptionMasterSearchParameter subscriptionMasterSearchParameter, string tenantCode)
        {
            IList<MeetingUsage> meetingUsages = new List<MeetingUsage>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGenerator(subscriptionMasterSearchParameter, tenantCode);
                var meetingUsageRecords = new List<TTD_MeetingUsageRecord>();
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    if (subscriptionMasterSearchParameter.PagingParameter.PageIndex > 0 && subscriptionMasterSearchParameter.PagingParameter.PageSize > 0)
                    {
                        meetingUsageRecords = nISEntitiesDataContext.TTD_MeetingUsageRecord
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((subscriptionMasterSearchParameter.PagingParameter.PageIndex - 1) * subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .Take(subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else
                    {
                        meetingUsageRecords = nISEntitiesDataContext.TTD_MeetingUsageRecord
                        .Where(whereClause)
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }

                    if (meetingUsageRecords != null && meetingUsageRecords.Count > 0)
                    {
                        meetingUsageRecords.ForEach(item =>
                        {
                            meetingUsages.Add(new MeetingUsage()
                            {
                                Identifier = item.Id,
                                BatchId = item.BatchId,
                                CustomerId = item.CustomerId,
                                Month = item.Month,
                                Year = item.Year,
                                Microsoft = item.Microsoft,
                                Zoom = item.Zoom,
                                TenantCode = item.TenantCode
                            });
                        });
                    }
                }
                return meetingUsages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method gets the specified list of emails by subscription from tenant transaction data repository.
        /// </summary>
        /// <param name="subscriptionMasterSearchParameter">The subscription master search parameter</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns the list of emails by subscription
        /// </returns>
        public IList<EmailsBySubscription> Get_TTD_EmailsBySubscription(SubscriptionMasterSearchParameter subscriptionMasterSearchParameter, string tenantCode)
        {
            IList<EmailsBySubscription> emailsBySubscriptions = new List<EmailsBySubscription>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGenerator(subscriptionMasterSearchParameter, tenantCode);
                var emailsBySubscriptionRecords = new List<TTD_EmailsBySubscriptionRecord>();
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    if (subscriptionMasterSearchParameter.PagingParameter.PageIndex > 0 && subscriptionMasterSearchParameter.PagingParameter.PageSize > 0)
                    {
                        emailsBySubscriptionRecords = nISEntitiesDataContext.TTD_EmailsBySubscriptionRecord
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((subscriptionMasterSearchParameter.PagingParameter.PageIndex - 1) * subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .Take(subscriptionMasterSearchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else
                    {
                        emailsBySubscriptionRecords = nISEntitiesDataContext.TTD_EmailsBySubscriptionRecord
                        .Where(whereClause)
                        .OrderBy(subscriptionMasterSearchParameter.SortParameter.SortColumn + " " + subscriptionMasterSearchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }

                    if (emailsBySubscriptionRecords != null && emailsBySubscriptionRecords.Count > 0)
                    {
                        emailsBySubscriptionRecords.ForEach(item =>
                        {
                            emailsBySubscriptions.Add(new EmailsBySubscription()
                            {
                                Identifier = item.Id,
                                BatchId = item.BatchId,
                                CustomerId = item.CustomerId,
                                Emails = item.Emails,
                                Subscription = item.Subscription,
                                TenantCode = item.TenantCode
                            });
                        });
                    }
                }
                return emailsBySubscriptions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate string for dynamic linq.
        /// </summary>
        /// <param name="searchParameter">Role search Parameters</param>
        /// <returns>
        /// Returns a string.
        /// </returns>
        private string WhereClauseGenerator(SubscriptionMasterSearchParameter searchParameter, string tenantCode)
        {
            StringBuilder queryString = new StringBuilder();

            if (searchParameter.SearchMode == SearchMode.Equals)
            {
                if (validationEngine.IsValidLong(searchParameter.Identifier))
                {
                    queryString.Append("(" + string.Join("or ", searchParameter.Identifier.ToString().Split(',').Select(item => string.Format("Id.Equals({0}) ", item))) + ") and ");
                }
            }
            if (validationEngine.IsValidLong(searchParameter.BatchId))
            {
                queryString.Append("(" + string.Join("or ", searchParameter.BatchId.ToString().Split(',').Select(item => string.Format("BatchId.Equals({0}) ", item))) + ") and ");
            }
            if (validationEngine.IsValidLong(searchParameter.CustomerId))
            {
                queryString.Append("(" + string.Join("or ", searchParameter.CustomerId.ToString().Split(',').Select(item => string.Format("CustomerId.Equals({0}) ", item))) + ") and ");
            }
            if (validationEngine.IsValidText(searchParameter.VendorName))
            {
                queryString.Append(string.Format("VendorName.Contains(\"{0}\") and ", searchParameter.VendorName));
            }
            if (validationEngine.IsValidText(searchParameter.Subscription))
            {
                queryString.Append(string.Format("Subscription.Contains(\"{0}\") and ", searchParameter.Subscription));
            }
            if (validationEngine.IsValidText(searchParameter.EmailId))
            {
                queryString.Append(string.Format("Email.Contains(\"{0}\") and ", searchParameter.EmailId));
            }
            queryString.Append(string.Format("TenantCode.Equals(\"{0}\") ", tenantCode));
            return queryString.ToString();
        }

        private string WhereClauseGeneratorForCustomer(CustomerSearchParameter searchParameter, string tenantCode)
        {
            StringBuilder queryString = new StringBuilder();

            if (validationEngine.IsValidLong(searchParameter.Identifier))
            {
                queryString.Append("(" + string.Join("or ", searchParameter.Identifier.ToString().Split(',').Select(item => string.Format("Id.Equals({0}) ", item))) + ") and ");
            }
            if (validationEngine.IsValidLong(searchParameter.BatchId))
            {
                queryString.Append("(" + string.Join("or ", searchParameter.BatchId.ToString().Split(',').Select(item => string.Format("BatchId.Equals({0}) ", item))) + ") and ");
            }
            queryString.Append(string.Format("TenantCode.Equals(\"{0}\") ", tenantCode));
            return queryString.ToString();
        }

        /// <summary>
        /// This method help to set and validate connection string
        /// </summary>
        /// <param name="tenantCode">
        /// The tenant code
        /// </param>
        private void SetAndValidateConnectionString(string tenantCode)
        {
            try
            {
                this.connectionString = validationEngine.IsValidText(this.connectionString) ? this.connectionString : this.configurationutility.GetConnectionString(ModelConstant.COMMON_SECTION, ModelConstant.NIS_CONNECTION_STRING, ModelConstant.CONFIGURATON_BASE_URL, ModelConstant.TENANT_CODE_KEY, tenantCode);
                if (!this.validationEngine.IsValidText(this.connectionString))
                {
                    throw new ConnectionStringNotFoundException(tenantCode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }
}