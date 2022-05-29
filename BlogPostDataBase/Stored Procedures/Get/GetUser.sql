CREATE PROCEDURE [dbo].[GetUser]
	@userId INT
AS
	SELECT Users.Firstname, Users.Lastname, Users.Username, Roles.Role FROM dbo.Users
	JOIN dbo.Roles ON Users.Role = Roles.Id
	WHERE Users.Id = @userId;
RETURN 0
