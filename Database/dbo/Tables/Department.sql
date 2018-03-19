CREATE TABLE [dbo].[Department]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NULL, 
    [HeadId] INT NOT NULL, 
    CONSTRAINT [FK_Departments_Users] FOREIGN KEY ([HeadId]) REFERENCES [User]([Id])
)
