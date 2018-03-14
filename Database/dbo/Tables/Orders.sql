CREATE TABLE [dbo].[Orders]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Number] NVARCHAR(10) NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [Status] VARCHAR(20) NOT NULL, 
    [Owner] VARCHAR(50) NOT NULL, 
    [DeliveryDriver] VARCHAR(50) NULL, 
    [DeliveryAddress] TEXT NULL, 
    [Receiver] VARCHAR(50) NULL, 
    [Comment] TEXT NULL, 
    [DeliveryDate] DATETIME NULL, 
    [PaymentStatus] VARCHAR(6) NULL
)
