CREATE PROCEDURE [dbo].[UserLogInPasswordCheck]
	@Username VARCHAR(100)
AS
	SELECT Password FROM dbo.UserLogIn
	WHERE Username = @Username;
RETURN;
