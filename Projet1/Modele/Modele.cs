using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Projet1.Entite;

namespace Projet1 {

    public class Modele {
        private readonly SqliteConnection Connection;

        private readonly string URL = "Data Source=/Users/dusunceli/Documents/B3 DEV/Projet1/Modele/Database.db";

        public Modele(){
            this.Connection = new SqliteConnection(this.URL);
        }

        private void OpenConnection(){
            if(this.Connection.State == System.Data.ConnectionState.Closed) {
                this.Connection.Open();
            }
        }

        private void CloseConnection(){
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

        /// <summary>
        /// Insère un utilisateur dans la base de données.
        /// </summary>
        /// <param name="user">L'objet utilisateur à insérer.</param>
        /// <returns>Retourne true si l'insertion a réussi, sinon false.</returns>
        public int InsertIntoUtilisateur(Utilisateur User){
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

        public Utilisateur? GetUserByLoginAndPasswod(string login, string password){
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

        /// <summary>
        /// Supprime un utilisateur dans la base de données.
        /// </summary>
        /// <param name="idUser">L'id utilisateur à supprimer.</param>
        /// <returns>Retourne 0 si échoué ou 1 sinon.</returns>
        public int DeleteFromUtilisateur(int idUser){
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

        public int checkIfUserConnected(int id_user){
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

        public List<Article>? GetAllArticles(){
            try{
                this.OpenConnection();
                List<Article> lesArticles = new List<Article>();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM Article";
                SqliteDataReader resultSet = command.ExecuteReader();
                while(resultSet.Read()){
                    lesArticles.Add(new Article(resultSet.GetInt32(0), resultSet.GetString(1), resultSet.GetString(2), resultSet.GetDouble(3), resultSet.GetFieldValue<byte[]>(4)));
                }
                return lesArticles;
            } catch (Exception ex){
                Console.WriteLine($"Erreur lors de la récupération des articles depuis la BDD : {ex.Message}");
                return null;
            } finally {
                this.CloseConnection();
            }
        }

        public int InsertIntoArticle(Article unArticle){
            try {
                this.OpenConnection();
                SqliteCommand command = this.Connection.CreateCommand();
                command.CommandText = "INSERT INTO Article(nom, description, prix, image) VALUES(@nom, @description, @prix, @image);";
                command.Parameters.AddWithValue("nom", unArticle.GetTitre());
                command.Parameters.AddWithValue("description", unArticle.GetDescription());
                command.Parameters.AddWithValue("prix", unArticle.GetPrix());
                command.Parameters.AddWithValue("image", unArticle.GetDataImage());
                return command.ExecuteNonQuery();
            } catch (Exception ex){
                Console.WriteLine($"Erreur lors de l'insertion d'un article dans la BDD : {ex.Message}");
                return 0;
            } finally {
                this.CloseConnection();
            }
        }

        public int InsertIntoConnectedUser(int idUser){
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

        public int DeleteFromConnectedUser(int idUser){
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

        /// <summary>
        /// Recupere les utilisateurs dans la base de données.
        /// </summary>
        /// <param></param>
        /// <returns>Retourne une liste d'utilisateur</returns>
        public List<Utilisateur> GetAllUser(){
            List<Utilisateur> lesUsers = new List<Utilisateur>();

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
    }
}