CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
begin set nocount on;
	select *
	from [dbo].[User]
	where Id = @Id;
end

