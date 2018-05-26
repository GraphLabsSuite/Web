using System.Data.SqlClient;
using NUnit.Framework;

namespace GraphLabs.Tests.DomainModel
{
    /// <summary> Если вы точно уверены, что тесты не запущены где-то ещё, 
    /// а в TestTable - какая-то фигня, то  воспользуйтесь этим </summary>
    [TestFixture]
    public class TestTableCleaner : TestBase
    {
        [Test]
        [Ignore("Эта штука - не тест. Нужна для сброса признака тестов в базе.")]
        public void Test()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand(@"delete from dbo.TestTable;", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
