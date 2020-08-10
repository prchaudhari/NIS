﻿CREATE TABLE [NIS].[TenantConfiguration]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NCHAR(10) NOT NULL,
    [Description] NVARCHAR(500) NULL, 
    [InputDataSourcePath] NVARCHAR(MAX) NULL, 
    [OutputHTMLPath] NVARCHAR(MAX) NULL, 
    [OutputPDFPath] NVARCHAR(MAX) NULL,
    [TenantCode] [nvarchar](50) NOT NULL

)
