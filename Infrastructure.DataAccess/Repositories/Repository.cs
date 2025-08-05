using System.Configuration;
using System.Data.SqlClient;

namespace Infrastructure.DataAccess.Repositories
{
    public abstract class Repository
    {
        private readonly string connectionString;

        public Repository()
        {
            //Vous pouvez modifier cette chaîne de connexion dans le fichier de configuration de l'application(App.Config - Cape UI)
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionToSql"].ToString();
            //o Vous pouvez activer la ligne ci-dessous si vous souhaitez avoir la chaîne de connexion dans cette classe
            //connectionString = "Server=.\\SQLEXPRESS;DataBase= MyCompanyTest; integrated security= true";
        }
        protected SqlConnection 
            GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
