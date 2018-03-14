CREATE TABLE [dbo].[Customers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(10) NULL, 
    [Phone] NCHAR(10) NULL, 
    [Email] NCHAR(10) NULL
)
