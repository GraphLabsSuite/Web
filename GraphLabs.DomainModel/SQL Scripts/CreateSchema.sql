
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/15/2014 10:28:59
-- Generated from EDMX file: C:\Users\SanCom\Documents\Visual Studio 2012\Projects\trunk\GraphLabs.DomainModel\GraphLabsDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [gltst];
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
    ALTER TABLE [dbo].[Actions] DROP CONSTRAINT [FK_ResultAction];
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
    ALTER TABLE [dbo].[Actions] DROP CONSTRAINT [FK_TaskAction];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryTestQuestion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestQuestions] DROP CONSTRAINT [FK_CategoryTestQuestion];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_inherits_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_Student] DROP CONSTRAINT [FK_Student_inherits_User];
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
IF OBJECT_ID(N'[dbo].[Actions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Actions];
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

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'News'
CREATE TABLE [dbo].[News] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(1000)  NOT NULL,
    [PublicationTime] datetime  NOT NULL,
    [Title] nvarchar(100)  NOT NULL,
    [LastModificationTime] datetime  NULL,
    [User_Id] bigint  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Surname] nvarchar(30)  NOT NULL,
    [Name] nvarchar(30)  NOT NULL,
    [FatherName] nvarchar(30)  NULL,
    [PasswordHash] nvarchar(100)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [Role] int  NOT NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Number] int  NOT NULL,
    [IsOpen] bit  NOT NULL,
    [FirstYear] int  NOT NULL
);
GO

-- Creating table 'Results'
CREATE TABLE [dbo].[Results] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Mode] int  NOT NULL,
    [StartDateTime] datetime  NOT NULL,
    [Grade] int  NULL,
    [Student_Id] bigint  NOT NULL,
    [LabVariant_Id] bigint  NOT NULL
);
GO

-- Creating table 'Actions'
CREATE TABLE [dbo].[Actions] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(100)  NOT NULL,
    [Penalty] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [Result_Id] bigint  NOT NULL,
    [Task_Id] bigint  NOT NULL
);
GO

-- Creating table 'LabWorks'
CREATE TABLE [dbo].[LabWorks] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [AcquaintanceFrom] datetime  NULL,
    [AcquaintanceTill] datetime  NULL
);
GO

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Xap] varbinary(max)  NOT NULL,
    [VariantGenerator] varbinary(max)  NULL,
    [Sections] nvarchar(100)  NULL,
    [Version] nvarchar(20)  NOT NULL,
    [Note] nvarchar(max)  NULL
);
GO

-- Creating table 'LabVariants'
CREATE TABLE [dbo].[LabVariants] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Number] nvarchar(max)  NOT NULL,
    [Version] bigint  NOT NULL,
    [IntroducingVariant] bit  NOT NULL,
    [LabWork_Id] bigint  NOT NULL
);
GO

-- Creating table 'TestQuestions'
CREATE TABLE [dbo].[TestQuestions] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(100)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [TypeOfQuestion] nvarchar(max)  NOT NULL,
    [Section] nvarchar(max)  NOT NULL,
    [CategoryId] bigint  NOT NULL
);
GO

-- Creating table 'AnswerVariants'
CREATE TABLE [dbo].[AnswerVariants] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Answer] nvarchar(50)  NOT NULL,
    [IsCorrect] nvarchar(10)  NOT NULL,
    [TestQuestion_Id] bigint  NOT NULL
);
GO

-- Creating table 'Sessions'
CREATE TABLE [dbo].[Sessions] (
    [CreationTime] datetime  NOT NULL,
    [LastAction] datetime  NOT NULL,
    [Guid] uniqueidentifier  NOT NULL,
    [IP] nvarchar(max)  NOT NULL,
    [User_Id] bigint  NOT NULL
);
GO

-- Creating table 'Settings'
CREATE TABLE [dbo].[Settings] (
    [Id] bigint  NOT NULL,
    [SystemDate] datetime  NOT NULL,
    [DefaultVariantGenerator] varbinary(max)  NULL
);
GO

