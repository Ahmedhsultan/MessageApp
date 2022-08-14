using Database.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace Database
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }


        public DbSet<Users> Users { get; set; }
        public DbSet<Messages> Messages { get; set; }
    }
}
