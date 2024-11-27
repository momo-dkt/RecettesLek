using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecettesLek.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<RecetteUtilisateur> _recetteUtilisateur { get; set; }
        public DbSet<Recette> _recette {  get; set; }  

        public DbSet<Commentaire> _commentaire { get; set; }  
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RecetteUtilisateur>().HasKey(x => new { x.UtilisateurID,x.RecetteId });
         
            /*
            modelBuilder.Entity<MyTable>()
            .Property(m => m.Id)
            .ValueGeneratedOnAdd();*/

        }
    }
}
