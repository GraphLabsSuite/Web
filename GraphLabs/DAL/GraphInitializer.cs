using System.Collections.Generic;
using System.Data.Entity;
using GraphLabs.Site.Models;

namespace GraphLabs.Site.DAL
{
    public class GraphInitializer : DropCreateDatabaseIfModelChanges<GraphContext>
    {
        protected override void Seed(GraphContext context)
        {
            var groups = new List<Group>
            {
                new Group { GroupID = 1, Name = "Администраторы", Year = 1, IsActive = false },
                new Group { GroupID = 2, Name = "Преподаватели", Year = 1, IsActive = true },
                new Group { GroupID = 3, Name = "K2-221", Year = 2013, IsActive = true },
                new Group { GroupID = 4, Name = "K2-222", Year = 2013, IsActive = true },
                new Group { GroupID = 5, Name = "K2-223", Year = 2013, IsActive = true },
                new Group { GroupID = 6, Name = "K2-224", Year = 2013, IsActive = true }
            };
            groups.ForEach(s => context.Groups.Add(s));
            context.SaveChanges();

            var users = new List<User>
            {
                new User { UserID = 1, Login = "SanCom", Password = "123456qwe", Verify = true, Name = "Александр", SurName = "Комаров", FatherName = "Дмитриевич", Email = "SannComm@mail.ru", GroupID = 1 },
                new User { UserID = 2, Login = "MAKorotkova", Password = "123456qwe", Verify = false, Name = "Мария", SurName = "Короткова", FatherName = "Александровна", Email = "bioinf@yandex.ru", GroupID = 2 }
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();
        }
    }
}