USE MonitorDB;
GO

CREATE PROCEDURE InsertInfo 
	@Domain nvarchar(max),
	@Url nvarchar(max),
	@IpAdress nvarchar(50),
	@Browser smallint,
	@RequestId uniqueidentifier,
	@SessionId uniqueidentifier,
	@Refferer nvarchar(max)
AS
	INSERT Requests(Domain, Url, IpAdress, Browser, RequestId, SessionId, Refferer)
		VALUES (@Domain, @Url, @IpAdress, @Browser, @RequestId, @SessionId, @Refferer);