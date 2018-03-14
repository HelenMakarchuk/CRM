CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [FullName] VARCHAR(50) NULL, 
    [Position] VARCHAR(50) NULL, 
    [Department] VARCHAR(50) NULL, 
    [Email] NVARCHAR(60) NULL, 
    [Phone] NVARCHAR(20) NULL, 
    [BirthDate] DATE NULL, 
    [Gender] CHAR(1) NULL
)
