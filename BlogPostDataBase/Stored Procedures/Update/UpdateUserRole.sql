CREATE PROCEDURE [dbo].[UpdateUserRole]
	@PromoterId INT,
	@PromoteeId INT,
	@NewRole VARCHAR(30),
	@DateOfChange DATE
AS
	DECLARE @RoleId INT;
	SET @RoleId = (
		SELECT Id FROM dbo.Roles
		WHERE Role = @NewRole
	);

	DECLARE @PreviousRoleId INT;
	SET @PreviousRoleId = (
		SELECT Role FROM dbo.Users
		WHERE Id = @PromoteeId
	);

	UPDATE dbo.Users
	SET Role = @NewRole
	WHERE Id = @PromoteeId;

	INSERT INTO dbo.RoleUpdate (PromoterId, PromoteeId, PreviousRole, NewRole, DateOfChange)
	VALUES (@PromoterId, @PromoteeId, @PreviousRoleId, @NewRole, @DateOfChange)
RETURN
