CREATE PROCEDURE [dbo].[spInventory_Insert]	
	@ProductId int,
	@Quantity int,
	@PurchasedPrice money,
	@PurchasedDate datetime2
AS
begin
	set nocount on;
	insert into dbo.Inventory (ProductId, Quantity, PurchasedPrice, PurchasedDate)
	values (@ProductId,@Quantity,@PurchasedPrice,@PurchasedDate);
end