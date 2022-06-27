CREATE PROCEDURE [dbo].[spInventory_GetAll]
	
AS
begin 
	set nocount on;
	select [ProductId], [Quantity], [PurchasedPrice], [PurchasedDate] 
	from dbo.Inventory
end