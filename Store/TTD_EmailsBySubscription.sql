﻿CREATE TABLE [NIS].[TTD_EmailsBySubscription]
(
	Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	Subscription NVARCHAR(100) NOT NULL,
	Emails BIGINT NOT NULL,
	[TenantCode] NVARCHAR(50) NOT NULL, 
)