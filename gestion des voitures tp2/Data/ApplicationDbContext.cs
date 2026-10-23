using gestion_des_voitures_tp2.Models;
using Microsoft.EntityFrameworkCore;

namespace gestion_des_voitures_tp2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Table « CategoriesVoitures » dans la base de données SQL Server
        public DbSet<CategoryVoiture> CategoriesVoitures { get; set; }
    }
}