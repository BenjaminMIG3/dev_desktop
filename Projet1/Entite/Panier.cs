using System.Collections.Generic;

namespace Projet1.Entite {
    public class Panier {
        public int IdUser {get; set;}

        public double Total {get; set;}

        public List<Article> SesArticles {get; set;}

        public Panier(int idUser){
            this.IdUser = idUser;
            this.SesArticles = [];
        }

        public Panier(int idUser,double total){
            this.IdUser = idUser;
            this.Total = total;
            this.SesArticles = [];
        }
    }
}