CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [ProductName] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [RetailPrice]   Money   not null,
    [QuantityInStock] INT NOT NULL DEFAULT 1,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [LastModified] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [IsTaxable] Bit not null default 1
)
