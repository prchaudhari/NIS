﻿CREATE TABLE [NIS].[City]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL,
    [StateId]BIGINT NOT NULL,
	[IsActive] BIT NOT NULL,
	[IsDeleted] BIT NOT NULL
)
