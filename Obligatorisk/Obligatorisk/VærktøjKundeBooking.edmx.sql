
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/22/2020 14:59:46
-- Generated from EDMX file: P:\Aflevering i C#\Obligatorisk\Obligatorisk\VærktøjKundeBooking.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Værktøjsudlejning];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BookingVærktøj_Booking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookingVærktøj] DROP CONSTRAINT [FK_BookingVærktøj_Booking];
GO
IF OBJECT_ID(N'[dbo].[FK_BookingVærktøj_Værktøj]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookingVærktøj] DROP CONSTRAINT [FK_BookingVærktøj_Værktøj];
GO
IF OBJECT_ID(N'[dbo].[FK_KundeBooking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookingSet] DROP CONSTRAINT [FK_KundeBooking];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BookingSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BookingSet];
GO
IF OBJECT_ID(N'[dbo].[BookingVærktøj]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BookingVærktøj];
GO
IF OBJECT_ID(N'[dbo].[KundeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KundeSet];
GO
IF OBJECT_ID(N'[dbo].[VærktøjSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VærktøjSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'BookingSet'
CREATE TABLE [dbo].[BookingSet] (
    [BookingnummerID] int IDENTITY(1,1) NOT NULL,
    [Afhentningstidspunkt] datetime  NOT NULL,
    [AntalDøgn] int  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [KundeKundenummerID] int  NOT NULL
);
GO

-- Creating table 'KundeSet'
CREATE TABLE [dbo].[KundeSet] (
    [KundenummerID] int IDENTITY(1,1) NOT NULL,
    [Navn] nvarchar(max)  NOT NULL,
    [Adresse] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Brugernavn] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NULL
);
GO

-- Creating table 'VærktøjSet'
CREATE TABLE [dbo].[VærktøjSet] (
    [VærktøjID] int IDENTITY(1,1) NOT NULL,
    [Navn] nvarchar(max)  NOT NULL,
    [Beskrivelse] nvarchar(max)  NOT NULL,
    [Depositum] float  NOT NULL,
    [DøgnPris] float  NOT NULL
);
GO

-- Creating table 'BookingVærktøj'
CREATE TABLE [dbo].[BookingVærktøj] (
    [BookingSet_BookingnummerID] int  NOT NULL,
    [VærktøjSet_VærktøjID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [BookingnummerID] in table 'BookingSet'
ALTER TABLE [dbo].[BookingSet]
ADD CONSTRAINT [PK_BookingSet]
    PRIMARY KEY CLUSTERED ([BookingnummerID] ASC);
GO

-- Creating primary key on [KundenummerID] in table 'KundeSet'
ALTER TABLE [dbo].[KundeSet]
ADD CONSTRAINT [PK_KundeSet]
    PRIMARY KEY CLUSTERED ([KundenummerID] ASC);
GO

-- Creating primary key on [VærktøjID] in table 'VærktøjSet'
ALTER TABLE [dbo].[VærktøjSet]
ADD CONSTRAINT [PK_VærktøjSet]
    PRIMARY KEY CLUSTERED ([VærktøjID] ASC);
GO

-- Creating primary key on [BookingSet_BookingnummerID], [VærktøjSet_VærktøjID] in table 'BookingVærktøj'
ALTER TABLE [dbo].[BookingVærktøj]
ADD CONSTRAINT [PK_BookingVærktøj]
    PRIMARY KEY CLUSTERED ([BookingSet_BookingnummerID], [VærktøjSet_VærktøjID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [KundeKundenummerID] in table 'BookingSet'
ALTER TABLE [dbo].[BookingSet]
ADD CONSTRAINT [FK_KundeBooking]
    FOREIGN KEY ([KundeKundenummerID])
    REFERENCES [dbo].[KundeSet]
        ([KundenummerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KundeBooking'
CREATE INDEX [IX_FK_KundeBooking]
ON [dbo].[BookingSet]
    ([KundeKundenummerID]);
GO

-- Creating foreign key on [BookingSet_BookingnummerID] in table 'BookingVærktøj'
ALTER TABLE [dbo].[BookingVærktøj]
ADD CONSTRAINT [FK_BookingVærktøj_BookingSet]
    FOREIGN KEY ([BookingSet_BookingnummerID])
    REFERENCES [dbo].[BookingSet]
        ([BookingnummerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [VærktøjSet_VærktøjID] in table 'BookingVærktøj'
ALTER TABLE [dbo].[BookingVærktøj]
ADD CONSTRAINT [FK_BookingVærktøj_VærktøjSet]
    FOREIGN KEY ([VærktøjSet_VærktøjID])
    REFERENCES [dbo].[VærktøjSet]
        ([VærktøjID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BookingVærktøj_VærktøjSet'
CREATE INDEX [IX_FK_BookingVærktøj_VærktøjSet]
ON [dbo].[BookingVærktøj]
    ([VærktøjSet_VærktøjID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------