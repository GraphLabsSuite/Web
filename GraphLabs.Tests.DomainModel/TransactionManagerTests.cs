using System.Linq;
using GraphLabs.DomainModel;
using NUnit.Framework;

namespace GraphLabs.Tests.DomainModel
{
    [TestFixture]
    public class TransactionManagerTests : GraphLabsTestBase
    {
        [Test]
        public void TestWhenNoCommitThenRollback()
        {
            TestDelegate doSmthWithoutCommit =
                () =>
                {
                    using(var manager = new TransactionManager(GraphLabsContext))
                    using (var transaction = manager.BeginTransaction())
                    {
                        var group = new Group()
                                    {
                                        FirstYear = 2014,
                                        IsOpen = true,
                                        Number = 123,
                                    };
                        GraphLabsContext.Groups.Add(group);

                        var user = new Student()
                                   {
                                       Email = "abc@example.com",
                                       FatherName = "123",
                                       Name = "123",
                                       Group = group
                                   };
                        GraphLabsContext.Users.Add(user);
                    }
                };
            Assert.DoesNotThrow(doSmthWithoutCommit);

            Assert.IsFalse(GraphLabsContext.ChangeTracker.HasChanges());
            
            var anotherContext = CreateDbContext();
            Assert.IsFalse(anotherContext.Groups.Any());
            Assert.IsFalse(anotherContext.Users.OfType<Student>().Any());
        }

        [Test]
        public void TestWhenCommitThenChangesSaved()
        {
            TestDelegate doSmthWithCommit =
                () =>
                {
                    using (var manager = new TransactionManager(GraphLabsContext))
                    using (var transaction = manager.BeginTransaction())
                    {
                        var group = new Group
                                    {
                                        FirstYear = 2014,
                                        IsOpen = true,
                                        Number = 123,
                                    };
                        GraphLabsContext.Groups.Add(group);

                        var user = new Student()
                                   {
                                       Email = "abc@example.com",
                                       FatherName = "321",
                                       Surname = "ttt",
                                       Name = "123",
                                       Group = group,
                                       PasswordHash = "ddddddddd",
                                   };
                        GraphLabsContext.Users.Add(user);
                        transaction.Commit();
                    }
                };

            Assert.DoesNotThrow(doSmthWithCommit);

            var anotherContext = CreateDbContext();
            
            var anotherGroup = anotherContext.Groups.Single();
            Assert.AreEqual(123, anotherGroup.Number);

            var anotherStudent = anotherContext.Users.OfType<Student>().Single();
            Assert.AreEqual("abc@example.com", anotherStudent.Email);
            Assert.AreEqual(anotherGroup, anotherStudent.Group);
        }

        [Test]
        public void TestCommitAfterRollback()
        {
            TestDelegate doSmth =
                () =>
                {
                    using (var manager = new TransactionManager(GraphLabsContext))
                    using (var transaction = manager.BeginTransaction())
                    {
                        var group = new Group()
                        {
                            FirstYear = 2013,
                            IsOpen = true,
                            Number = 234,
                        };
                        GraphLabsContext.Groups.Add(group);
                        transaction.Rollback();

                        group = new Group()
                        {
                            FirstYear = 2014,
                            IsOpen = true,
                            Number = 123,
                        };
                        GraphLabsContext.Groups.Add(group);
                        
                        var user = new Student()
                        {
                            Email = "abc@example.com",
                            FatherName = "321",
                            Surname = "ttt",
                            Name = "123",
                            Group = group,
                            PasswordHash = "ddddddddd",
                        };
                        GraphLabsContext.Users.Add(user);
                        transaction.Commit();
                    }
                };

            Assert.DoesNotThrow(doSmth);

            var anotherContext = CreateDbContext();

            var anotherGroup = anotherContext.Groups.Single();
            Assert.AreEqual(123, anotherGroup.Number);

            var anotherStudent = anotherContext.Users.OfType<Student>().Single();
            Assert.AreEqual("abc@example.com", anotherStudent.Email);
            Assert.AreEqual(anotherGroup, anotherStudent.Group);
        }

        [Test]
        public void TestRollbackAfterCommit()
        {
            TestDelegate doSmth =
                () =>
                {
                    using(var manager = new TransactionManager(GraphLabsContext))
                    using (var transaction = manager.BeginTransaction())
                    {
                        var group = new Group()
                                    {
                                        FirstYear = 2013,
                                        IsOpen = true,
                                        Number = 234,
                                    };
                        GraphLabsContext.Groups.Add(group);
                        transaction.Commit();

                        group = new Group()
                                {
                                    FirstYear = 2014,
                                    IsOpen = true,
                                    Number = 123,
                                };
                        GraphLabsContext.Groups.Add(group);

                        var user = new Student()
                                   {
                                       Email = "abc@example.com",
                                       FatherName = "321",
                                       Surname = "ttt",
                                       Name = "123",
                                       Group = group,
                                       PasswordHash = "ddddddddd",
                                   };
                        GraphLabsContext.Users.Add(user);
                        transaction.Rollback();
                    }
                };

            Assert.DoesNotThrow(doSmth);

            var anotherContext = CreateDbContext();

            var anotherGroup = anotherContext.Groups.Single();
            Assert.AreEqual(234, anotherGroup.Number);

            var usersCount = anotherContext.Users.OfType<Student>().Count();
            Assert.AreEqual(0, usersCount);
        }

        [Test]
        public void TestNoActiveTransactionAfterScopeDisposed()
        {
            var manager = new TransactionManager(GraphLabsContext);
            using (var t = manager.BeginTransaction())
            {
                
            }
            Assert.IsFalse(manager.HasActiveTransaction);
            manager.Dispose();
        }
    }
}
