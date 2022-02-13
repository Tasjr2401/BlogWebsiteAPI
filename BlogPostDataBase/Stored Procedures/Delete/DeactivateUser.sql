CREATE PROCEDURE [dbo].[DeactivateUser]
	@UserId INT,
	@DeactivatorId INT,
	@Date DATE,
	@Reason VARCHAR(500)
AS
	INSERT INTO dbo.DeactivatedUsers (UserId, DeactivatorId, DateOfDeactivation, ReasonForDeactivation)
	VALUES (@UserId, @DeactivatorId, @Date, @Reason)
RETURN
