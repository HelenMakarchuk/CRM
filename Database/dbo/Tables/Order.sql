CREATE TABLE [dbo].[Order]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Number] NVARCHAR(10) NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [Status] TINYINT NOT NULL, 
    [OwnerId] INT NOT NULL, 
    [DeliveryDriverId] INT NULL, 
    [DeliveryAddress] TEXT NULL, 
    [ReceiverId] INT NULL, 
    [Comment] TEXT NULL, 
    [DeliveryDate] DATETIME NULL, 
    CONSTRAINT [FK_Order_Owner] FOREIGN KEY ([OwnerId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_Order_DeliveryDriver] FOREIGN KEY ([DeliveryDriverId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([ReceiverId]) REFERENCES [Customer]([Id]) 
)
