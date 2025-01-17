namespace Projet1.Entite {
    public class CategorieArticle {
        public int Id {get; set;}

        public string Designation {get; set;}

        public CategorieArticle(int id, string designation){
            Id = id;
            Designation = designation;
        }

        public CategorieArticle(string designation){
            Designation = designation;
        }
    }
}