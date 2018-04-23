CREATE TABLE [dbo].[Department]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [HeadId] INT NULL, 
    [Phone] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_Department_ToHead] FOREIGN KEY ([HeadId]) REFERENCES [User]([Id])
)
