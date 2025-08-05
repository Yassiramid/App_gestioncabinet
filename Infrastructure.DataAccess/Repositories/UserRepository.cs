using Infrastructure.Common.Cache;
using Infrastructure.DataAccess.Abstracts;
using Infrastructure.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.DataAccess.Repositories
{
    public class UserRepository : MasterRepository, IUserRepository
    {
        public int add(User entity)//ajouter un nouvel utilisateur
        {
            //Créer des paramètres et ajouter de la valeur
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@userName", entity.userName));
            parameters.Add(new SqlParameter("@password", entity.password));
            parameters.Add(new SqlParameter("@firstName", entity.firstName));
            parameters.Add(new SqlParameter("@lastName", entity.lastName));
            parameters.Add(new SqlParameter("@position", entity.position));
            parameters.Add(new SqlParameter("@email", entity.email));
            parameters.Add(new SqlParameter("@profile", entity.profile));
            //EXÉCUTER PROCÉDURE Stocké (ou COMMANDE DE TEXTe SQL au cas où vous auriez modifié el metodo principal- voir le point(M-T) de la clase MasterRepository)
            //Envoyer les paramètres
            return ExecuteNonQuery("addUser", parameters);
            //return ExecuteNonQuery("insert into Users values(@userName, @password, @firstName, @lastName, @position, @email, @profile)", paramètres);


            //EXÉCUTER UNE COMMANDE MIXTE(PROCÉDURE o requete)
            //return ExecuteNonQuery(
            //    "insert into Users values(@userName, @password, @firstName, @lastName, @position, @email, @profile)",
            //    paramètres,
            //    CommandType.Text);
            //ou
            // return ExecuteNonQuery("addUser", paramètres, CommandType.StoredProcedure);
        }

        public int edit(User entity)//modifier l'utilisateur
        {

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@userName", entity.userName));
            parameters.Add(new SqlParameter("@password", entity.password));
            parameters.Add(new SqlParameter("@firstName", entity.firstName));
            parameters.Add(new SqlParameter("@lastName", entity.lastName));
            parameters.Add(new SqlParameter("@position", entity.position));
            parameters.Add(new SqlParameter("@email", entity.email));
            parameters.Add(new SqlParameter("@profile", entity.profile));
            parameters.Add(new SqlParameter("@id", entity.id));
            return ExecuteNonQuery("editUser", parameters);
        }

        public IEnumerable<User> getAll()//obtenir toutes les données utilisateur
        {
            var result = ExecuteReader("selectAllUsers");
            var listUser = new List<User>();
            foreach (DataRow item in result.Rows)
            {
                var userEntity = new User();//créer un objet utilisateur
                userEntity.id = Convert.ToInt32(item[0]);
                userEntity.userName = item[1].ToString();
                userEntity.password = item[2].ToString();
                userEntity.firstName = item[3].ToString();
                userEntity.lastName = item[4].ToString();
                userEntity.position = item[5].ToString();
                userEntity.email = item[6].ToString();
                //nous téléchargeons la photo de profil tant qu'elle n'est pas nulle
                if (item[7] != DBNull.Value) userEntity.profile = (byte[])item[7];

                //ajouter un utilisateur à la liste
                listUser.Add(userEntity);
            }
            return listUser;
        }

        public IEnumerable<User> getByValue(string value)//recherche par valeur (recherche par nom d'utilisateur ou e-mail)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@findValue", value));
            var result = ExecuteReader("selectUser", parameters);
            var listUser = new List<User>();
            foreach (DataRow item in result.Rows)
            {
                var userEntity = new User();//créer un objet utilisateur
                userEntity.id = Convert.ToInt32(item[0]);
                userEntity.userName = item[1].ToString();
                userEntity.password = item[2].ToString();
                userEntity.firstName = item[3].ToString();
                userEntity.lastName = item[4].ToString();
                userEntity.position = item[5].ToString();
                userEntity.email = item[6].ToString();
                //nous téléchargeons la photo de profil tant qu'elle n'est pas nulle
                if (item[7] != DBNull.Value) userEntity.profile = (byte[])item[7];

                //ajouter un utilisateur à la liste
                listUser.Add(userEntity);
            }
            return listUser;
        }

        public bool login(string user, string pass)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@user", user));
            parameters.Add(new SqlParameter("@password", pass));

            var result = ExecuteReader("loginUser", parameters);

            if (result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows) //Charger les données utilisateur actives dans le cache
                {
                    ActiveUser.c_id = Convert.ToInt32(item[0]);
                    ActiveUser.c_userName = item[1].ToString();
                    ActiveUser.c_password = item[2].ToString();
                    ActiveUser.c_firstName = item[3].ToString();
                    ActiveUser.c_lastName = item[4].ToString();
                    ActiveUser.c_position = item[5].ToString();
                    ActiveUser.c_email = item[6].ToString();
                    //nous téléchargeons la photo de profil tant qu'elle n'est pas nulle
                    if (item[7] != DBNull.Value) ActiveUser.c_profile = (byte[])item[7];
                }                
                return true;
            }
            else
                return false;
        }

        public int remove(int id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@id", id));
            return ExecuteNonQuery("removeUser", parameters);
        }
    }
}
