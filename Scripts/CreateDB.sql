USE [master]
GO
CREATE DATABASE [MonitorDB] ON  PRIMARY 
( NAME = N'MonitorDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQL\MSSQL\DATA\MonitorDB.mdf' , SIZE = 316416KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MonitorDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQL\MSSQL\DATA\MonitorDB.ldf' , SIZE = 1964480KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [MonitorDB] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MonitorDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [MonitorDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [MonitorDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [MonitorDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [MonitorDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [MonitorDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [MonitorDB] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [MonitorDB] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [MonitorDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [MonitorDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [MonitorDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [MonitorDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [MonitorDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [MonitorDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [MonitorDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [MonitorDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [MonitorDB] SET  DISABLE_BROKER 
GO

ALTER DATABASE [MonitorDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [MonitorDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [MonitorDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [MonitorDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [MonitorDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [MonitorDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [MonitorDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [MonitorDB] SET  READ_WRITE 
GO

ALTER DATABASE [MonitorDB] SET RECOVERY FULL 
GO

ALTER DATABASE [MonitorDB] SET  MULTI_USER 
GO

ALTER DATABASE [MonitorDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [MonitorDB] SET DB_CHAINING OFF 
GO

