
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/29/2013 10:48:32
-- Generated from EDMX file: E:\Рабочий стол\graphlabs.datamodel\trunk\GraphLabs.DataModel\GraphLabsDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [gltst];
GO

-- Ищем и убиваем существующие индексы
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'UK_UserEmail')
  DROP INDEX UK_UserEmail ON [dbo].[Users];
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Tasks]') AND name = N'UK_TaskNameVersion')
   DROP INDEX UK_TaskNameVersion ON [dbo].[Tasks]; 
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TaskVariants]') AND name = N'UK_TaskVariantNumberVersion')
   DROP INDEX UK_TaskVariantNumberVersion ON [dbo].[TaskVariants]; 
GO

-- Создаём индексы
CREATE UNIQUE INDEX UK_UserEmail 
   ON [dbo].[Users] (Email); 
GO
CREATE UNIQUE INDEX UK_TaskNameVersion 
   ON [dbo].[Tasks] (Name, Version); 
GO
CREATE UNIQUE INDEX UK_TaskVariantNumberVersion 
   ON [dbo].[TaskVariants] (Task_Id, Number, Version); 
GO

-- Учётка администратора
INSERT INTO [dbo].[Users] 
	(Surname, Name, FatherName, PasswordHash, Email, Role)
VALUES 
    ('Admin', 'Admin', 'Admin', 
	'$2a$06$LzfX0oshO/c9C2yKVIBmfOupOcoIfqDrXdteGR1yVQjdKj6BDztZO',
	'admin@graphlabs.ru', 
	4)
GO

--Учетка преподавателя
INSERT INTO [dbo].[Users]
	(Surname, Name, FatherName, PasswordHash, Email, Role)
VALUES 
    ('Tutor', 'Tutor', 'Tutor', 
	'$2a$06$LzfX0oshO/c9C2yKVIBmfOupOcoIfqDrXdteGR1yVQjdKj6BDztZO',
	'tutor@graphlabs.ru', 
	2)
GO

-- Учётка пользователя
INSERT INTO [dbo].[Users] 
	(Surname, Name, FatherName, PasswordHash, Email, Role)
VALUES 
    ('Студент', 'Типичный', 'Тестовый', 
	'$2a$06$LzfX0oshO/c9C2yKVIBmfOupOcoIfqDrXdteGR1yVQjdKj6BDztZO',
	'student@graphlabs.ru', 
	1)
GO

INSERT INTO [dbo].[Groups] 
	(Name, IsOpen, FirstYear)
VALUES 
    ('221', 'True', '1999')
GO

INSERT INTO [dbo].[Users_Student] 
	(IsVerified, IsDismissed, Id, Group_Id)
VALUES 
    ('True', 'False', '2', '1')
GO

-- Табличка с настройками
INSERT INTO [dbo].[Settings] 
	(Id,
	SystemDate)
VALUES 
    (1,
    ('20140101')) -- 1.1.2014
GO