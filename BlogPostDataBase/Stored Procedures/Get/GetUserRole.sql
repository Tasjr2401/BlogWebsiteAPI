CREATE PROCEDURE [dbo].[GetUserRole]
	@UserId INT
AS
	SELECT Role FROM dbo.Users
	WHERE Id = @UserId
RETURN
