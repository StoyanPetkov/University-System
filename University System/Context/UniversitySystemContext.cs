using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.Context
{
    public class UniversitySystemContext : DbContext
    {
        public UniversitySystemContext() : base("name=UniversitySystemContext")
        {
        }

        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Title> Title { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<CourseSubject> CourseSubject { get; set; }
    }
}