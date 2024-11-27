using System.ComponentModel;
using System.Reflection.Metadata;

namespace RecettesLek.Data
{
    public class Recette
    {
        public int RecetteId { get; set; } 
        public string Nom { get; set; }    
        public string Description { get; set; }
        public List<String> ComposantsPrincipaux { get; set; }

        
        public byte[] Image {  get; set; }   

        public string Mimetype { get; set; }    

        public virtual ICollection<Commentaire> ListeCommentaires{ get; set; }    
        

    }
}
