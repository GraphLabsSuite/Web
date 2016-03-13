using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using GraphLabs.Dal.Ef;
using NUnit.Framework;

namespace GraphLabs.Tests.DomainModel
{
    [TestFixture]
    public abstract class GraphLabsTestBase : TestBaseBase
    {
        protected GraphLabsContext GraphLabsContext { get; private set; }

        protected GraphLabsContext CreateDbContext()
        {
            return new GraphLabsContext("GraphLabsTestContext");
        }

        #region Скрипты

        private const string SQL_SCRIPTS_FOLDER = "SqlScripts";
        private const string CREATION_SCRIPT = "CreateSchema.sql";
        private const string INITIALIZATION_SCRIPT = "InitScript.sql";
        private const string CLEAR_SCHEMA_SCRIPT = "ClearSchema.sql";
        private const string CLEAR_TABLES_SCRIPT = "ClearTables.sql";

        private static IEnumerable<string> GetSchemaCreationScript()
        {
            return GetCommands(File.ReadAllText(Path.Combine(SQL_SCRIPTS_FOLDER, CREATION_SCRIPT)));
        }

        private static IEnumerable<string> GetSchemaInitializationScript()
        {
            return GetCommands(File.ReadAllText(Path.Combine(SQL_SCRIPTS_FOLDER, INITIALIZATION_SCRIPT)));
        }

        private static IEnumerable<string> GetSchemaCleaningScript()
        {
            return GetCommands(File.ReadAllText(Path.Combine(SQL_SCRIPTS_FOLDER, CLEAR_SCHEMA_SCRIPT)));
        }

        private static IEnumerable<string> GetTablesCleaningScript()
        {
            return GetCommands(File.ReadAllText(Path.Combine(SQL_SCRIPTS_FOLDER, CLEAR_TABLES_SCRIPT)));
        }

        private static IEnumerable<string> GetCommands(string sqlText)
        {
            var sql = sqlText
                .Replace("gltst", "gl_unit_tests");

            var strings = sql
                .Split(new[] {Environment.NewLine, "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var builder = new StringBuilder();
            foreach (var str in strings)
            {
                if (string.IsNullOrWhiteSpace(str) || str.StartsWith("--"))
                    continue;

                if (str.StartsWith("GO", StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return builder.ToString();
                    builder.Clear();
                }
                else
                {
                    builder.AppendLine(str);
                }
            }
        }

        private void ExecuteScript(IEnumerable<string> commands, SqlConnection connection)
        {
            foreach (var command in commands)
            {
                using (var sqlCommand = new SqlCommand(command, connection))
                {
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        private void ExecuteOnConnection(Action<SqlConnection> action)
        {
            var connection = new SqlConnection(GetConnectionString());
            try
            {
                connection.Open();

                action(connection);
            }
            catch (Exception e)
            {
                Assert.Fail("Не забудьте почистить dbo.TestTable, т.к. произошла ошибка: {0}", e);
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion


        #region Признак выполняющихся тестов в базе

        private void SetTestsAreRunning(SqlConnection connection)
        {
            using (var cmd = new SqlCommand(@"insert into dbo.TestTable	(Id) values (1);", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private bool CheckNoTestsRunning(SqlConnection connection)
        {
            var cmd = new SqlCommand(@"select count(*) from dbo.TestTable;", connection);
            try
            {
                var count = (int)cmd.ExecuteScalar();
                return count == 0;
            }
            catch (Exception)
            {
                throw new Exception("При попытке обращения к таблице dbo.TestTable возникла какая-то ошибка. Быть может, таблица отсутствует?");
            }
            finally
            {
                cmd.Dispose();
            }
        }

        private void SetTestsFinished(SqlConnection connection)
        {
            using (var cmd = new SqlCommand(@"delete from dbo.TestTable;", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        
        #region SetUp/TearDown

        /// <summary> Создаём схему </summary>
        [TestFixtureSetUp]
        public virtual void SetUp()
        {
            ExecuteOnConnection(
                c =>
                {
                    if (!CheckNoTestsRunning(c))
                    {
                        Assert.Fail(
                            "Либо сейчас тесты выполняются где-то ещё, либо кто-то забыл удалить все записи из dbo.TestTable.");
                    }
                    SetTestsAreRunning(c);

                    ExecuteScript(GetSchemaCreationScript(), c);
                });
        }

        /// <summary> Чистим схему </summary>
        [TestFixtureTearDown]
        public virtual void TearDown()
        {
            GraphLabsContext.Dispose();

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();

                ExecuteScript(GetSchemaCleaningScript(), connection);
                SetTestsFinished(connection);
            }
        }

        [SetUp]
        public virtual void TestSetUp()
        {
            ExecuteOnConnection(c => ExecuteScript(GetSchemaInitializationScript(), c));
            GraphLabsContext = CreateDbContext();
        }

        [TearDown]
        public virtual void TestTearDown()
        {
            GraphLabsContext.Dispose();
            ExecuteOnConnection(c => ExecuteScript(GetTablesCleaningScript(), c));
        }

        #endregion
    }
}
