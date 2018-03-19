CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FullName] VARCHAR(50) NULL, 
    [Position] VARCHAR(50) NULL, 
    [DepartmentId] INT NULL, 
    [Email] NVARCHAR(60) NULL, 
    [Phone] NVARCHAR(20) NULL, 
    [BirthDate] DATE NULL, 
    [Gender] CHAR(1) NULL, 
    [Login] NVARCHAR(50) NULL, 
    [Password] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_Users_Departments] FOREIGN KEY ([DepartmentId]) REFERENCES [Department]([Id])
)
