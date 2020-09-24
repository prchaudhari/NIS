﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace nIS
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NISEntities : DbContext
    {
        public NISEntities()
            : base("name=NISEntities")
        {
        }
        public NISEntities(string connectionString)
                                                      : base(connectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<UserRecord> UserRecords { get; set; }
        public virtual DbSet<UserCredentialHistoryRecord> UserCredentialHistoryRecords { get; set; }
        public virtual DbSet<UserLoginRecord> UserLoginRecords { get; set; }
        public virtual DbSet<UserLoginActivityHistoryRecord> UserLoginActivityHistoryRecords { get; set; }
        public virtual DbSet<UserRoleMapRecord> UserRoleMapRecords { get; set; }
        public virtual DbSet<RolePrivilegeRecord> RolePrivilegeRecords { get; set; }
        public virtual DbSet<AssetRecord> AssetRecords { get; set; }
        public virtual DbSet<AssetLibraryRecord> AssetLibraryRecords { get; set; }
        public virtual DbSet<AssetPathSettingRecord> AssetPathSettingRecords { get; set; }
        public virtual DbSet<PageRecord> PageRecords { get; set; }
        public virtual DbSet<PageTypeRecord> PageTypeRecords { get; set; }
        public virtual DbSet<PageWidgetMapRecord> PageWidgetMapRecords { get; set; }
        public virtual DbSet<ScheduleRecord> ScheduleRecords { get; set; }
        public virtual DbSet<StatementRecord> StatementRecords { get; set; }
        public virtual DbSet<StatementPageMapRecord> StatementPageMapRecords { get; set; }
        public virtual DbSet<TenantRecord> TenantRecords { get; set; }
        public virtual DbSet<RoleRecord> RoleRecords { get; set; }
        public virtual DbSet<CityRecord> CityRecords { get; set; }
        public virtual DbSet<CountryRecord> CountryRecords { get; set; }
        public virtual DbSet<StateRecord> StateRecords { get; set; }
        public virtual DbSet<WidgetRecord> WidgetRecords { get; set; }
        public virtual DbSet<AssetSettingRecord> AssetSettingRecords { get; set; }
        public virtual DbSet<ScheduleRunHistoryRecord> ScheduleRunHistoryRecords { get; set; }
        public virtual DbSet<RenderEngineRecord> RenderEngineRecords { get; set; }
        public virtual DbSet<BatchDetailRecord> BatchDetailRecords { get; set; }
        public virtual DbSet<CustomerInfoRecord> CustomerInfoRecords { get; set; }
        public virtual DbSet<CustomerMediaRecord> CustomerMediaRecords { get; set; }
        public virtual DbSet<NewsAlertRecord> NewsAlertRecords { get; set; }
        public virtual DbSet<CustomerMasterRecord> CustomerMasterRecords { get; set; }
        public virtual DbSet<ImageRecord> ImageRecords { get; set; }
        public virtual DbSet<VideoRecord> VideoRecords { get; set; }
        public virtual DbSet<ReminderAndRecommendationRecord> ReminderAndRecommendationRecords { get; set; }
        public virtual DbSet<ScheduleLogRecord> ScheduleLogRecords { get; set; }
        public virtual DbSet<ScheduleLogDetailRecord> ScheduleLogDetailRecords { get; set; }
        public virtual DbSet<AccountTransactionRecord> AccountTransactionRecords { get; set; }
        public virtual DbSet<SavingTrendRecord> SavingTrendRecords { get; set; }
        public virtual DbSet<Top4IncomeSourcesRecord> Top4IncomeSourcesRecord { get; set; }
        public virtual DbSet<TransactionDetailRecord> TransactionDetailRecords { get; set; }
        public virtual DbSet<AccountMasterRecord> AccountMasterRecords { get; set; }
        public virtual DbSet<TenantConfigurationRecord> TenantConfigurationRecords { get; set; }
        public virtual DbSet<StatementAnalyticRecord> StatementAnalyticRecords { get; set; }
        public virtual DbSet<StatementMetadataRecord> StatementMetadataRecords { get; set; }
        public virtual DbSet<BatchMasterRecord> BatchMasterRecords { get; set; }
        public virtual DbSet<AnalyticsDataRecord> AnalyticsDataRecords { get; set; }
        public virtual DbSet<View_ScheduleRecord> View_ScheduleRecord { get; set; }
        public virtual DbSet<View_UserRecord> View_UserRecord { get; set; }
        public virtual DbSet<View_PageRecord> View_PageRecord { get; set; }
        public virtual DbSet<View_StatementDefinitionRecord> View_StatementDefinitionRecord { get; set; }
        public virtual DbSet<View_SourceDataRecord> View_SourceDataRecord { get; set; }
        public virtual DbSet<View_ScheduleLog> View_ScheduleLog { get; set; }
        public virtual DbSet<ContactTypeRecord> ContactTypeRecords { get; set; }
        public virtual DbSet<TenantContactRecord> TenantContactRecords { get; set; }
    }
}
