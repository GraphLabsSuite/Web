SET QUOTED_IDENTIFIER OFF;
GO
USE [gl_unit_tests];
GO

-- --------------------------------------------------
-- Clear all tables
-- --------------------------------------------------
DECLARE @name VARCHAR(128)
DECLARE @schema VARCHAR(128)
DECLARE @SQL VARCHAR(254)
DECLARE @COUNT INT

IF OBJECT_ID(N'tempdb..#tablesToClear') IS NOT NULL
BEGIN
  PRINT 'Dropping #tablesToClear...' 
  DROP TABLE #tablesToClear
END

PRINT 'Creating temp table...' 

SELECT sys.objects.[name] as [name], sys.schemas.[name] as [schema]
INTO #tablesToClear
FROM sys.objects INNER JOIN sys.schemas ON sys.objects.[schema_id] = sys.schemas.[schema_id]
WHERE sys.objects.[type] = N'U' AND NOT (sys.schemas.[name] = N'dbo' AND sys.objects.[name] = N'TestTable')
ORDER BY sys.objects.[name]

SELECT @COUNT = (SELECT count(*) FROM #tablesToClear)
PRINT 'Clearing ' + convert(varchar(10), @count) + ' tables'

SELECT TOP 1 
  @name = [name], 
  @schema = [schema]
FROM #tablesToClear
ORDER BY #tablesToClear.[name]

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DELETE FROM [' + RTRIM(@schema) + '].[' + RTRIM(@name) +']'
    EXEC (@SQL)
	SELECT @SQL = 'DELETE FROM #tablesToClear WHERE [name] = ''' + @name + ''' AND [schema] = ''' + @schema + ''''
    EXEC (@SQL)
    PRINT 'Cleared Table: ' + @schema + '.' + @name
	SELECT @name = null
    SELECT TOP 1 
      @name = [name], 
      @schema = [schema]
    FROM #tablesToClear
    ORDER BY #tablesToClear.[name]
END
GO
DELETE FROM [dbo].[TaskResults];
GO