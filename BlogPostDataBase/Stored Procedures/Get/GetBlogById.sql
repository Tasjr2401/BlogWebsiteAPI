CREATE PROCEDURE [dbo].[GetBlogById]
	@BlogId INT
AS
	SELECT * FROM dbo.Blog
	WHERE Blog.Id = @BlogId
RETURN 0
