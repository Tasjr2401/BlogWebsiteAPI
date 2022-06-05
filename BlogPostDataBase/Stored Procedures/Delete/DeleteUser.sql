CREATE PROCEDURE [dbo].[DeleteUser]
	@UserId INT
AS
	DECLARE @UserDeactive INT;
	SET @UserDeactive = (
		SELECT COUNT(Id) FROM dbo.DeactivatedUsers
		WHERE Id = @UserId
	);

	IF(@UserDeactive > 0)
		DELETE FROM dbo.Users
		WHERE Id = @UserId;
	ELSE
		RETURN 0
RETURN
