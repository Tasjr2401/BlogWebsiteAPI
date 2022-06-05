CREATE PROCEDURE [dbo].[UsernameTakenCheck]
	@username VARCHAR(50)
AS
	SELECT COUNT(*) FROM dbo.Users
	WHERE Username = @username
RETURN;
