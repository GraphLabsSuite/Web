SET QUOTED_IDENTIFIER OFF;
GO
USE [gl_unit_tests];
GO

-- --------------------------------------------------
-- Clean up
-- --------------------------------------------------

DELETE FROM [dbo].[News];
GO
DELETE FROM [dbo].[Sessions];
GO
DELETE FROM [dbo].[Settings];
GO
DELETE FROM [dbo].[Actions];
GO
DELETE FROM [dbo].[Results];
GO
DELETE FROM [dbo].[AnswerVariants];
GO
DELETE FROM [dbo].[VariantTestQuestion];
GO
DELETE FROM [dbo].[TestQuestions];
GO
DELETE FROM [dbo].[Categories];
GO
DELETE FROM [dbo].[LabEntries];
GO
DELETE FROM [dbo].[LabVariantTaskVariant];
GO
DELETE FROM [dbo].[TaskVariants];
GO
DELETE FROM [dbo].[Tasks];
GO
DELETE FROM [dbo].[LabVariants];
GO
DELETE FROM [dbo].[LabWorkGroup];
GO
DELETE FROM [dbo].[LabWorks];
GO
DELETE FROM [dbo].[Users_Student];
GO
DELETE FROM [dbo].[Users];
GO
DELETE FROM [dbo].[Groups];
GO