INSERT INTO dbo.OrderStatuses (Id, Name) VALUES(0, 'New')
INSERT INTO dbo.OrderStatuses (Id, Name) VALUES(1, 'Confirmed')
INSERT INTO dbo.OrderStatuses (Id, Name) VALUES(2, 'Closed')

INSERT INTO dbo.Roles (Id, Name) VALUES(0, 'Administrator')
INSERT INTO dbo.Roles (Id, Name) VALUES(1, 'Order Manager')
INSERT INTO dbo.Roles (Id, Name) VALUES(2, 'Product Manager')
INSERT INTO dbo.Roles (Id, Name) VALUES(3, 'Customer Manager')

INSERT INTO dbo.Products (Code, Name, Price) VALUES('C1', 'P1', 12.22)
INSERT INTO dbo.Products (Code, Name, Price) VALUES('C2', 'P2', 12.22)

INSERT INTO dbo.Customers (Name, Mail) VALUES('C1', 'xx1@wp.pl')
INSERT INTO dbo.Customers (Name, Mail) VALUES('C2', 'xx2@wp.pl')

DECLARE @OutputTbl TABLE (Id INT)
INSERT INTO dbo.Users (Login, FirstName, LastName) OUTPUT INSERTED.ID INTO @OutputTbl VALUES ('impaq\tjak', 'Tomasz', 'Janicki')
INSERT INTO dbo.UsersRoles (UserId, RoleId) SELECT o.Id, r.Id FROM dbo.Roles r CROSS JOIN @OutputTbl o