namespace RecettesLek.Data

{
    public class Utilisateur: Microsoft.AspNetCore.Identity.IdentityUser
    {
        public Utilisateur():base() {
            this.ListeRecettes = new HashSet<RecetteUtilisateur>();
        }
         
        public string Prenom {  get; set; } 

        public virtual ICollection<RecetteUtilisateur> ListeRecettes { get; set; } 
    }
}
