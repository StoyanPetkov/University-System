namespace University_System.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Context.UniversitySystemContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Context.UniversitySystemContext context)
        {
            context.Administrator.AddOrUpdate(
                a=> a.FirstName,
               new Entities.Administrator()
               {
                   FirstName = "Stoyan",
                   LastName = "Petkov",
                   UserName = "admin",
                   Password = "pass",
                   IsActive = true
               }
               );
            context.SaveChanges();
            context.Title.AddOrUpdate(
                    t=>t.Name,
                    new Entities.Title()
                    {
                        Name = "Professor"
                    }
                );
            context.SaveChanges();
            context.Teacher.AddOrUpdate(
                t => t.FirstName,
                    new Entities.Teacher()
                    {
                        FirstName = "Ivan",
                        LastName = "Ivanov",
                        UserName = "teacher",
                        Password = "pass",
                        TitleID = 1,
                        IsActive = true
                    }
                );
            context.SaveChanges();
            context.Course.AddOrUpdate(
                    c => c.Name,
                    new Entities.Course() {
                        Name = "Informatics",
                        Code = 99,
                    }
                );
            context.SaveChanges();
            context.Subject.AddOrUpdate(
                    s=> s.Name,
                    new Entities.Subject() {
                        Name = "Programming"
                    }
                );
            context.SaveChanges();
            context.CourseSubject.AddOrUpdate(
                    cs=> cs.CourseID,
                    new Entities.CourseSubject() {
                        CourseID = 1,
                        SubjectID =1
                    }
                );
            context.SaveChanges();
            context.Student.AddOrUpdate(
                    s=> s.FirstName,
                    new Entities.Student() {
                        FirstName = "Nikolai",
                        LastName = "Nikolov",
                        UserName = "student",
                        Password = "pass",
                        CourseID = 1,
                        IsActive = true,
                        FacultyNumber = 99999999
                    }
                );
            context.SaveChanges();
        }
    }
}
