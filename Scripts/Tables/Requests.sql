CREATE TABLE dbo.Requests
(
	Id uniqueidentifier DEFAULT (newid()) NOT NULL PRIMARY KEY ,
	Created datetime2 DEFAULT (getdate()),
	Domain nvarchar(max),
	Url nvarchar(max),
	IpAdress nvarchar(50),
	Browser smallint,
	RequestId uniqueidentifier,
	SessionId uniqueidentifier,
	Refferer nvarchar(max)
);
