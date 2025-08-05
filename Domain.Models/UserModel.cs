using Domain.ValueObjects;
using Infrastructure.DataAccess.Abstracts;
using Infrastructure.DataAccess.Entities;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.MailServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class UserModel
    {
        //Attributs
        private int _id;
        private string _userName;
        private string _password;
        private string _firstName;
        private string _lastName;
        private string _position;
        private string _email;
        private byte[] _profile;//photo de profil
        private EntityState _state;
        private bool _editMyUserProfile = false;
        private IUserRepository userRepository;

        // Les constructeurs 
        public UserModel()
        {
            userRepository = new UserRepository();
        }

        //Propriétés, validations de données et modèle d'affichage
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        [Required(ErrorMessage = "The Field Username is requerid")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must contain a minimum of 3 characters.")]
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        [Required(ErrorMessage = "The Field password is requerid")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password must contain a minimum of 5 characters.")]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        [Required(ErrorMessage = "The Field first name is requerid")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "First name must be only letters")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Firt name must contain a minimum of 3 characters.")]
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        [Required]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Last name must be only letters")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Last name must contain a minimum of 3 characters.")]
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }

        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        [Required]
        [EmailAddress]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        public byte[] Profile
        {
            get
            {
                return _profile;
            }

            set
            {
                _profile = value;
            }
        }

        public EntityState State
        {
            private get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public bool editMyUserProfile
        {
            private get
            {
                return _editMyUserProfile;
            }
            set
            {
                _editMyUserProfile = value;
            }
        }



        //Méthodes, comportements

        public string saveChanges()//insérer, modifier ou supprimer
        {
            string result = null;
            try
            {
                switch (State)
                {
                    case EntityState.Added:
                        userRepository.add(userEntity());
                        result = "Successfully recorded";
                        break;
                    case EntityState.Edited:
                        userRepository.edit(userEntity());
                        if ((editMyUserProfile == true)) //Si l'utilisateur est celui qui édite ses données, nous nous reconnecterons avec les nouvelles données pour mettre à jour les données du cache et donc aussi mettre à jour dans l'interface graphique
                        {
                            logIn(UserName, Password);
                        }

                        result = "Successfully edited";
                        break;
                    case EntityState.Removed:
                        userRepository.remove(Id);
                        result = "Successfully removed";
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Data.SqlClient.SqlException sqlEx = ex as System.Data.SqlClient.SqlException;
                if (sqlEx != null && sqlEx.Number == 2627)
                {
                    result = "[ : ( ] UPS¡¡ Duplicate record \n user name is already registered, try another";
                }
                else {
                    result = ex.ToString();
                }

            }

            return result;

        }
        private User userEntity()//créer un objet ->entity user
        {
            var userObject = new User();
            userObject.id = Id;
            userObject.userName = UserName;
            userObject.password = Password;
            userObject.firstName = FirstName;
            userObject.lastName = LastName;
            userObject.position = Position;
            userObject.email = Email;
            userObject.profile = Profile;

            return userObject;
        }

        public bool logIn(string user, string pass)//commencer la session
        {

            try
            {
                return userRepository.login(user, pass);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error as ocurred " + ex.ToString());
            }
        }

        public string recoverPassword(string requestingUser)//récupérer le mot de passe par mail
        {
            var user = userRepository.getByValue(requestingUser);//rechercher un utilisateur par e-mail
            if (user != null)
            {
                foreach (User item in user)
                {
                    FirstName = item.firstName + ", " + item.lastName;
                    Email = item.email;
                    Password = item.password;
                }

                var mailServices = new SystemSupportMail();
                mailServices.sendMail(//enviar correo al usuario
                    subject: "SYSTEM: Password recovery request",
                    body: "Hi " + FirstName + "\nyou requested to recover your password.\nYour current password is: " + Password + "\nHowever, we ask that you change your password immediately once you enter the system",
                    recipientMail: new List<string>() { Email
                    });

                return "Hi " + FirstName + "\nyou requested to recover your password.\n Please check your email: " + Email + "\nHowever, we ask that you change your password immediately once you enter the system";
            }
            else
                return "Sorry, you do not have an account with that email or username.";
        }

        public List<UserModel> getAllUsers()//obtenir tous les enregistrements utilisateur
        {
            var result = userRepository.getAll();
            var userModelList = new List<UserModel>();

            foreach (User item in result)
                userModelList.Add(new UserModel()
                {
                    Id = item.id,
                    UserName = item.userName,
                    Password = item.password,
                    FirstName = item.firstName,
                    LastName = item.lastName,
                    Position = item.position,
                    Email = item.email,
                    Profile = item.profile
                });

            return userModelList;
        }

        public List<UserModel> getAUserByValue(string value)//rechercher ou filtrer l'utilisateur
        {
            var result = userRepository.getByValue(value);
            var userModelList = new List<UserModel>();

            foreach (User item in result)
                userModelList.Add(new UserModel()
                {
                    Id = item.id,
                    UserName = item.userName,
                    Password = item.password,
                    FirstName = item.firstName,
                    LastName = item.lastName,
                    Position = item.position,
                    Email = item.email,
                    Profile = item.profile
                });

            return userModelList;
        }
    }
}
