CREATE TABLE [dbo].[Departments]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NULL, 
    [HeadId] INT NULL, 
    CONSTRAINT [FK_Departments_Users] FOREIGN KEY ([HeadId]) REFERENCES [Users]([Id])
)
