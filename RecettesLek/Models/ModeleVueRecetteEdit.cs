using RecettesLek.Data;
using System.ComponentModel.DataAnnotations;

namespace RecettesLek.Models
{
    public class ModeleVueRecetteEdit
    {
        [Required]
        public int RecetteId { get; set; }

        [Required]
        public string Nom { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public List<String> ComposantsPrincipaux { get; set; }

       
        public byte[]? Image { get; set; }

        

        public string Mimetype { get; set; }

        public virtual ICollection<Commentaire>? ListeCommentaires { get; set; }

        public IFormFile? NewImage { get; set; }
        
    }
}
