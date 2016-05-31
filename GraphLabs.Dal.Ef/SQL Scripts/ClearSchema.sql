
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/17/2014 23:07:17
-- Generated from EDMX file: D:\C#\GRAPH\trunk\GraphLabs.Dal.Ef\GraphLabsDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [gl_unit_tests];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserNews]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[News] DROP CONSTRAINT [FK_UserNews];
GO
IF OBJECT_ID(N'[dbo].[FK_ResultAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentActions] DROP CONSTRAINT [FK_ResultAction];
GO
IF OBJECT_ID(N'[dbo].[FK_TestQuestionAnswerVariant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AnswerVariants] DROP CONSTRAINT [FK_TestQuestionAnswerVariant];
GO
IF OBJECT_ID(N'[dbo].[FK_VariantTestQuestion_LabVariant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VariantTestQuestion] DROP CONSTRAINT [FK_VariantTestQuestion_LabVariant];
GO
IF OBJECT_ID(N'[dbo].[FK_VariantTestQuestion_TestQuestion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VariantTestQuestion] DROP CONSTRAINT [FK_VariantTestQuestion_TestQuestion];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sessions] DROP CONSTRAINT [FK_UserSession];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_Student] DROP CONSTRAINT [FK_GroupStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Results] DROP CONSTRAINT [FK_StudentResult];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskTaskVariant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskVariants] DROP CONSTRAINT [FK_TaskTaskVariant];
GO
IF OBJECT_ID(N'[dbo].[FK_LabVariantTaskVariant_LabVariant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LabVariantTaskVariant] DROP CONSTRAINT [FK_LabVariantTaskVariant_LabVariant];
GO
IF OBJECT_ID(N'[dbo].[FK_LabVariantTaskVariant_TaskVariant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LabVariantTaskVariant] DROP CONSTRAINT [FK_LabVariantTaskVariant_TaskVariant];
GO
IF OBJECT_ID(N'[dbo].[FK_LabWorkLabVariant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LabVariants] DROP CONSTRAINT [FK_LabWorkLabVariant];
GO
IF OBJECT_ID(N'[dbo].[FK_LabVariantResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Results] DROP CONSTRAINT [FK_LabVariantResult];
GO
IF OBJECT_ID(N'[dbo].[FK_LabWorkLabEntry]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LabEntries] DROP CONSTRAINT [FK_LabWorkLabEntry];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskLabEntry]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LabEntries] DROP CONSTRAINT [FK_TaskLabEntry];
GO
IF OBJECT_ID(N'[dbo].[FK_LabWorkGroup_LabWork]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LabWorkGroup] DROP CONSTRAINT [FK_LabWorkGroup_LabWork];
GO
IF OBJECT_ID(N'[dbo].[FK_LabWorkGroup_Group]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LabWorkGroup] DROP CONSTRAINT [FK_LabWorkGroup_Group];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentActions] DROP CONSTRAINT [FK_TaskAction];
GO
IF OBJECT_ID(N'[dbo].[FK_TestQuestionCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestQuestions] DROP CONSTRAINT [FK_TestQuestionCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskTaskData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_TaskTaskData];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_inherits_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_Student] DROP CONSTRAINT [FK_Student_inherits_User];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskVariantTaskResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskResults] DROP CONSTRAINT [FK_TaskVariantTaskResult];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[News]', 'U') IS NOT NULL
    DROP TABLE [dbo].[News];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];
GO
IF OBJECT_ID(N'[dbo].[Results]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Results];
GO
IF OBJECT_ID(N'[dbo].[StudentActions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentActions];
GO
IF OBJECT_ID(N'[dbo].[LabWorks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LabWorks];
GO
IF OBJECT_ID(N'[dbo].[Tasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tasks];
GO
IF OBJECT_ID(N'[dbo].[LabVariants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LabVariants];
GO
IF OBJECT_ID(N'[dbo].[TestQuestions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestQuestions];
GO
IF OBJECT_ID(N'[dbo].[AnswerVariants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AnswerVariants];
GO
IF OBJECT_ID(N'[dbo].[Sessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sessions];
GO
IF OBJECT_ID(N'[dbo].[Settings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Settings];
GO
IF OBJECT_ID(N'[dbo].[TaskVariants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskVariants];
GO
IF OBJECT_ID(N'[dbo].[LabEntries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LabEntries];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[TaskDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskDatas];
GO
IF OBJECT_ID(N'[dbo].[Users_Student]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users_Student];
GO
IF OBJECT_ID(N'[dbo].[VariantTestQuestion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VariantTestQuestion];
GO
IF OBJECT_ID(N'[dbo].[LabVariantTaskVariant]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LabVariantTaskVariant];
GO
IF OBJECT_ID(N'[dbo].[LabWorkGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LabWorkGroup];
GO
IF OBJECT_ID(N'[dbo].[TaskResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskResults];
GO