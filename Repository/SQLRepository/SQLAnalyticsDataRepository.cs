﻿// <copyright file="SQLAnalyticsDataRepository.cs" company="Websym Solutions Pvt Ltd">
// Copyright (c) 2017 Websym Solutions Pvt Ltd.
// </copyright>
// -----------------------------------------------------------------------  

namespace nIS
{
    #region Referemces 

    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Net.NetworkInformation;
    using System.Text;
    using Unity;

    #endregion

    /// <summary>
    /// This class represents repository layer of accet library for crud operation.
    /// </summary>
    public class SQLAnalyticsDataRepository : IAnalyticsDataRepository
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
        /// The utility object
        /// </summary>
        private IUtility utility = null;

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

        /// <summary>
        /// Initializing instance of class.
        /// </summary>
        public SQLAnalyticsDataRepository(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
            this.validationEngine = new ValidationEngine();
            this.configurationutility = new ConfigurationUtility(this.unityContainer);
            this.utility = new Utility();
        }

        #endregion

        #region Public Functions

        #region Analytics Data function

        /// <summary>
        /// This is responsible for get analytics data
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <param name="tenantCode"></param>
        /// <returns></returns>
        public IList<AnalyticsData> GetAnalyticsData(AnalyticsSearchParameter searchParameter, string tenantCode)
        {
            IList<AnalyticsData> AnalyticsDatas = new List<AnalyticsData>();
            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                IList<View_SourceDataRecord> AnalyticsDataRecords = new List<View_SourceDataRecord>();
                string whereClause = this.WhereClauseGenerator(searchParameter, tenantCode);

                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {

                    if (searchParameter.PagingParameter.PageIndex > 0 && searchParameter.PagingParameter.PageSize > 0)
                    {
                        AnalyticsDataRecords = nISEntitiesDataContext.View_SourceDataRecord
                        .OrderBy(searchParameter.SortParameter.SortColumn + " " + searchParameter.SortParameter.SortOrder.ToString())
                        .Where(whereClause)
                        .Skip((searchParameter.PagingParameter.PageIndex - 1) * searchParameter.PagingParameter.PageSize)
                        .Take(searchParameter.PagingParameter.PageSize)
                        .ToList();
                    }
                    else
                    {
                        AnalyticsDataRecords = nISEntitiesDataContext.View_SourceDataRecord
                        .Where(whereClause)
                        .OrderBy(searchParameter.SortParameter.SortColumn + " " + searchParameter.SortParameter.SortOrder.ToString().ToLower())
                        .ToList();
                    }
                }

                AnalyticsDataRecords?.ToList().ForEach(item =>
                {
                    AnalyticsData data = new AnalyticsData();
                    data.Identifier = item.Id;
                    data.StatementId = item.StatementId;
                    data.CustomerId = item.CustomerId;
                    data.AccountId = item.AccountId;
                    data.PageWidgetId = item.PageWidgetId == null ? 0 : (long)item.PageWidgetId;
                    data.PageId = item.PageId == null ? 0 : (long)item.PageId;
                    data.WidgetId = item.WidgetId == null ? 0 : (long)item.WidgetId;
                    data.EventDate = DateTime.SpecifyKind((DateTime)item.EventDate, DateTimeKind.Utc);
                    data.EventType = item.EventType;
                    data.CustomerName = item.CustomerName;
                    data.PageName = item.PageName ?? "";
                    data.Widgetname = item.WidgetName ?? "";
                    data.PageTypeName = item.PageTypeName ?? "";
                    AnalyticsDatas.Add(data);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return AnalyticsDatas;
        }

        /// <summary>
        /// This method is responsible to get analytics data count
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <param name="tenantCode"></param>
        /// <returns>analytics data count</returns>
        public int GetAnalyticsDataCount(AnalyticsSearchParameter searchParameter, string tenantCode)
        {
            IList<AnalyticsData> AnalyticsDatas = new List<AnalyticsData>();
            int count = 0;

            try
            {
                this.SetAndValidateConnectionString(tenantCode);
                string whereClause = this.WhereClauseGenerator(searchParameter, tenantCode);
                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    count = nISEntitiesDataContext.View_SourceDataRecord.Where(whereClause).ToList().Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }

        /// <summary>
        /// This is responsible to save analytics data
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="tenantCode"></param>
        /// <returns></returns>
        public bool Save(IList<AnalyticsData> settings, string tenantCode)
        {
            IList<AnalyticsDataRecord> records = new List<AnalyticsDataRecord>();
            bool result;
            try
            {

                this.SetAndValidateConnectionString(tenantCode);
                StringBuilder queryString = new StringBuilder();
                queryString.Append(string.Join("or ", settings.Where(item => item.PageWidgetId > 0).Select(item => string.Format("Id.Equals({0}) ", item.PageWidgetId))));

                using (NISEntities nISEntitiesDataContext = new NISEntities(this.connectionString))
                {
                    IList<PageWidgetMapRecord> pageWidgeMap = new List<PageWidgetMapRecord>();
                    if (!string.IsNullOrEmpty(queryString.ToString()))
                    {
                        pageWidgeMap = nISEntitiesDataContext.PageWidgetMapRecords.Where(queryString.ToString()).ToList();
                    }
                    settings.ToList().ForEach(setting =>
                    {
                        AnalyticsDataRecord record = new AnalyticsDataRecord();
                        PageWidgetMapRecord map = new PageWidgetMapRecord();
                        if (setting.PageWidgetId > 0)
                        {
                            map = pageWidgeMap.Where(page => page.Id == setting.PageWidgetId)?.FirstOrDefault();
                            setting.PageId = map.PageId;
                            setting.WidgetId = map.ReferenceWidgetId;

                        }
                        record.StatementId = setting.StatementId;
                        record.CustomerId = setting.CustomerId;
                        record.AccountId = setting.AccountId != null ? setting.AccountId : "";
                        record.PageWidgetId = setting.PageWidgetId;
                        record.PageId = setting.PageId;
                        record.WidgetId = setting.WidgetId;
                        record.EventDate = setting.EventDate;
                        record.EventType = setting.EventType;
                        record.TenantCode = ModelConstant.DEFAULT_TENANT_CODE;
                        records.Add(record);

                    });

                    nISEntitiesDataContext.AnalyticsDataRecords.AddRange(records);
                    nISEntitiesDataContext.SaveChanges();
                    result = true;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion


        #endregion

        #region Private Methhod

        #region Get Connection String

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
                this.connectionString = this.configurationutility.GetConnectionString(ModelConstant.COMMON_SECTION, ModelConstant.NIS_CONNECTION_STRING, ModelConstant.CONFIGURATON_BASE_URL, ModelConstant.TENANT_CODE_KEY, tenantCode);
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

        /// <summary>
        /// Generate string for dynamic linq.
        /// </summary>
        /// <param name="searchParameter">Role search Parameters</param>
        /// <param name="tenantCode">The tenant code</param>
        /// <returns>
        /// Returns a string.
        /// </returns>
        private string WhereClauseGenerator(AnalyticsSearchParameter searchParameter, string tenantCode)
        {
            StringBuilder queryString = new StringBuilder();

            if (validationEngine.IsValidText(searchParameter.Pagename))
            {
                queryString.Append(string.Format("PageName.Contains(\"{0}\") and ", searchParameter.Pagename));
            }
            if (validationEngine.IsValidText(searchParameter.WidgetName))
            {
                queryString.Append(string.Format("WidgetName.Contains(\"{0}\") and ", searchParameter.WidgetName));
            }
            if (validationEngine.IsValidText(searchParameter.CustomerName))
            {
                queryString.Append(string.Format("CustomerName.Contains(\"{0}\") and ", searchParameter.CustomerName));
            }
            if (validationEngine.IsValidText(searchParameter.PageTypeName))
            {
                queryString.Append(string.Format("PageTypeName.Equals(\"{0}\") and ", searchParameter.PageTypeName));
            }
            if (this.validationEngine.IsValidDate(searchParameter.StartDate) && !this.validationEngine.IsValidDate(searchParameter.EndDate))
            {
                DateTime fromDateTime = DateTime.SpecifyKind(Convert.ToDateTime(searchParameter.StartDate), DateTimeKind.Utc);
                queryString.Append("EventDate >= DateTime(" + fromDateTime.Year + "," + fromDateTime.Month + "," + fromDateTime.Day + "," + fromDateTime.Hour + "," + fromDateTime.Minute + "," + fromDateTime.Second + ") and ");
            }

            if (this.validationEngine.IsValidDate(searchParameter.EndDate) && !this.validationEngine.IsValidDate(searchParameter.StartDate))
            {
                DateTime toDateTime = DateTime.SpecifyKind(Convert.ToDateTime(searchParameter.EndDate), DateTimeKind.Utc);
                queryString.Append("EventDate <= DateTime(" + toDateTime.Year + "," + toDateTime.Month + "," + toDateTime.Day + "," + toDateTime.Hour + "," + toDateTime.Minute + "," + toDateTime.Second + ") and ");
            }

            if (this.validationEngine.IsValidDate(searchParameter.StartDate) && this.validationEngine.IsValidDate(searchParameter.EndDate))
            {
                DateTime fromDateTime = DateTime.SpecifyKind(Convert.ToDateTime(searchParameter.StartDate), DateTimeKind.Utc);
                DateTime toDateTime = DateTime.SpecifyKind(Convert.ToDateTime(searchParameter.EndDate), DateTimeKind.Utc);

                queryString.Append("EventDate >= DateTime(" + fromDateTime.Year + "," + fromDateTime.Month + "," + fromDateTime.Day + "," + fromDateTime.Hour + "," + fromDateTime.Minute + "," + fromDateTime.Second + ") " +
                               "and EventDate <= DateTime(" + +toDateTime.Year + "," + toDateTime.Month + "," + toDateTime.Day + "," + toDateTime.Hour + "," + toDateTime.Minute + "," + toDateTime.Second + ") and ");
            }

            queryString.Append(string.Format("TenantCode.Equals(\"{0}\")", tenantCode));
            return queryString.ToString();
        }

        #endregion

    }
}