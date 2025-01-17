using System;
using System.Collections.Generic;

namespace Projet1.Entite {
    public class Facturation
    {
        public int IdUser { get; set; }
        public int? IdFacturation { get; set; }
        public decimal MontantFacturation { get; set; }
        public DateTime DateTransaction { get; set; }

        public List<Article>? LesArticles {get; set;}

        public Facturation(int idUser, int? idFacturation, decimal montantFacturation, DateTime dateTransaction)
        {
            IdUser = idUser;
            IdFacturation = idFacturation;
            MontantFacturation = montantFacturation;
            DateTransaction = dateTransaction;
            this.LesArticles = null;
        }
    }
}