using System;
using ContactAppWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactAppWeb.Data
{
    // DataContext class inherits from DbContext, responsible for database interactions
    public class DataContext : DbContext
    {
        // Constructor accepts DbContextOptions, used to configure database connection
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        // DbSet property to represent the ContactModel entity in the database
        public DbSet<ContactModel> ContactModels { get; set; }
    }
}

