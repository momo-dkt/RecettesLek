using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RecettesLek.Data
{
    public class RecetteUtilisateur
    {

        
        public int RecetteId { get; set; }

        [ForeignKey(nameof(User))]
        public string UtilisateurID {  get; set; }

        

        public virtual Utilisateur User {  get; set; }

        public virtual Recette Recette { get; set; }


    }
}