-- Creating table 'TaskVariants'
CREATE TABLE [dbo].[TaskVariants] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Number] nvarchar(50)  NOT NULL,
    [GeneratorVersion] nvarchar(max)  NOT NULL,
    [Data] varbinary(max)  NOT NULL,
    [Version] bigint  NOT NULL,
    [Task_Id] bigint  NOT NULL
);
GO

-- Creating table 'LabEntries'
CREATE TABLE [dbo].[LabEntries] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Order] int  NOT NULL,
    [LabWork_Id] bigint  NOT NULL,
    [Task_Id] bigint  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Users_Student'
CREATE TABLE [dbo].[Users_Student] (
    [IsVerified] bit  NOT NULL,
    [IsDismissed] bit  NOT NULL,
    [Id] bigint  NOT NULL,
    [Group_Id] bigint  NULL
);
GO

-- Creating table 'VariantTestQuestion'
CREATE TABLE [dbo].[VariantTestQuestion] (
    [LabVariant_Id] bigint  NOT NULL,
    [TestQuestions_Id] bigint  NOT NULL
);
GO

-- Creating table 'LabVariantTaskVariant'
CREATE TABLE [dbo].[LabVariantTaskVariant] (
    [LabVariantTaskVariant_TaskVariant_Id] bigint  NOT NULL,
    [TaskVariants_Id] bigint  NOT NULL
);
GO

