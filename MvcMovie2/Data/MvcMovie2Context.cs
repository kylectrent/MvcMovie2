using Microsoft.EntityFrameworkCore;
using MvcMovie2.Models;

namespace MvcMovie2.Data
{
    public class MvcMovie2Context : DbContext
    {
        public MvcMovie2Context(DbContextOptions<MvcMovie2Context> options)
            : base(options)
        {
        }

        public DbSet<Movie2> Movie2 { get; set; }
    }
}