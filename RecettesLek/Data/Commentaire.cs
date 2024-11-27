namespace RecettesLek.Data
{
    public class Commentaire
    {
        public int CommentaireId { get; set; } 

        public int RecetteId { get; set; }  

        public string UserId{  get; set; } 

        public string contenuCommentaire { get; set; }  

        public virtual Recette Recette { get; set; }    

        public virtual Utilisateur User { get; set; }
    }
}
