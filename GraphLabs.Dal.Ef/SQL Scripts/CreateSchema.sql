
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/31/2016 21:35:38
-- Generated from EDMX file: C:\Users\Илья\Desktop\taskresults2\GraphLabs.Dal.Ef\GraphLabsDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [gltst];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
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
    [Score] smallint  NULL,
    [Status] tinyint  NOT NULL,
    [Student_Id] bigint  NOT NULL,
    [LabVariant_Id] bigint  NOT NULL
);
GO

-- Creating table 'StudentActions'
CREATE TABLE [dbo].[StudentActions] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(100)  NOT NULL,
    [Penalty] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [TaskResult_Id] bigint  NOT NULL
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
    [VariantGenerator] varbinary(max)  NULL,
    [Sections] nvarchar(100)  NULL,
    [Version] nvarchar(20)  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [TaskData_Id] int  NOT NULL
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
    [Question] nvarchar(max)  NOT NULL,
    [Category_Id] bigint  NOT NULL
);
GO

-- Creating table 'AnswerVariants'
CREATE TABLE [dbo].[AnswerVariants] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Answer] nvarchar(50)  NOT NULL,
    [IsCorrect] bit  NOT NULL,
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

-- Creating table 'TaskDatas'
CREATE TABLE [dbo].[TaskDatas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Xap] varbinary(max)  NOT NULL
);
GO

-- Creating table 'TaskResults'
CREATE TABLE [dbo].[TaskResults] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Status] tinyint  NOT NULL,
    [Result_Id] bigint  NOT NULL,
    [TaskVariant_Id] bigint  NOT NULL
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

-- Creating primary key on [Id] in table 'StudentActions'
ALTER TABLE [dbo].[StudentActions]
ADD CONSTRAINT [PK_StudentActions]
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

-- Creating primary key on [Id] in table 'TaskDatas'
ALTER TABLE [dbo].[TaskDatas]
ADD CONSTRAINT [PK_TaskDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaskResults'
ALTER TABLE [dbo].[TaskResults]
ADD CONSTRAINT [PK_TaskResults]
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
    PRIMARY KEY CLUSTERED ([LabVariant_Id], [TestQuestions_Id] ASC);
GO

-- Creating primary key on [LabVariantTaskVariant_TaskVariant_Id], [TaskVariants_Id] in table 'LabVariantTaskVariant'
ALTER TABLE [dbo].[LabVariantTaskVariant]
ADD CONSTRAINT [PK_LabVariantTaskVariant]
    PRIMARY KEY CLUSTERED ([LabVariantTaskVariant_TaskVariant_Id], [TaskVariants_Id] ASC);
GO

-- Creating primary key on [LabWorks_Id], [Groups_Id] in table 'LabWorkGroup'
ALTER TABLE [dbo].[LabWorkGroup]
ADD CONSTRAINT [PK_LabWorkGroup]
    PRIMARY KEY CLUSTERED ([LabWorks_Id], [Groups_Id] ASC);
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
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserNews'
CREATE INDEX [IX_FK_UserNews]
ON [dbo].[News]
    ([User_Id]);
GO

-- Creating foreign key on [TestQuestion_Id] in table 'AnswerVariants'
ALTER TABLE [dbo].[AnswerVariants]
ADD CONSTRAINT [FK_TestQuestionAnswerVariant]
    FOREIGN KEY ([TestQuestion_Id])
    REFERENCES [dbo].[TestQuestions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

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
GO

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
GO

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
GO

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
GO

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
GO

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
GO

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
GO

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
GO

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
GO

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
GO

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
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LabWorkGroup_Group'
CREATE INDEX [IX_FK_LabWorkGroup_Group]
ON [dbo].[LabWorkGroup]
    ([Groups_Id]);
GO

-- Creating foreign key on [Category_Id] in table 'TestQuestions'
ALTER TABLE [dbo].[TestQuestions]
ADD CONSTRAINT [FK_TestQuestionCategory]
    FOREIGN KEY ([Category_Id])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestQuestionCategory'
CREATE INDEX [IX_FK_TestQuestionCategory]
ON [dbo].[TestQuestions]
    ([Category_Id]);
GO

-- Creating foreign key on [TaskData_Id] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_TaskTaskData]
    FOREIGN KEY ([TaskData_Id])
    REFERENCES [dbo].[TaskDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskTaskData'
CREATE INDEX [IX_FK_TaskTaskData]
ON [dbo].[Tasks]
    ([TaskData_Id]);
GO

-- Creating foreign key on [Result_Id] in table 'TaskResults'
ALTER TABLE [dbo].[TaskResults]
ADD CONSTRAINT [FK_ResultTaskResult]
    FOREIGN KEY ([Result_Id])
    REFERENCES [dbo].[Results]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ResultTaskResult'
CREATE INDEX [IX_FK_ResultTaskResult]
ON [dbo].[TaskResults]
    ([Result_Id]);
GO

-- Creating foreign key on [TaskResult_Id] in table 'StudentActions'
ALTER TABLE [dbo].[StudentActions]
ADD CONSTRAINT [FK_TaskResultStudentAction]
    FOREIGN KEY ([TaskResult_Id])
    REFERENCES [dbo].[TaskResults]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskResultStudentAction'
CREATE INDEX [IX_FK_TaskResultStudentAction]
ON [dbo].[StudentActions]
    ([TaskResult_Id]);
GO

-- Creating foreign key on [TaskVariant_Id] in table 'TaskResults'
ALTER TABLE [dbo].[TaskResults]
ADD CONSTRAINT [FK_TaskResultTaskVariant]
    FOREIGN KEY ([TaskVariant_Id])
    REFERENCES [dbo].[TaskVariants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskResultTaskVariant'
CREATE INDEX [IX_FK_TaskResultTaskVariant]
ON [dbo].[TaskResults]
    ([TaskVariant_Id]);
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