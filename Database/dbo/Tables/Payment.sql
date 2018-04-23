CREATE TABLE [dbo].[Payment]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Status] TINYINT NULL, 
    [Sum] MONEY NULL, 
    [Method] TINYINT NULL, 
    [OrderId] INT NULL, 
    CONSTRAINT [FK_Payment_ToOrder] FOREIGN KEY ([OrderId]) REFERENCES [Order]([Id])
)
