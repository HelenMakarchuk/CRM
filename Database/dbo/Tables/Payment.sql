CREATE TABLE [dbo].[Payment]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Status] VARCHAR(6) NULL, 
    [Sum] MONEY NULL, 
    [Method] VARCHAR(10) NULL, 
	CONSTRAINT [FK_Payments_Orders] FOREIGN KEY ([Id]) REFERENCES [Order]([Id])
)
