-- Re-creates the complete DB schema
-- NOT to be run in a upgrade scenario! (For an upgrade, separate scrips should be created)
-- Run on empty database to create all objects
-- Run second time to drop and re-create all objects

USE Watches;
GO
SET NOCOUNT ON;
GO

-- First drop everything

-- Drop all foreign keys
IF OBJECT_ID (N'dbo.Watch', N'U') IS NOT NULL
	ALTER TABLE dbo.Watch DROP CONSTRAINT Watch_Brand_FK;
GO
IF OBJECT_ID (N'dbo.Watch', N'U') IS NOT NULL
	ALTER TABLE dbo.Watch DROP CONSTRAINT Watch_Movement_FK;
GO

-- Drop all tables
IF OBJECT_ID (N'dbo.Brand', N'U') IS NOT NULL
	DROP TABLE dbo.Brand;
GO
IF OBJECT_ID (N'dbo.Movement', N'U') IS NOT NULL
	DROP TABLE dbo.Movement;
GO
IF OBJECT_ID (N'dbo.Watch', N'U') IS NOT NULL
	DROP TABLE dbo.Watch;
GO

-- Now re-create everything

-- Brand
CREATE TABLE dbo.Brand
(
	Id BIGINT NOT NULL IDENTITY(1,1)
	, Ts ROWVERSION NOT NULL
	, Title NVARCHAR(255) NOT NULL
	, YearFounded INT NOT NULL
	, Description NVARCHAR(1000) NOT NULL
	, DateCreated DATE NOT NULL
);
GO
ALTER TABLE dbo.Brand ADD CONSTRAINT Brand_PK PRIMARY KEY CLUSTERED (Id);
GO

-- Movement
CREATE TABLE dbo.Movement
(
	Id BIGINT NOT NULL IDENTITY(1,1)
	, Ts ROWVERSION NOT NULL
	, Title NVARCHAR(255) NOT NULL
);
GO
ALTER TABLE dbo.Movement ADD CONSTRAINT Movement_PK PRIMARY KEY CLUSTERED (Id);
GO

-- Watch
CREATE TABLE dbo.Watch
(
	Id BIGINT NOT NULL IDENTITY(1,1)
	, Ts ROWVERSION NOT NULL
	, Model NVARCHAR(255) NOT NULL
	, Title NVARCHAR(255) NOT NULL
	, Gender INT NOT NULL
	, CaseSize INT NOT NULL
	, CaseMaterial INT NOT NULL
	, DateCreated DATE NOT NULL
	, BrandId BIGINT NOT NULL
	, MovementId BIGINT NOT NULL
);
GO
ALTER TABLE dbo.Watch ADD CONSTRAINT Watch_PK PRIMARY KEY CLUSTERED (Id);
GO
ALTER TABLE dbo.Watch ADD CONSTRAINT Watch_Brand_FK FOREIGN KEY (BrandId) REFERENCES dbo.Brand(Id) ON DELETE CASCADE;
GO
CREATE NONCLUSTERED INDEX Watch_Brand_IDX ON dbo.Watch (BrandId);
GO
ALTER TABLE dbo.Watch ADD CONSTRAINT Watch_Movement_FK FOREIGN KEY (MovementId) REFERENCES dbo.Movement(Id);
GO
CREATE NONCLUSTERED INDEX Watch_Movement_IDX ON dbo.Watch (MovementId);
GO