using System;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using GraphLabs.DomainModel;
using NUnit.Framework;
using System.Linq;

namespace GraphLabs.Tests.DomainModel
{
    [TestFixture]
    public class DataModelTests : TestBase
    {
        private void RemoveByCriteria<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var set = GraphLabsContext.Set<T>();
            var entities = set.Where(predicate);
            foreach (var entity in entities)
            {
                set.Remove(entity);
            }
            GraphLabsContext.SaveChanges();
        }

        private const string TEST_NAME = "testUser";

        [Test]
        public void TestDoSmth()
        {
            RemoveByCriteria<User>(u => u.Name == TEST_NAME);

            Assert.AreEqual(0, GraphLabsContext.Users.Count(u => u.Name == TEST_NAME));

            var user = new User
            {
                Name = TEST_NAME,
                Email = "example@example.com",
                FatherName = "FatherName",
                PasswordHash = "PasswordHash",
                HashSalt = "ttt",
                Surname = "Surname",
            };
            GraphLabsContext.Users.Add(user);
            GraphLabsContext.SaveChanges();

            Assert.AreEqual(1, GraphLabsContext.Users.Count(u => u.Name == TEST_NAME));

        }

        [Test]
        public void TestUserLoginIsUnique()
        {
            const string EMAIL = "example@example.com";

            RemoveByCriteria<User>(u => u.Email == EMAIL);

            var user1 = new User
            {
                Name = TEST_NAME,
                Email = EMAIL,
                FatherName = "FatherName",
                PasswordHash = "PasswordHash",
                HashSalt = "ttt",
                Surname = "Surname"
            };
            GraphLabsContext.Users.Add(user1);
            GraphLabsContext.SaveChanges();

            var user2 = new User
            {
                Name = TEST_NAME,
                Email = EMAIL,
                FatherName = "FatherName",
                PasswordHash = "PasswordHash",
                HashSalt = "ttt",
                Surname = "Surname"
            };
            GraphLabsContext.Users.Add(user2);
            Assert.Throws<DbUpdateException>(() => GraphLabsContext.SaveChanges());

        }
    }
}
