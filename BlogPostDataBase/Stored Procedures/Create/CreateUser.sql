CREATE PROCEDURE [dbo].[CreateUser]
	@Username VARCHAR(30),
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@Role INT,
	@Password VARCHAR(1000),
	@Salt VARBINARY(1000)
AS
	DECLARE @userId AS INT
	INSERT INTO dbo.Users (Username, FirstName, LastName, Role)
	VALUES (@Username, @Password, @LastName, @Role);

	SET @userId = (
		SELECT Id FROM dbo.Users
		WHERE Username = @Username AND FirstName = @FirstName
	);

	INSERT INTO dbo.UserLogIn (Username, Password, Salt, UserId)
	VALUES (@Username, @Password, @Salt, @userId);

RETURN;
