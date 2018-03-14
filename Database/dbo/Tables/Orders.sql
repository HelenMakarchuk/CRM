CREATE TABLE [dbo].[Orders]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Number] NVARCHAR(10) NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [Status] VARCHAR(20) NOT NULL, 
    [OwnerId] INT NOT NULL, 
    [DeliveryDriverId] INT NULL, 
    [DeliveryAddress] TEXT NULL, 
    [ReceiverId] INT NULL, 
    [Comment] TEXT NULL, 
    [DeliveryDate] DATETIME NULL, 
    [PaymentId] INT NULL, 
    CONSTRAINT [FK_Orders_Owners] FOREIGN KEY ([OwnerId]) REFERENCES [Users]([Id]), 
    CONSTRAINT [FK_Orders_DeliveryDrivers] FOREIGN KEY ([DeliveryDriverId]) REFERENCES [Users]([Id]), 
    CONSTRAINT [FK_Orders_Customers] FOREIGN KEY ([ReceiverId]) REFERENCES [Customers]([Id]), 
    CONSTRAINT [FK_Orders_Payments] FOREIGN KEY ([PaymentId]) REFERENCES [Payments]([Id])
)