-- Creating table 'LabWorkGroup'
CREATE TABLE [dbo].[LabWorkGroup] (
    [LabWorks_Id] bigint  NOT NULL,
    [Groups_Id] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [PK_News]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Results'
ALTER TABLE [dbo].[Results]
ADD CONSTRAINT [PK_Results]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Actions'
ALTER TABLE [dbo].[Actions]
ADD CONSTRAINT [PK_Actions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LabWorks'
ALTER TABLE [dbo].[LabWorks]
ADD CONSTRAINT [PK_LabWorks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [PK_Tasks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LabVariants'
ALTER TABLE [dbo].[LabVariants]
ADD CONSTRAINT [PK_LabVariants]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TestQuestions'
ALTER TABLE [dbo].[TestQuestions]
ADD CONSTRAINT [PK_TestQuestions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AnswerVariants'
ALTER TABLE [dbo].[AnswerVariants]
ADD CONSTRAINT [PK_AnswerVariants]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Guid] in table 'Sessions'
ALTER TABLE [dbo].[Sessions]
ADD CONSTRAINT [PK_Sessions]
    PRIMARY KEY CLUSTERED ([Guid] ASC);
GO

-- Creating primary key on [Id] in table 'Settings'
ALTER TABLE [dbo].[Settings]
ADD CONSTRAINT [PK_Settings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaskVariants'
ALTER TABLE [dbo].[TaskVariants]
ADD CONSTRAINT [PK_TaskVariants]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LabEntries'
ALTER TABLE [dbo].[LabEntries]
ADD CONSTRAINT [PK_LabEntries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users_Student'
ALTER TABLE [dbo].[Users_Student]
ADD CONSTRAINT [PK_Users_Student]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LabVariant_Id], [TestQuestions_Id] in table 'VariantTestQuestion'
ALTER TABLE [dbo].[VariantTestQuestion]
ADD CONSTRAINT [PK_VariantTestQuestion]
    PRIMARY KEY NONCLUSTERED ([LabVariant_Id], [TestQuestions_Id] ASC);
GO

-- Creating primary key on [LabVariantTaskVariant_TaskVariant_Id], [TaskVariants_Id] in table 'LabVariantTaskVariant'
ALTER TABLE [dbo].[LabVariantTaskVariant]
ADD CONSTRAINT [PK_LabVariantTaskVariant]
    PRIMARY KEY NONCLUSTERED ([LabVariantTaskVariant_TaskVariant_Id], [TaskVariants_Id] ASC);
GO

-- Creating primary key on [LabWorks_Id], [Groups_Id] in table 'LabWorkGroup'
ALTER TABLE [dbo].[LabWorkGroup]
ADD CONSTRAINT [PK_LabWorkGroup]
    PRIMARY KEY NONCLUSTERED ([LabWorks_Id], [Groups_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [FK_UserNews]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserNews'
CREATE INDEX [IX_FK_UserNews]
ON [dbo].[News]
    ([User_Id]);
GO

-- Creating foreign key on [Result_Id] in table 'Actions'
ALTER TABLE [dbo].[Actions]
ADD CONSTRAINT [FK_ResultAction]
    FOREIGN KEY ([Result_Id])
    REFERENCES [dbo].[Results]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ResultAction'
CREATE INDEX [IX_FK_ResultAction]
ON [dbo].[Actions]
    ([Result_Id]);
GO

-- Creating foreign key on [TestQuestion_Id] in table 'AnswerVariants'
ALTER TABLE [dbo].[AnswerVariants]
ADD CONSTRAINT [FK_TestQuestionAnswerVariant]
    FOREIGN KEY ([TestQuestion_Id])
    REFERENCES [dbo].[TestQuestions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TestQuestionAnswerVariant'
CREATE INDEX [IX_FK_TestQuestionAnswerVariant]
ON [dbo].[AnswerVariants]
    ([TestQuestion_Id]);
GO

-- Creating foreign key on [LabVariant_Id] in table 'VariantTestQuestion'
ALTER TABLE [dbo].[VariantTestQuestion]
ADD CONSTRAINT [FK_VariantTestQuestion_LabVariant]
    FOREIGN KEY ([LabVariant_Id])
    REFERENCES [dbo].[LabVariants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TestQuestions_Id] in table 'VariantTestQuestion'
ALTER TABLE [dbo].[VariantTestQuestion]
ADD CONSTRAINT [FK_VariantTestQuestion_TestQuestion]
    FOREIGN KEY ([TestQuestions_Id])
    REFERENCES [dbo].[TestQuestions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_VariantTestQuestion_TestQuestion'
CREATE INDEX [IX_FK_VariantTestQuestion_TestQuestion]
ON [dbo].[VariantTestQuestion]
    ([TestQuestions_Id]);
GO

-- Creating foreign key on [User_Id] in table 'Sessions'
ALTER TABLE [dbo].[Sessions]
ADD CONSTRAINT [FK_UserSession]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSession'
CREATE INDEX [IX_FK_UserSession]
ON [dbo].[Sessions]
    ([User_Id]);
GO

-- Creating foreign key on [Group_Id] in table 'Users_Student'
ALTER TABLE [dbo].[Users_Student]
ADD CONSTRAINT [FK_GroupStudent]
    FOREIGN KEY ([Group_Id])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupStudent'
CREATE INDEX [IX_FK_GroupStudent]
ON [dbo].[Users_Student]
    ([Group_Id]);
GO

-- Creating foreign key on [Student_Id] in table 'Results'
ALTER TABLE [dbo].[Results]
ADD CONSTRAINT [FK_StudentResult]
    FOREIGN KEY ([Student_Id])
    REFERENCES [dbo].[Users_Student]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentResult'
CREATE INDEX [IX_FK_StudentResult]
ON [dbo].[Results]
    ([Student_Id]);
GO

-- Creating foreign key on [Task_Id] in table 'TaskVariants'
ALTER TABLE [dbo].[TaskVariants]
ADD CONSTRAINT [FK_TaskTaskVariant]
    FOREIGN KEY ([Task_Id])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskTaskVariant'
CREATE INDEX [IX_FK_TaskTaskVariant]
ON [dbo].[TaskVariants]
    ([Task_Id]);
GO

-- Creating foreign key on [LabVariantTaskVariant_TaskVariant_Id] in table 'LabVariantTaskVariant'
ALTER TABLE [dbo].[LabVariantTaskVariant]
ADD CONSTRAINT [FK_LabVariantTaskVariant_LabVariant]
    FOREIGN KEY ([LabVariantTaskVariant_TaskVariant_Id])
    REFERENCES [dbo].[LabVariants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TaskVariants_Id] in table 'LabVariantTaskVariant'
ALTER TABLE [dbo].[LabVariantTaskVariant]
ADD CONSTRAINT [FK_LabVariantTaskVariant_TaskVariant]
    FOREIGN KEY ([TaskVariants_Id])
    REFERENCES [dbo].[TaskVariants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LabVariantTaskVariant_TaskVariant'
CREATE INDEX [IX_FK_LabVariantTaskVariant_TaskVariant]
ON [dbo].[LabVariantTaskVariant]
    ([TaskVariants_Id]);
GO

-- Creating foreign key on [LabWork_Id] in table 'LabVariants'
ALTER TABLE [dbo].[LabVariants]
ADD CONSTRAINT [FK_LabWorkLabVariant]
    FOREIGN KEY ([LabWork_Id])
    REFERENCES [dbo].[LabWorks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LabWorkLabVariant'
CREATE INDEX [IX_FK_LabWorkLabVariant]
ON [dbo].[LabVariants]
    ([LabWork_Id]);
GO

-- Creating foreign key on [LabVariant_Id] in table 'Results'
ALTER TABLE [dbo].[Results]
ADD CONSTRAINT [FK_LabVariantResult]
    FOREIGN KEY ([LabVariant_Id])
    REFERENCES [dbo].[LabVariants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LabVariantResult'
CREATE INDEX [IX_FK_LabVariantResult]
ON [dbo].[Results]
    ([LabVariant_Id]);
GO

-- Creating foreign key on [LabWork_Id] in table 'LabEntries'
ALTER TABLE [dbo].[LabEntries]
ADD CONSTRAINT [FK_LabWorkLabEntry]
    FOREIGN KEY ([LabWork_Id])
    REFERENCES [dbo].[LabWorks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LabWorkLabEntry'
CREATE INDEX [IX_FK_LabWorkLabEntry]
ON [dbo].[LabEntries]
    ([LabWork_Id]);
GO

-- Creating foreign key on [Task_Id] in table 'LabEntries'
ALTER TABLE [dbo].[LabEntries]
ADD CONSTRAINT [FK_TaskLabEntry]
    FOREIGN KEY ([Task_Id])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskLabEntry'
CREATE INDEX [IX_FK_TaskLabEntry]
ON [dbo].[LabEntries]
    ([Task_Id]);
GO

-- Creating foreign key on [LabWorks_Id] in table 'LabWorkGroup'
ALTER TABLE [dbo].[LabWorkGroup]
ADD CONSTRAINT [FK_LabWorkGroup_LabWork]
    FOREIGN KEY ([LabWorks_Id])
    REFERENCES [dbo].[LabWorks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Groups_Id] in table 'LabWorkGroup'
ALTER TABLE [dbo].[LabWorkGroup]
ADD CONSTRAINT [FK_LabWorkGroup_Group]
    FOREIGN KEY ([Groups_Id])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LabWorkGroup_Group'
CREATE INDEX [IX_FK_LabWorkGroup_Group]
ON [dbo].[LabWorkGroup]
    ([Groups_Id]);
GO

-- Creating foreign key on [Task_Id] in table 'Actions'
ALTER TABLE [dbo].[Actions]
ADD CONSTRAINT [FK_TaskAction]
    FOREIGN KEY ([Task_Id])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskAction'
CREATE INDEX [IX_FK_TaskAction]
ON [dbo].[Actions]
    ([Task_Id]);
GO

-- Creating foreign key on [CategoryId] in table 'TestQuestions'
ALTER TABLE [dbo].[TestQuestions]
ADD CONSTRAINT [FK_CategoryTestQuestion]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryTestQuestion'
CREATE INDEX [IX_FK_CategoryTestQuestion]
ON [dbo].[TestQuestions]
    ([CategoryId]);
GO

-- Creating foreign key on [Id] in table 'Users_Student'
ALTER TABLE [dbo].[Users_Student]
ADD CONSTRAINT [FK_Student_inherits_User]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------