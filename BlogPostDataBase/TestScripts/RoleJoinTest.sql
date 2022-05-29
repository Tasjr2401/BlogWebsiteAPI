SELECT Users.Firstname, Users.Lastname, Users.Username, Roles.Role FROM dbo.Users
	JOIN dbo.Roles ON Users.Role = Roles.Id