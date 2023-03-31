using ExamSession.Таблицы;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ExamSession
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Assessment> Assessments => Set<Assessment>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<StudyGroup> StudyGroups => Set<StudyGroup>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Teacher_item> Teachers_items => Set<Teacher_item>();

        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=(localdb)\\mssqllocaldb;Trusted_Connection=True;");
            //optionsBuilder.UseSqlite("Data Source=helloapp.db");
        }
    }
}
