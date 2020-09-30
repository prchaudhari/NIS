﻿CREATE TABLE [NIS].[MultiTenantUserAccessMap]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[UserId] BIGINT NOT NULL,
	[AssociatedTenantCode] NVARCHAR(50) NOT NULL,
	[OtherTenantCode] NVARCHAR(50) NOT NULL,
	[OtherTenantAccessRoleId] BIGINT NOT NULL,
	[IsActive] BIT NOT NULL,
	[IsDeleted] BIT NOT NULL, 
    [LastUpdatedBy] BIGINT NOT NULL, 
    [LastUpdatedDate] DATETIME NOT NULL
)
