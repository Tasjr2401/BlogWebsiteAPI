CREATE PROCEDURE [dbo].[GetUserId]
	@Username VARCHAR(50)
AS
	SELECT Id FROM dbo.Users
	WHERE Username = @Username;
RETURN
