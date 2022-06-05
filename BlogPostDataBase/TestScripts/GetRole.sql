CREATE PROCEDURE [dbo].[GetRole]
	@RoleId INT = 1,
	@RoleIdTwo INT = 2
AS
	DECLARE @var VARCHAR(20);

	SET @var = (
		SELECT Role FROM dbo.Roles
		WHERE Id = @RoleId
	);

	SELECT Role FROM dbo.Roles
	WHERE Id = @RoleIdTwo;

RETURN;

EXEC dbo.GetRole;