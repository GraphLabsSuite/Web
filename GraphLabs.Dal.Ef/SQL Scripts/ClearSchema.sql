
-- --------------------------------------------------
-- Скрипт очистки тестовой базы
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [gl_unit_tests];
GO

/* Drop all non-system stored procs */
DECLARE @name VARCHAR(128)
DECLARE @schema VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT TOP 1 
  @name = sys.objects.[name], 
  @schema = sys.schemas.[name]
FROM sys.objects INNER JOIN sys.schemas ON sys.objects.[schema_id] = sys.schemas.[schema_id]
WHERE sys.objects.[type] = N'P'
ORDER BY sys.objects.[name]

WHILE @name is not null
BEGIN
    SELECT @SQL = 'DROP PROCEDURE [' + RTRIM(@schema) + '].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Procedure: ' + @schema + '.' + @name
	SELECT @name = null
    SELECT TOP 1 
      @name = sys.objects.[name], 
      @schema = sys.schemas.[name]
    FROM sys.objects INNER JOIN sys.schemas ON sys.objects.[schema_id] = sys.schemas.[schema_id]
    WHERE sys.objects.[type] = N'P'
    ORDER BY sys.objects.[name]
END
GO

/* Drop all views */
DECLARE @name VARCHAR(128)
DECLARE @schema VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT TOP 1 
  @name = sys.objects.[name], 
  @schema = sys.schemas.[name]
FROM sys.objects INNER JOIN sys.schemas ON sys.objects.[schema_id] = sys.schemas.[schema_id]
WHERE sys.objects.[type] = N'V'
ORDER BY sys.objects.[name]


WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DROP VIEW [' + RTRIM(@schema) + '].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped View: ' + @schema + '.' + @name
	SELECT @name = null
    SELECT TOP 1 
      @name = sys.objects.[name], 
      @schema = sys.schemas.[name]
    FROM sys.objects INNER JOIN sys.schemas ON sys.objects.[schema_id] = sys.schemas.[schema_id]
    WHERE sys.objects.[type] = N'V'
    ORDER BY sys.objects.[name]
END
GO

/* Drop all functions */
DECLARE @name VARCHAR(128)
DECLARE @schema VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT TOP 1 
  @name = sys.objects.[name], 
  @schema = sys.schemas.[name]
FROM sys.objects INNER JOIN sys.schemas ON sys.objects.[schema_id] = sys.schemas.[schema_id]
WHERE sys.objects.[type] IN (N'FN', N'IF', N'TF', N'FS', N'FT')
ORDER BY sys.objects.[name]

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DROP FUNCTION [' + RTRIM(@schema) + '].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Function: ' + @schema + '.' + @name
	SELECT @name = null
    SELECT TOP 1 
      @name = sys.objects.[name], 
      @schema = sys.schemas.[name]
    FROM sys.objects INNER JOIN sys.schemas ON sys.objects.[schema_id] = sys.schemas.[schema_id]
    WHERE sys.objects.[type] IN (N'FN', N'IF', N'TF', N'FS', N'FT')
    ORDER BY sys.objects.[name]
END
GO

/* Drop all Foreign Key constraints */
DECLARE @tableName VARCHAR(128)
DECLARE @tableSchema VARCHAR(128)
DECLARE @constraintName VARCHAR(254)
DECLARE @SQL VARCHAR(254)

SELECT TOP 1 
  @tableName = TABLE_NAME,
  @tableSchema = TABLE_SCHEMA,
  @constraintName = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = N'FOREIGN KEY'
ORDER BY TABLE_NAME

WHILE @constraintName is not null
BEGIN
  SELECT @SQL = 'ALTER TABLE [' + RTRIM(@tableSchema) + '].[' + RTRIM(@tableName) +'] DROP CONSTRAINT [' + RTRIM(@constraintName) +']'
  EXEC (@SQL)
  PRINT 'Dropped FK Constraint: ' + @constraintName + ' on ' + @tableSchema + '.' + @tableName
  SELECT @constraintName = null
  SELECT TOP 1 
    @tableName = TABLE_NAME,
    @tableSchema = TABLE_SCHEMA,
    @constraintName = CONSTRAINT_NAME
  FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
  WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = N'FOREIGN KEY'
  ORDER BY TABLE_NAME
END
GO

/* Drop all Primary Key constraints */
DECLARE @tableName VARCHAR(128)
DECLARE @tableSchema VARCHAR(128)
DECLARE @constraintName VARCHAR(254)
DECLARE @SQL VARCHAR(254)

SELECT TOP 1 
  @tableName = TABLE_NAME,
  @tableSchema = TABLE_SCHEMA,
  @constraintName = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = N'PRIMARY KEY'
ORDER BY TABLE_NAME

WHILE @constraintName is not null
BEGIN
  SELECT @SQL = 'ALTER TABLE [' + RTRIM(@tableSchema) + '].[' + RTRIM(@tableName) +'] DROP CONSTRAINT [' + RTRIM(@constraintName) +']'
  EXEC (@SQL)
  PRINT 'Dropped FK Constraint: ' + @constraintName + ' on ' + @tableSchema + '.' + @tableName
  SELECT @constraintName = null
  SELECT TOP 1 
    @tableName = TABLE_NAME,
    @tableSchema = TABLE_SCHEMA,
    @constraintName = CONSTRAINT_NAME
  FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
  WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = N'PRIMARY KEY'
  ORDER BY TABLE_NAME
END
GO


/* Drop all tables */
DECLARE @name VARCHAR(128)
DECLARE @schema VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT TOP 1 
  @name = sys.objects.[name], 
  @schema = sys.schemas.[name]
FROM sys.objects INNER JOIN sys.schemas ON sys.objects.[schema_id] = sys.schemas.[schema_id]
WHERE sys.objects.[type] = N'U' AND sys.schemas.[name] != N'dbo' AND sys.objects.[name] != N'TestTable'
ORDER BY sys.objects.[name]


WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DROP TABLE [' + RTRIM(@schema) + '].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Table: ' + @schema + '.' + @name
	SELECT @name = null
    SELECT TOP 1 
      @name = sys.objects.[name], 
      @schema = sys.schemas.[name]
    FROM sys.objects INNER JOIN sys.schemas ON sys.objects.[schema_id] = sys.schemas.[schema_id]
    WHERE sys.objects.[type] = N'U' AND sys.schemas.[name] != N'dbo' AND sys.objects.[name] != N'TestTable'
    ORDER BY sys.objects.[name]
END
GO