CREATE PROCEDURE [dbo].[UserLogInPasswordCheck]
	@Username VARCHAR(100)
AS
	SELECT Password, salt, UserId FROM dbo.UserLogIn
	WHERE Username = @Username;
RETURN;
