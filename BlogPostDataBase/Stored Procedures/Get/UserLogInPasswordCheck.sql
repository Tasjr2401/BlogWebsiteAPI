ALTER PROCEDURE [dbo].[UserLogInPasswordCheck]
	@Username VARCHAR(100)
AS
	SELECT Password, Salt, UserId FROM dbo.UserLogIn
	WHERE Username = @Username;
RETURN;
