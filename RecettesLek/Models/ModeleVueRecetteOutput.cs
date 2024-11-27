using RecettesLek.Data;

namespace RecettesLek.Models
{
    public class ModeleVueRecetteOutput
    {
        
        public int RecetteID { get; set; }

        
        public string? Nom { get; set; }
        
        public string? Description { get; set; }
        
        public List<String>?  ComposantsPrincipaux { get; set; }
        
        public byte[]? Image { get; set; }

        public string? Mimetype {  get; set; }

        public virtual ICollection<Commentaire>? ListeCommentaires { get; set; }


    }
}
