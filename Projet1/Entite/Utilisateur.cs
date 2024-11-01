namespace Projet1.Entite
{
        public class Utilisateur {
        private string Nom,Prenom,Login,Password;
        private int Id;

        public Utilisateur(int id, string nom, string prenom, string login, string password){
            this.Id = id;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Login = login;
            this.Password = password;
        }

        public int GetId(){
            return this.Id;
        }

        public void SetId(int id){
            this.Id = id;
        }

        public string GetNom(){
            return this.Nom;
        }

        public void SetNom(string prenom){
            this.Nom = prenom;
        }

        public string GetPrenom(){
            return this.Prenom;
        }

        public void SetPrenom(string prenom){
            this.Prenom = prenom;
        }

        public string GetLogin(){
            return this.Login;
        }

        public void SetLogin(string login){
            this.Login = login;
        }

        public string GetPassword(){
            return this.Password;
        }

        public void SetPassword(string password){
            this.Password = password;
        }

        public override string ToString(){
            return $"[ id : {this.Id}, nom : {this.Nom}, prenom : {this.Prenom}, login : {this.Login}, password : {this.Password} ]";
        }
    }
}