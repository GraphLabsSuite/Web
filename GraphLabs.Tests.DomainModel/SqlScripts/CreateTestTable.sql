SET QUOTED_IDENTIFIER OFF;
GO
USE [gl_unit_tests];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

CREATE TABLE [dbo].[TestTable] (
    [Id] bigint NOT NULL,
);
GO

ALTER TABLE [dbo].[TestTable]
ADD CONSTRAINT [PK_TestTable]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO