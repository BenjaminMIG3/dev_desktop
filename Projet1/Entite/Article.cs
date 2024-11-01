namespace Projet1 {
    public class Article {
        private int id;
        private string Nom;
        private string description;
        private double prix;
        private byte[] dataImage;

        public Article(int id, string nom, string description, double prix, byte[] dataImage) {
            this.id = id;
            this.Nom = nom;
            this.description = description;
            this.prix = prix;
            this.dataImage = dataImage;
        }

        public Article(string titre, string description, double prix, byte[] dataImage) {
            this.Nom = titre;
            this.description = description;
            this.prix = prix;
            this.dataImage = dataImage;
        }

        public int GetId() {
            return id;
        }

        public void SetId(int value) {
            id = value;
        }

        public string GetTitre() {
            return Nom;
        }

        public void SetTitre(string value) {
            Nom = value;
        }

        public string GetDescription() {
            return description;
        }

        public void SetDescription(string value) {
            description = value;
        }

        public double GetPrix() {
            return prix;
        }

        public void SetPrix(double value) {
            prix = value;
        }

        public byte[] GetDataImage() {
            return dataImage;
        }

        public void SetDataImage(byte[] value) {
            dataImage = value;
        }
    }
}
