using System;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using GraphLabs.Dal.Ef;
using NUnit.Framework;
using System.Linq;
using GraphLabs.DomainModel;

namespace GraphLabs.Tests.DomainModel
{
    [TestFixture]
    public class DataModelTests : GraphLabsTestBase
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

            var user = GraphLabsContext.Users.Create();
            user.Name = TEST_NAME;
            user.Email = "example@example.com";
            user.FatherName = "FatherName";
            user.PasswordHash = "PasswordHash";
            user.Surname = "Surname";

            GraphLabsContext.SaveChanges();

            Assert.AreEqual(1, GraphLabsContext.Users.Count(u => u.Name == TEST_NAME));
        }

        [Test]
        public void TestUserLoginIsUnique()
        {
            const string EMAIL = "example@example.com";

            RemoveByCriteria<User>(u => u.Email == EMAIL);

            var user1 = GraphLabsContext.Users.Create();
            user1.Name = TEST_NAME;
            user1.Email = EMAIL;
            user1.FatherName = "FatherName";
            user1.PasswordHash = "PasswordHash";
            user1.Surname = "Surname";
            GraphLabsContext.Users.Add(user1);
            GraphLabsContext.SaveChanges();

            var user2 = GraphLabsContext.Users.Create();
            user2.Name = TEST_NAME;
            user2.Email = EMAIL;
            user2.FatherName = "FatherName";
            user2.PasswordHash = "PasswordHash";
            user2.Surname = "Surname";
            GraphLabsContext.Users.Add(user2);

            Assert.Throws<DbUpdateException>(() => GraphLabsContext.SaveChanges());
        }
    }
}
