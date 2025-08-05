using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.DataAccess.Repositories
{
    public abstract class MasterRepository : Repository
    {
        //Exécuter des procédures pour insérer, modifier et supprimer-> PAR DÉFAUT POUR LA PROCÉDURE STOCKÉE
        protected int ExecuteNonQuery(string sqlCommand, List<SqlParameter> parameters)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sqlCommand;
                    command.CommandType = CommandType.StoredProcedure;//(M-T)Tu peux modifier au TEXTE si tu veux exécuter par défaut seulement Commandes de texte sql (insert into, update...)->command.CommandType = CommandType.Text;
                    foreach (SqlParameter item in parameters)
                        command.Parameters.Add(item);
                    var result = command.ExecuteNonQuery();
                    parameters.Clear();
                    return result;
                }
            }
        }

        //EXÉCUTER DES COMMANDES MIXTE
        //EXÉCUTER LES COMMANDES Insérer, éditer et supprimer MIXTE(PROCÉDURE, TEXTE OU TABLEAU DIRECT)
        //DANS LE CAS OU VOUS SOUHAITEZ PRÉCISER LE TYPE DE COMMANDE DES DÉPOSITAIRES DES ENTITYS, PAR EXEMPLE:
        //->EXÉCUTEZ UNE COMMANDE DE TEXTE ou SQL
        //-> return ExecuteNonQuery("insert into tabla values (@param1, @param2)", paramètres,CommandType.Text);
        //Vous pouvez voir le code d'implémentation  dans la classe UserRepository.   
        //CETTE MÉTHODE EST FACULTATIVE, SI VOUS N'UTILISEZ QUE DES PROCÉDURES STOCKÉES
        //(Si vous avez modifié la méthode ci-dessus comme je l'ai indiqué), VOUS POUVEZ SUPPRIMER CETTE MÉTHODE.
        protected int ExecuteNonQuery(string sqlCommand, List<SqlParameter> parameters, CommandType cmdType)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sqlCommand;
                    command.CommandType = cmdType;//Type de commande spécifié à partir des référentiels d'entity
                    foreach (SqlParameter item in parameters)
                        command.Parameters.Add(item);
                    var result = command.ExecuteNonQuery();
                    parameters.Clear();
                    return result;
                }
            }
        }

        // Exécuter des commandes de requête sans paramètres/Tout sélectionner' -> DÉFAUT DE PROCÉDURE
        protected DataTable ExecuteReader(string sqlCommand)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sqlCommand;
                    command.CommandType = CommandType.StoredProcedure;//Procédure stockée de type de commande
                    var reader = command.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);
                    reader.Dispose();
                    return table;
                }
            }
        }

        // Exécuter des commandes de requête avec des paramètres/Rechercher ou filtrer'-> par PROCÉDURE
        protected DataTable ExecuteReader(string sqlCommand, List<SqlParameter> parameters)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sqlCommand;
                    foreach (SqlParameter item in parameters)
                        command.Parameters.Add(item);
                    command.CommandType = CommandType.StoredProcedure;//Procédure stockée de type de commande
                    var reader = command.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);
                    reader.Dispose();
                    return table;
                }
            }
        }
        //EXÉCUTER DES COMMANDES MIXTES POUR LA CONSULTATION
        //FACULTATIF
        //VOUS POUVEZ SURCHARGER CES 2 DERNIÈRES MÉTHODES DE LA MÊME FAÇON QUE AVEC LA MÉTHODE ExecuteNonQuery() 
        //POUR QUE DANS LE CAS VOUS SOUHAITEZ PRÉCISER LE TYPE DE COMMANDE DES DÉPOSITAIRES DES ENTITY
        //EXEMPLE- > 
        //protégé DataTable ExecuteReader(string sqlCommand, List<SqlParameter> paramètres,CommandType cmdType)
        //protégé DataTable ExecuteReader(string sqlCommand, CommandType cmdType)

    }
}
