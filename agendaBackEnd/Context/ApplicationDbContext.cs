using agendaBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace agendaBackEnd.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UsersModel> UsersModels { get; set; }
    }
}
