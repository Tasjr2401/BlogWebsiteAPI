CREATE PROCEDURE [dbo].[CreateUser]
	@Username VARCHAR(30),
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@Role INT,
	@Password VARCHAR(1000),
	@Salt VARBINARY(1000)
AS
	BEGIN TRANSACTION
	BEGIN TRY
		DECLARE @userId AS INT
		INSERT INTO dbo.Users (Username, FirstName, LastName, Role)
		VALUES (@Username, @Password, @LastName, @Role);

		SET @userId = (
			SELECT Id FROM dbo.Users
			WHERE Username = @Username AND FirstName = @FirstName
		);

		INSERT INTO dbo.UserLogIn (Username, Password, Salt, UserId)
		VALUES (@Username, @Password, @Salt, @userId);
	END TRY
	BEGIN CATCH
		SELECT
			ERROR_NUMBER() AS ErrorNumber,
			ERROR_SEVERITY() AS ErrorSeverity,
			ERROR_STATE() AS ErrorState,
			ERROR_PROCEDURE() AS ErrorProcedure,
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage
		ROLLBACK TRANSACTION
		END CATCH
RETURN;
