﻿CREATE TABLE [TenantManager].[Tenant]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TenantCode] NVARCHAR(MAX) NOT NULL, 
    [TenantName] NVARCHAR(MAX) NOT NULL,
	[TenantDescription] NVARCHAR(MAX) NULL,
    [TenantType] NVARCHAR(MAX) NULL,
	[TenantImage] NVARCHAR(MAX) NULL,
	[TenantDomainName] NVARCHAR(MAX) NOT NULL,
    [FirstName] NVARCHAR(MAX) NULL, 
    [LastName] NVARCHAR(MAX) NULL, 
    [ContactNumber] NVARCHAR(MAX) NOT NULL, 
    [EmailAddress] NVARCHAR(MAX) NOT NULL,
	[SecondaryContactName] NVARCHAR(MAX) NULL,
	[SecondaryContactNumber] NVARCHAR(MAX) NULL,
	[SecondaryEmailAddress] NVARCHAR(MAX) NULL, 
    [AddressLine1] NVARCHAR(MAX) NULL, 
    [AddressLine2] NVARCHAR(MAX) NULL,
	[TenantCity] NVARCHAR(MAX) NULL, 
	[TenantState] NVARCHAR(MAX) NULL, 
	[TenantCountry] NVARCHAR(MAX) NULL,  
    [PinCode] NVARCHAR(MAX) NULL, 
    [StartDate] DATE NULL, 
    [EndDate] DATE NULL,      
    [StorageAccount] NVARCHAR(MAX) NOT NULL, 
	[AccessToken] NVARCHAR(MAX) NOT NULL,
	[ApplicationURL] NVARCHAR(MAX) NULL, 
    [ApplicationModules] NVARCHAR(MAX) NULL, 
    [BillingEmailAddress] NVARCHAR(MAX) NULL,  
    [SecondaryLastName] NVARCHAR(MAX) NULL, 
    [BillingFirstName] NVARCHAR(MAX) NULL, 
    [BillingLastName] NVARCHAR(MAX) NULL, 
    [BillingContactNumber] NVARCHAR(MAX) NULL,
	[PanNumber] NVARCHAR(MAX) NULL, 
    [ServiceTax] NVARCHAR(MAX) NULL, 
    [IsPrimaryTenant] BIT NULL, 
    [ManageType] NVARCHAR(MAX) NULL,
	[ExternalCode] NVARCHAR(MAX) NULL,
	[AutheticationMode] NVARCHAR(MAX) NULL,
	[IsActive] BIT NOT NULL DEFAULT 1, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [ParentTenantCode] NVARCHAR(MAX) NULL, 
    [IsTenantConfigured] BIT NOT NULL
)
