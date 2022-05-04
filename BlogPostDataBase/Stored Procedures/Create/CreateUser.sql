ALTER PROCEDURE [dbo].[CreateUser]
	@Username VARCHAR(30),
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@Role INT,
	@Password VARCHAR(1000),
	@Salt VARBINARY(1000)
AS
	BEGIN TRANSACTION
	BEGIN TRY
		INSERT INTO dbo.Users (Username, FirstName, LastName, Role)
		VALUES (@Username, @Password, @LastName, @Role);
		

		DECLARE @userId AS INT
		SET @userId = SCOPE_IDENTITY();

		INSERT INTO dbo.UserLogIn (Username, Password, Salt, UserId)
		VALUES (@Username, @Password, @Salt, @userId);

		COMMIT
	END TRY
	BEGIN CATCH
		SELECT
			ERROR_NUMBER() AS ErrorNumber,
			ERROR_SEVERITY() AS ErrorSeverity,
			ERROR_STATE() AS ErrorState,
			ERROR_PROCEDURE() AS ErrorProcedure,
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage
		ROLLBACK
		END CATCH
RETURN;
