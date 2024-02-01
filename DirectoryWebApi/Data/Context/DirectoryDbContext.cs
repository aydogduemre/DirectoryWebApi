using DirectoryWebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DirectoryWebApi.Data.Context
{
    public sealed class DirectoryDbContext : DbContext
    {
        public DirectoryDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Person> People { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
    }
}
