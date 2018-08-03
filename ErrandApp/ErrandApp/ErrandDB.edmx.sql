
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/02/2018 13:29:17
-- Generated from EDMX file: C:\Users\stunji\Desktop\FROM TIVAS LAPTOP\FROM TIVAS LAPTOP\ErrandApp\ErrandApp\ErrandApp\ErrandDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [onesimpleapi];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Registerations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Registerations];
GO
IF OBJECT_ID(N'[dbo].[ErrandeeProfileImages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErrandeeProfileImages];
GO
IF OBJECT_ID(N'[dbo].[ErranderRegisterations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErranderRegisterations];
GO
IF OBJECT_ID(N'[dbo].[ErrandTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErrandTypes];
GO
IF OBJECT_ID(N'[dbo].[Requests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Requests];
GO
IF OBJECT_ID(N'[dbo].[ErrandeeAvailabilities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErrandeeAvailabilities];
GO
IF OBJECT_ID(N'[dbo].[ErrandeeLocations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErrandeeLocations];
GO
IF OBJECT_ID(N'[dbo].[ErranderLocations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErranderLocations];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Registerations'
CREATE TABLE [dbo].[Registerations] (
    [RegisterationId] int  NOT NULL,
    [FullName] nchar(100)  NULL,
    [Email] nchar(100)  NULL,
    [UserName] nchar(100)  NULL,
    [PhoneNo] nchar(100)  NULL,
    [Password] nchar(200)  NULL,
    [Location] nchar(100)  NULL,
    [Gender] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ErrandeeProfileImages'
CREATE TABLE [dbo].[ErrandeeProfileImages] (
    [ProfileImageId] int  NOT NULL,
    [ErrandeeID] nchar(100)  NULL,
    [ProfileImageName] nchar(100)  NULL,
    [Date] nvarchar(100)  NULL
);
GO

-- Creating table 'ErranderRegisterations'
CREATE TABLE [dbo].[ErranderRegisterations] (
    [ErranderId] int IDENTITY(1,1) NOT NULL,
    [FullName] nchar(200)  NULL,
    [Email] nchar(100)  NULL,
    [Password] nchar(100)  NULL,
    [Address] nchar(100)  NULL,
    [Gender] nchar(100)  NULL,
    [AddedOn] nchar(100)  NULL,
    [PhoneNo] nchar(100)  NULL
);
GO

-- Creating table 'ErrandTypes'
CREATE TABLE [dbo].[ErrandTypes] (
    [ErrandTypeId] int IDENTITY(1,1) NOT NULL,
    [ErrandName] nchar(100)  NULL,
    [Category] nchar(100)  NULL,
    [AddedOn] nchar(100)  NULL,
    [Cost] nchar(100)  NULL
);
GO

-- Creating table 'Requests'
CREATE TABLE [dbo].[Requests] (
    [RequestId] int IDENTITY(1,1) NOT NULL,
    [ErranderID] nchar(100)  NULL,
    [ErrandeeID] nchar(100)  NULL,
    [ErrandID] nchar(100)  NULL,
    [PickUpLocation] nchar(100)  NULL,
    [DropOffLocation] nchar(100)  NULL,
    [Status] nchar(100)  NULL,
    [Date] nchar(100)  NULL,
    [IsComplete] bit  NOT NULL
);
GO

-- Creating table 'ErrandeeAvailabilities'
CREATE TABLE [dbo].[ErrandeeAvailabilities] (
    [AvailabilityID] int IDENTITY(1,1) NOT NULL,
    [ErrandeeID] nchar(100)  NULL,
    [isAvailable] bit  NULL
);
GO

-- Creating table 'ErrandeeLocations'
CREATE TABLE [dbo].[ErrandeeLocations] (
    [LocationId] int IDENTITY(1,1) NOT NULL,
    [ErrandeeEmail] nchar(100)  NULL,
    [Lat] nchar(200)  NULL,
    [Long] nchar(200)  NULL
);
GO

-- Creating table 'ErranderLocations'
CREATE TABLE [dbo].[ErranderLocations] (
    [LocationId] int IDENTITY(1,1) NOT NULL,
    [ErranderEmail] nchar(100)  NULL,
    [Lat] nchar(200)  NULL,
    [Long] nchar(200)  NULL
);
GO

-- Creating table 'AccountDetails'
CREATE TABLE [dbo].[AccountDetails] (
    [AccountId] int IDENTITY(1,1) NOT NULL,
    [AccountName] nvarchar(max)  NOT NULL,
    [AccountNo] nvarchar(max)  NOT NULL,
    [BankName] nvarchar(max)  NOT NULL,
    [ErrandeeID] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Earnings'
CREATE TABLE [dbo].[Earnings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [ErrandeeID] nvarchar(max)  NOT NULL,
    [DateEarned] datetime  NOT NULL,
    [IsLiquidated] bit  NOT NULL
);
GO

-- Creating table 'ErrandeeRatings'
CREATE TABLE [dbo].[ErrandeeRatings] (
    [RatingId] int IDENTITY(1,1) NOT NULL,
    [ErrandeeID] nvarchar(max)  NOT NULL,
    [ErranderID] nvarchar(max)  NOT NULL,
    [Review] nvarchar(max)  NOT NULL,
    [Rating] int  NOT NULL,
    [RateDate] datetime  NOT NULL
);
GO

-- Creating table 'PaymentMethods'
CREATE TABLE [dbo].[PaymentMethods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsCash] bit  NOT NULL,
    [IsCard] bit  NOT NULL,
    [ErranderID] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CardDetails'
CREATE TABLE [dbo].[CardDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MaskedCardNo] nvarchar(max)  NOT NULL,
    [ErranderID] nvarchar(max)  NOT NULL,
    [CardType] nvarchar(max)  NOT NULL,
    [CardNo] nvarchar(max)  NOT NULL,
    [CVV] nvarchar(max)  NOT NULL,
    [Exp] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [RegisterationId] in table 'Registerations'
ALTER TABLE [dbo].[Registerations]
ADD CONSTRAINT [PK_Registerations]
    PRIMARY KEY CLUSTERED ([RegisterationId] ASC);
GO

-- Creating primary key on [ProfileImageId] in table 'ErrandeeProfileImages'
ALTER TABLE [dbo].[ErrandeeProfileImages]
ADD CONSTRAINT [PK_ErrandeeProfileImages]
    PRIMARY KEY CLUSTERED ([ProfileImageId] ASC);
GO

-- Creating primary key on [ErranderId] in table 'ErranderRegisterations'
ALTER TABLE [dbo].[ErranderRegisterations]
ADD CONSTRAINT [PK_ErranderRegisterations]
    PRIMARY KEY CLUSTERED ([ErranderId] ASC);
GO

-- Creating primary key on [ErrandTypeId] in table 'ErrandTypes'
ALTER TABLE [dbo].[ErrandTypes]
ADD CONSTRAINT [PK_ErrandTypes]
    PRIMARY KEY CLUSTERED ([ErrandTypeId] ASC);
GO

-- Creating primary key on [RequestId] in table 'Requests'
ALTER TABLE [dbo].[Requests]
ADD CONSTRAINT [PK_Requests]
    PRIMARY KEY CLUSTERED ([RequestId] ASC);
GO

-- Creating primary key on [AvailabilityID] in table 'ErrandeeAvailabilities'
ALTER TABLE [dbo].[ErrandeeAvailabilities]
ADD CONSTRAINT [PK_ErrandeeAvailabilities]
    PRIMARY KEY CLUSTERED ([AvailabilityID] ASC);
GO

-- Creating primary key on [LocationId] in table 'ErrandeeLocations'
ALTER TABLE [dbo].[ErrandeeLocations]
ADD CONSTRAINT [PK_ErrandeeLocations]
    PRIMARY KEY CLUSTERED ([LocationId] ASC);
GO

-- Creating primary key on [LocationId] in table 'ErranderLocations'
ALTER TABLE [dbo].[ErranderLocations]
ADD CONSTRAINT [PK_ErranderLocations]
    PRIMARY KEY CLUSTERED ([LocationId] ASC);
GO

-- Creating primary key on [AccountId] in table 'AccountDetails'
ALTER TABLE [dbo].[AccountDetails]
ADD CONSTRAINT [PK_AccountDetails]
    PRIMARY KEY CLUSTERED ([AccountId] ASC);
GO

-- Creating primary key on [Id] in table 'Earnings'
ALTER TABLE [dbo].[Earnings]
ADD CONSTRAINT [PK_Earnings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [RatingId] in table 'ErrandeeRatings'
ALTER TABLE [dbo].[ErrandeeRatings]
ADD CONSTRAINT [PK_ErrandeeRatings]
    PRIMARY KEY CLUSTERED ([RatingId] ASC);
GO

-- Creating primary key on [Id] in table 'PaymentMethods'
ALTER TABLE [dbo].[PaymentMethods]
ADD CONSTRAINT [PK_PaymentMethods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CardDetails'
ALTER TABLE [dbo].[CardDetails]
ADD CONSTRAINT [PK_CardDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------