using RecettesLek.Data;
using System.ComponentModel.DataAnnotations;

namespace RecettesLek.Models
{
    public class ModeleVueRecette
    {
        [Required]
        public int RecetteID { get; set; }

        [Required]
        public string Nom { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public List<String> ComposantsPrincipaux { get; set; }

        

        [Required]
        public IFormFile Image { get; set; }

        public string Mimetype;

    }
}
