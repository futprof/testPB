--CREATE DATABASE TestDB
USE TestDB

CREATE TABLE [Client](
[Id] NVARCHAR(36) PRIMARY KEY NOT NULL --Guid length
,[FirstName]  NVARCHAR(200) NOT NULL
,[LastName]  NVARCHAR(200) NOT NULL)

CREATE TABLE [IpAdress](
[Id] NVARCHAR(36) PRIMARY KEY NOT NULL 
,[Ip] NVARCHAR(32) not null)

CREATE TABLE [ClientsIPs](
[Id] NVARCHAR(36) PRIMARY KEY NOT NULL 
,[ClientId] NVARCHAR(36) NOT NULL 
,[IpId] NVARCHAR(36) NOT NULL)


CREATE TABLE [Department](
[Id] NVARCHAR(36) PRIMARY KEY NOT NULL 
,[Name] NVARCHAR(200) NOT NULL
,[Adress] NVARCHAR(MAX) NOT NULL)

CREATE TABLE [Currency](
[Id] NVARCHAR(3) PRIMARY KEY NOT NULL
,[Name] NVARCHAR(200) NOT NULL)

CREATE TABLE [Status](
[Id] TINYINT PRIMARY KEY NOT NULL
,[Name] NVARCHAR(100) NOT NULL)


CREATE TABLE [CashRequest](
[Id] NVARCHAR(36) PRIMARY KEY NOT NULL 
,[DepartmentId] NVARCHAR(36) NOT NULL
,[ClientId] NVARCHAR(36) NOT NULL)

CREATE TABLE [CashRequestDetails](
[Id] NVARCHAR(36) PRIMARY KEY NOT NULL
,[Date] DATETIMEOFFSET NOT NULL
,[CashRequestId] NVARCHAR(36) NOT NULL 
,[Amount] MONEY NOT NULL
,[CurrencyId] NVARCHAR(3) not null
,[StatusId] TINYINT NOT NULL)
GO

ALTER TABLE [ClientsIPs]
ADD CONSTRAINT FK_ClientsIPs_To_Clients
FOREIGN KEY ([ClientId]) REFERENCES [Client](Id)

ALTER TABLE [ClientsIPs]
ADD CONSTRAINT FK_ClientsIPs_To_IpAdress
FOREIGN KEY ([IpId]) REFERENCES [IpAdress](Id)

ALTER TABLE [CashRequest]
ADD CONSTRAINT FK_CashRequest_To_Client
FOREIGN KEY ([ClientId]) REFERENCES [Client](Id)

ALTER TABLE [CashRequestDetails]
ADD CONSTRAINT FK_CashRequestDetails_To_CashRequest
FOREIGN KEY ([CashRequestId]) REFERENCES [CashRequest](Id)

ALTER TABLE [CashRequestDetails]
ADD CONSTRAINT FK_CashRequestDetails_To_Currency
FOREIGN KEY ([CurrencyId]) REFERENCES [Currency](Id)

ALTER TABLE [CashRequestDetails]
ADD CONSTRAINT FK_CashRequestDetails_To_Status
FOREIGN KEY ([StatusId]) REFERENCES [Status](Id)

--Add some custom objects
INSERT [Client] VALUES ('1', 'Some', 'Person')
INSERT Department VALUES ('1', 'Some department', 'Some Adress')
INSERT [Currency] VALUES ('UAH', 'Гривна'),('USD', 'Доллар США')
INSERT [Status] values (1, 'InProcessing'),(2, 'Ready'),(3, 'Сompleted')
GO

CREATE OR ALTER FUNCTION GetCashRequestById
(
	@Id nvarchar(36)
)
RETURNS @returntable TABLE
(
	[Amount] money,
	[Currency] NVARCHAR(3),
	[Status]  NVARCHAR(100)
)
AS
BEGIN
    INSERT @returntable
	SELECT 
	LeftPart.[Amount],
	LeftPart.[CurrencyId] AS [Currency],
	RightPart.[Name] as [Status]
	FROM [CashRequestDetails] AS LeftPart
	LEFT JOIN [Status] as RightPart
	ON LeftPart.[StatusId] = RightPart.Id
	WHERE [CashRequestId] = @Id
RETURN
END
GO


CREATE OR ALTER FUNCTION GetCashRequests
(
	@ClientId nvarchar(36),
	@DepartmentAdress NVARCHAR(MAX)
)
RETURNS @returntable TABLE
(
	[Amount] money,
	[Currency] NVARCHAR(3),
	[Status]  NVARCHAR(100)
)
AS
BEGIN
    INSERT @returntable
	SELECT 
	LeftPart.[Amount],
	LeftPart.[CurrencyId] AS [Currency],
	RightPart.[Name] as [Status]
	FROM [CashRequestDetails] AS LeftPart
	LEFT JOIN [Status] as RightPart
	ON LeftPart.[StatusId] = RightPart.Id	 
	WHERE [CashRequestId] IN (	SELECT Id 
								FROM [CashRequest]
								WHERE [ClientId] = @ClientId
								AND [DepartmentId] IN (	SELECT Id 
														FROM Department
														WHERE Adress LIKE @DepartmentAdress))
								
RETURN
END
GO


