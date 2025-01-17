using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using Projet1.Entite;

namespace Projet1 {

    public class Modele {
        private readonly SqliteConnection Connection;

        private readonly string URL = "Data Source=/Users/dusunceli/Documents/B3 DEV/Projet1/Modele/Database.db";

        public Modele()
        {
            this.Connection = new SqliteConnection(this.URL);
        }

        private void OpenConnection()
        {
            if(this.Connection.State == System.Data.ConnectionState.Closed) {
                this.Connection.Open();
            }
        }

        private void CloseConnection()
        {
            if(this.Connection.State == System.Data.ConnectionState.Open){
                this.Connection.Close();
            }
        }

        public int CheckConnexionInfo(string login, string password) 
        {
            try
            {
                this.OpenConnection();
                using (SqliteCommand command = this.Connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(id) FROM Utilisateur WHERE login = @LOGIN AND password = @PASSWORD;";
                    command.Parameters.AddWithValue("@LOGIN", login);
                    command.Parameters.AddWithValue("@PASSWORD", password); // Considère de hacher le mot de passe

                    // Utiliser ExecuteScalar pour obtenir le résultat
                    object? result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la vérification des infos de connexion : {ex.Message}");
                return 0;
            }
            finally 
            {
                this.CloseConnection();
            }
        }

        public int InsertIntoUtilisateur(Utilisateur User)
        {
            try {
                this.OpenConnection();
                SqliteCommand Command = this.Connection.CreateCommand();
                Command.CommandText = "INSERT INTO Utilisateur(nom, prenom, login, password) VALUES(@nom, @prenom, @password, @login)";
                Command.Parameters.AddWithValue("nom", User.GetNom());
                Command.Parameters.AddWithValue("prenom", User.GetPrenom());
                Command.Parameters.AddWithValue("login", User.GetLogin());
                Command.Parameters.AddWithValue("password", User.GetPassword()); 

                return Command.ExecuteNonQuery(); 

            } catch (Exception ex){
                Console.WriteLine($"Error during insert into user : {ex.Message}");
                return 0;
            } finally {
                this.CloseConnection();
            }
        }

        public Utilisateur? GetUserByLoginAndPasswod(string login, string password)
        {
            this.OpenConnection();
            try
            {
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM Utilisateur WHERE login = @LOGIN AND password = @PASSWORD;";
                command.Parameters.AddWithValue("@LOGIN", login);
                command.Parameters.AddWithValue("@PASSWORD", password);
                SqliteDataReader ResultSet = command.ExecuteReader();
                while(ResultSet.Read()){
                    return new Utilisateur(
                        ResultSet.GetInt32(0),
                        ResultSet.GetString(1),
                        ResultSet.GetString(2),
                        ResultSet.GetString(3),
                        ResultSet.GetString(4)
                    );
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération de l'utilisateur {ex.Message}");
                return null;
            } finally {
                this.CloseConnection();
            }
        }

        public int DeleteFromUtilisateur(int idUser)
        {
            try {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "DELETE FROM Utilisateur WHERE id = @id;";
                command.Parameters.AddWithValue("id", idUser);
                return command.ExecuteNonQuery();
            } catch (Exception ex){
                Console.WriteLine($"Erreur lors de la suppression d'un user via son id : {ex.Message}");
                return 0;
            } finally {
                this.CloseConnection();
            }
        }

        public int checkIfUserConnected(int id_user)
        {
            try {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "SELECT COUNT(id_user) FROM ConnectedUser WHERE id_user = @id;";
                command.Parameters.AddWithValue("id", id_user);
                object? result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            } catch (Exception ex){
                Console.WriteLine($"Erreur lors de la recherche d'un user connecter via son id : {ex.Message}");
                return 0;
            } finally {
                this.CloseConnection();
            }
        }

        public List<Article> GetAllArticles()
        {
            try{
                this.OpenConnection();
                List<Article> lesArticles = [];
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM Article";
                SqliteDataReader resultSet = command.ExecuteReader();
                while(resultSet.Read()){
                    lesArticles.Add(new Article(
                        resultSet.GetInt32(0), 
                        resultSet.GetString(1), 
                        resultSet.GetString(2), 
                        resultSet.GetDouble(3), 
                        resultSet.GetFieldValue<byte[]>(4), 
                        resultSet.GetInt32(5)
                    ));
                }
                return lesArticles;
            } catch (Exception ex){
                Console.WriteLine($"Erreur lors de la récupération des articles depuis la BDD : {ex.Message}");
                return [];
            } finally {
                this.CloseConnection();
            }
        }

        public int InsertIntoArticle(Article unArticle)
        {
            try {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "INSERT INTO Article(nom, description, prix, image, id_categorie) VALUES(@nom, @description, @prix, @image, @id_categ);";
                command.Parameters.AddWithValue("nom", unArticle.Nom);
                command.Parameters.AddWithValue("description", unArticle.Description);
                command.Parameters.AddWithValue("prix", unArticle.Prix);
                command.Parameters.AddWithValue("image", unArticle.DataImage);
                command.Parameters.AddWithValue("id_categ", unArticle.IdCategorie);
                return command.ExecuteNonQuery();
            } catch (Exception ex){
                Console.WriteLine($"Erreur lors de l'insertion d'un article dans la BDD : {ex.Message}");
                return 0;
            } finally {
                this.CloseConnection();
            }
        }

        public bool DeleteFromArticle(int idArticle)
        {
            try {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "DELETE FROM Article WHERE id = @id;";
                command.Parameters.AddWithValue("id", idArticle);
                return command.ExecuteNonQuery() > 0;
            } catch (Exception ex){
                Console.WriteLine($"Erreur lors de la suppression d'un article dans la BDD : {ex.Message}");
                return false;
            } finally {
                this.CloseConnection();
            }
        }

        public bool DeleteArticleAndArchive(int articleId)
        {
            try
            {
                this.OpenConnection();

                SqliteCommand selectCmd = this.Connection.CreateCommand();
                selectCmd.CommandText = @"
                    SELECT id, nom, description, prix, image, id_categorie 
                    FROM Article 
                    WHERE id = @articleId;";
                selectCmd.Parameters.AddWithValue("@articleId", articleId);

                using (var reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SqliteCommand insertCmd = this.Connection.CreateCommand();
                        insertCmd.CommandText = @"
                            INSERT INTO Article_Historique (id, nom, description, prix, image, id_categorie) 
                            VALUES (@id, @nom, @description, @prix, @image, @id_categorie);";
                        insertCmd.Parameters.AddWithValue("@id", reader.GetInt32(0));
                        insertCmd.Parameters.AddWithValue("@nom", reader.GetString(1));
                        insertCmd.Parameters.AddWithValue("@description", reader.GetString(2));
                        insertCmd.Parameters.AddWithValue("@prix", reader.GetDouble(3));
                        insertCmd.Parameters.AddWithValue("@image", reader["image"]);
                        insertCmd.Parameters.AddWithValue("@id_categorie", reader.GetInt32(5));
                        insertCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        Console.WriteLine("Article not found.");
                        return false;
                    }
                }

                SqliteCommand deleteCmd = this.Connection.CreateCommand();
                deleteCmd.CommandText = "DELETE FROM Article WHERE id = @articleId;";
                deleteCmd.Parameters.AddWithValue("@articleId", articleId);
                deleteCmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during article deletion and archiving: {ex.Message}");
                return false;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public int InsertIntoConnectedUser(int idUser)
        {
            try {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "INSERT INTO ConnectedUser VALUES(@id_user);";
                command.Parameters.AddWithValue("id_user", idUser);
                return command.ExecuteNonQuery();
            } catch (Exception ex){
                Console.WriteLine($"Erreur lors de l'insertion d'un user dans connected user via son id : {ex.Message}");
                return 0;
            } finally {
                this.CloseConnection();
            }
        }

        public int DeleteFromConnectedUser(int idUser)
        {
            try {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "DELETE FROM ConnectedUser WHERE id_user = @id;";
                command.Parameters.AddWithValue("id", idUser);
                return command.ExecuteNonQuery();
            } catch (Exception ex){
                Console.WriteLine($"Erreur lors de la suppression d'un user via son id : {ex.Message}");
                return 0;
            } finally {
                this.CloseConnection();
            }
        }

        public List<Utilisateur> GetAllUser()
        {
            List<Utilisateur> lesUsers = [];

            try
            {
                this.OpenConnection();

                SqliteCommand command = this.Connection.CreateCommand();

                command.CommandText = "SELECT * FROM Utilisateur;";

                SqliteDataReader ResultSet = command.ExecuteReader();
                
                while(ResultSet.Read()){
                    lesUsers.Add(new Utilisateur(
                        ResultSet.GetInt32(0),
                        ResultSet.GetString(1),
                        ResultSet.GetString(2),
                        ResultSet.GetString(3),
                        ResultSet.GetString(4))
                    );
                }
                return lesUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY {ex.Message}");
                lesUsers.Clear();
                return lesUsers;
            } finally {
                this.CloseConnection();
            }
        }

        public List<CategorieArticle> GetAllCategorieArticle()
        {
            List<CategorieArticle> lesCateg = [];
            try
            {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM CategorieArticle;";
                SqliteDataReader ResultSet = command.ExecuteReader();
                while(ResultSet.Read()){
                    lesCateg.Add(new CategorieArticle(
                            ResultSet.GetInt32(0),
                            ResultSet.GetString(1)
                        )
                    );
                }
                return lesCateg;
            } catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY {ex.Message}");
                lesCateg.Clear();
                return lesCateg;
            } finally {
                this.CloseConnection();
            }
        }

        public bool InsertIntoCategorieArticle(CategorieArticle categArticle) 
        {
            try
            {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "INSERT INTO CategorieArticle(designation) VALUES(@designation)";
                command.Parameters.AddWithValue("designation", categArticle.Designation);
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY {ex.Message}");
                return false;
            } finally {
                this.CloseConnection();
            }
        }

        public bool DeleteFromCategorieArticle(int idCateg) 
        {
            try
            {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "DELETE FROM CategorieArticle WHERE id_categorie = @id";
                command.Parameters.AddWithValue("id", idCateg);
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY {ex.Message}");
                return false;
            } finally {
                this.CloseConnection();
            }
        }

        public bool InsertIntoPanier(Panier panier)
        {
            try {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "INSERT INTO Panier VALUES(@id_user);";
                command.Parameters.AddWithValue("id_user", panier.IdUser);
                return command.ExecuteNonQuery() > 0;
            } catch (Exception ex) {
                Console.WriteLine($"Error during SQL QUERY {ex.Message}");
                return false;
            } finally {
                this.CloseConnection();
            }
        }

        public List<Article> GetArticleByIdUser(int id_user) 
        {
             List<Article> lesArticles = [];
            try
            {
               this.OpenConnection();
               SqliteCommand command = this.Connection.CreateCommand();
               command.CommandText = "SELECT a.* FROM Article a, PanierArticle p WHERE p.id_user = @id_user AND p.id_article = a.id;";
               command.Parameters.AddWithValue("id_user", id_user);
               SqliteDataReader resultSet = command.ExecuteReader();
                while(resultSet.Read()){
                    lesArticles.Add(new Article(
                        resultSet.GetInt32(0), 
                        resultSet.GetString(1), 
                        resultSet.GetString(2), 
                        resultSet.GetDouble(3), 
                        resultSet.GetFieldValue<byte[]>(4), 
                        resultSet.GetInt32(5)
                    ));
                }
                Console.WriteLine(lesArticles.Count);
                return lesArticles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY {ex.Message}");
                lesArticles.Clear();
                return lesArticles;
            } finally {
                this.CloseConnection();
            }
        }

        public Dictionary<bool,int> InsertArticleIntoPanierArticle(int id_user, int id_article)
        {
            try
            {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "INSERT INTO PanierArticle(id_user, id_article) VALUES(@id_user, @id_article)";
                command.Parameters.AddWithValue("id_user", id_user);
                command.Parameters.AddWithValue("id_article", id_article);
                Dictionary<bool, int> returnValue = new Dictionary<bool, int>();
                returnValue.Add(command.ExecuteNonQuery() > 0, 0);
                return returnValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY {ex.Message}");
                Dictionary<bool, int> returnValue = new Dictionary<bool, int>();
                returnValue.Add(false, 1);
                return returnValue;
            } finally {
                this.CloseConnection();
            }
        }

       public bool DeleteArticleFromPanier(int id_user)
        {
            try
            {
                this.OpenConnection();
                List<int> idArticleToDelete = new List<int>();

                using (SqliteCommand selectCommand = this.Connection.CreateCommand())
                {
                    selectCommand.CommandText = "SELECT id_article FROM PanierArticle WHERE id_user = @id_user";
                    selectCommand.Parameters.AddWithValue("@id_user", id_user);

                    using (SqliteDataReader resultSet = selectCommand.ExecuteReader())
                    {
                        while (resultSet.Read())
                        {
                            idArticleToDelete.Add(resultSet.GetInt32(0));
                        }
                    }
                }

                using (SqliteCommand deletePanierCommand = this.Connection.CreateCommand())
                {
                    deletePanierCommand.CommandText = "DELETE FROM PanierArticle WHERE id_user = @id_user";
                    deletePanierCommand.Parameters.AddWithValue("@id_user", id_user);

                    if (deletePanierCommand.ExecuteNonQuery() <= 0)
                    {
                        Console.WriteLine("Aucun article supprimé du panier.");
                        return false;
                    }
                }

                if (idArticleToDelete.Count > 0)
                {
                    for(int i = 0; i < idArticleToDelete.Count; i++){
                        this.DeleteArticleAndArchive(idArticleToDelete[i]);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY: {ex.Message}");
                return false;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public bool DeleteArticleFromMyPanier(int id_user, int id_article) {
            try
            {
                this.OpenConnection();

                using (SqliteCommand deletePanierCommand = this.Connection.CreateCommand())
                {
                    deletePanierCommand.CommandText = "DELETE FROM PanierArticle WHERE id_user = @id_user";
                    deletePanierCommand.Parameters.AddWithValue("@id_user", id_user);

                    if (deletePanierCommand.ExecuteNonQuery() <= 0)
                    {
                        Console.WriteLine("Aucun article supprimé du panier.");
                        return false;
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY: {ex.Message}");
                return false;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public bool IsItemInUrCard(int idUser, int idArticle)
        {
            try
            {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "SELECT COUNT(id_article) FROM PanierArticle WHERE id_user = @id_user AND id_article = @id_article;";
                command.Parameters.AddWithValue("@id_user", idUser);
                command.Parameters.AddWithValue("@id_article", idArticle);
                int count = Convert.ToInt32(command.ExecuteScalar());

                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY: {ex.Message}");
                return false;
            } 
            finally 
            {
                this.CloseConnection();
            }
        }
    
        public bool InsertIntoFacturation(List<Article> lesArticles, Facturation facturation)
        {
            try
            {
                this.OpenConnection();

                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "INSERT INTO Facturation (id_user, montant_facturation, date_facturation) VALUES (@id_user, @montant_transaction, @dateTransaction)";
                command.Parameters.AddWithValue("id_user", facturation.IdUser);
                command.Parameters.AddWithValue("montant_transaction", facturation.MontantFacturation);
                command.Parameters.AddWithValue("@dateTransaction", facturation.DateTransaction.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();

                SqliteCommand getFactureIdCmd = this.Connection.CreateCommand();
                getFactureIdCmd.CommandText = "SELECT MAX(id_facturation) FROM Facturation;";
                int factureId = Convert.ToInt32(getFactureIdCmd.ExecuteScalar());

                foreach (var article in lesArticles)
                {
                    SqliteCommand insertLigneCmd = this.Connection.CreateCommand();
                    insertLigneCmd.CommandText = @"
                        INSERT INTO Ligne_Facturation (id_user, id_facturation, id_article, montant_article) 
                        VALUES (@id_user, @id_facturation, @id_article, @montant_article);";
                    insertLigneCmd.Parameters.AddWithValue("@id_user", facturation.IdUser);
                    insertLigneCmd.Parameters.AddWithValue("@id_facturation", factureId);
                    insertLigneCmd.Parameters.AddWithValue("@id_article", article.Id);
                    insertLigneCmd.Parameters.AddWithValue("@montant_article", article.Prix);
                    insertLigneCmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY: {ex.Message}");
                return false;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public List<Facturation> GetAllFacturationByIdUser(int idUser) {
            List<Facturation> lesFacturations = [];
            try
            {
                this.OpenConnection();

                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM Facturation WHERE id_user = @id_user";
                command.Parameters.AddWithValue("id_user", idUser);
                SqliteDataReader tabRes = command.ExecuteReader();

                while(tabRes.Read()){
                    lesFacturations.Add(
                        new Facturation(
                            idUser,
                            tabRes.GetInt32("id_facturation"),
                            tabRes.GetDecimal("montant_facturation"),
                            tabRes.GetDateTime("date_facturation")
                        )
                    );
                }
 
                return lesFacturations;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY: {ex.Message}");
                lesFacturations.Clear();
                return lesFacturations;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public List<Article> GetArticleByIdFacturation(int? idFacturation){

            List<Article> articles = new List<Article>();
            try
            {
                this.OpenConnection();

                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = @"
                    SELECT 
                        a.id,
                        a.nom,
                        a.description,
                        a.prix,
                        a.image,
                        a.id_categorie
                    FROM 
                        Ligne_Facturation lf
                    JOIN 
                        Article_Historique a ON lf.id_article = a.id
                    WHERE 
                        lf.id_facturation = @id_facturation";
                command.Parameters.AddWithValue("id_facturation", idFacturation);
                
                SqliteDataReader tabRes = command.ExecuteReader();

                while (tabRes.Read())
                {
                    articles.Add(
                        new Article(
                            tabRes.GetInt32("id"),
                            tabRes.GetString("nom"),
                            tabRes.GetString("description"),
                            tabRes.GetDouble("prix"),
                            (byte[])tabRes["image"],
                            tabRes.GetInt32("id_categorie")
                        )
                    );
                }

                return articles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SQL QUERY: {ex.Message}");
                articles.Clear();
                return articles;
            }
            finally
            {
                this.CloseConnection();
            }
        }

    }
}