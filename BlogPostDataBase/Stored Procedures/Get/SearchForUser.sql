CREATE PROCEDURE [dbo].[SearchForUser]
	@Search VARCHAR
AS
	SELECT * FROM dbo.Users
	WHERE Username LIKE @Search OR FirstName LIKE @Search OR LastName LIKE @Search
RETURN 0
