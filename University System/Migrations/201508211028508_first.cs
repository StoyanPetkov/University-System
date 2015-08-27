namespace University_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectID = c.Int(nullable: false),
                        TeacherID = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectID, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.TeacherID, cascadeDelete: true)
                .Index(t => t.SubjectID)
                .Index(t => t.TeacherID);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TitleID = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Titles", t => t.TitleID, cascadeDelete: true)
                .Index(t => t.TitleID);
            
            CreateTable(
                "dbo.CourseSubjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseID = c.Int(nullable: false),
                        SubjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectID, cascadeDelete: true)
                .Index(t => t.CourseID)
                .Index(t => t.SubjectID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Titles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        ArticleID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        UserType = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(),
                        ParentComment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleID, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.ParentComment_Id)
                .Index(t => t.ArticleID)
                .Index(t => t.ParentComment_Id);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectID = c.Int(nullable: false),
                        GradeValue = c.Double(nullable: false),
                        Student_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.Student_Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectID, cascadeDelete: true)
                .Index(t => t.SubjectID)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FacultyNumber = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ArticleID = c.Int(nullable: false),
                        UserType = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleID, cascadeDelete: true)
                .Index(t => t.ArticleID);
            
            CreateTable(
                "dbo.CourseSubjectTeachers",
                c => new
                    {
                        CourseSubject_Id = c.Int(nullable: false),
                        Teacher_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseSubject_Id, t.Teacher_Id })
                .ForeignKey("dbo.CourseSubjects", t => t.CourseSubject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id, cascadeDelete: true)
                .Index(t => t.CourseSubject_Id)
                .Index(t => t.Teacher_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "ArticleID", "dbo.Articles");
            DropForeignKey("dbo.Grades", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Grades", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Students", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Comments", "ParentComment_Id", "dbo.Comments");
            DropForeignKey("dbo.Comments", "ArticleID", "dbo.Articles");
            DropForeignKey("dbo.Articles", "TeacherID", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "TitleID", "dbo.Titles");
            DropForeignKey("dbo.CourseSubjectTeachers", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.CourseSubjectTeachers", "CourseSubject_Id", "dbo.CourseSubjects");
            DropForeignKey("dbo.CourseSubjects", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.CourseSubjects", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Articles", "SubjectID", "dbo.Subjects");
            DropIndex("dbo.CourseSubjectTeachers", new[] { "Teacher_Id" });
            DropIndex("dbo.CourseSubjectTeachers", new[] { "CourseSubject_Id" });
            DropIndex("dbo.Likes", new[] { "ArticleID" });
            DropIndex("dbo.Students", new[] { "CourseID" });
            DropIndex("dbo.Grades", new[] { "Student_Id" });
            DropIndex("dbo.Grades", new[] { "SubjectID" });
            DropIndex("dbo.Comments", new[] { "ParentComment_Id" });
            DropIndex("dbo.Comments", new[] { "ArticleID" });
            DropIndex("dbo.CourseSubjects", new[] { "SubjectID" });
            DropIndex("dbo.CourseSubjects", new[] { "CourseID" });
            DropIndex("dbo.Teachers", new[] { "TitleID" });
            DropIndex("dbo.Articles", new[] { "TeacherID" });
            DropIndex("dbo.Articles", new[] { "SubjectID" });
            DropTable("dbo.CourseSubjectTeachers");
            DropTable("dbo.Likes");
            DropTable("dbo.Students");
            DropTable("dbo.Grades");
            DropTable("dbo.Comments");
            DropTable("dbo.Titles");
            DropTable("dbo.Courses");
            DropTable("dbo.CourseSubjects");
            DropTable("dbo.Teachers");
            DropTable("dbo.Subjects");
            DropTable("dbo.Articles");
            DropTable("dbo.Administrators");
        }
    }
}
