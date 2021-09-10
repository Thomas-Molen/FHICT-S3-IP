using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_WebAPI_Testing.Models
{
    public class BookContext : DbContext
    {
        private IConfiguration _configuration;

        public BookContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SQL_BooksDatabase"));
        }
    }
}
