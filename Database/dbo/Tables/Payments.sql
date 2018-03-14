CREATE TABLE [dbo].[Payments]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Status] VARCHAR(6) NULL, 
    [Sum] MONEY NULL, 
    [Method] VARCHAR(10) NULL 
)
