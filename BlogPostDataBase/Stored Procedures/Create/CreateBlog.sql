ALTER PROCEDURE [dbo].[CreateBlog]
	@Title VARCHAR(500),
	@AuthorId INT,
	@Content NVARCHAR,
	@CreationDate DATE
AS
	BEGIN TRANSACTION
	BEGIN TRY

		INSERT INTO dbo.Blog(BlogTitle, AuthorId, BlogContent, CreationDate)
		VALUES (@Title, @AuthorId, @Content, @CreationDate)

		SELECT SCOPE_IDENTITY()

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
