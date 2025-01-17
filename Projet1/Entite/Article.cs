namespace Projet1.Entite {

    public class Article {
        public int Id {get; set;}
        public string Nom {get; set;}
        public string Description {get; set;}

        public double Prix {get; set;}

        public byte[] DataImage {get; set;}

        public int IdCategorie {get; set;}

        public Article(int id, string nom, string description, double prix, byte[] dataImage){
            Id = id;
            Nom = nom;
            Description = description;
            Prix = prix;
            DataImage = dataImage;
        }

        public Article(string nom, string description, double prix, byte[] dataImage){
            Nom = nom;
            Description = description;
            Prix = prix;
            DataImage = dataImage;
        }

        public Article(string nom, string description, double prix, byte[] dataImage, int idCategorie){
            Nom = nom;
            Description = description;
            Prix = prix;
            DataImage = dataImage;
            IdCategorie = idCategorie;
        }

        public Article(int id, string nom, string description, double prix, byte[] dataImage, int idCategorie){
            Id = id;
            Nom = nom;
            Description = description;
            Prix = prix;
            DataImage = dataImage;
            IdCategorie = idCategorie;
        }

    }

}
