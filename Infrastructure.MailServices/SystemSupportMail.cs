using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MailServices
{
    public class SystemSupportMail : MasterMailServer
    {
        public SystemSupportMail()
        {
            // Modifier les paramètres de connexion au serveur de messagerie sortant (smtpClient), 
            // de cette classe avec vos données e-mail d'expéditeur (Responsable de l'envoi des emails).
            // Si vous utilisez Gmail comme e-mail d'expéditeur, Vous devez autoriser l'accès aux applications
            // Ver : https://support.microsoft.com/es-pe/help/2758902/how-to-set-up-an-internet-email-account-in-outlook-2013-or-2016
            // non sécurisé dans l'e-mail afin que votre application puisse envoyer des e-mails.
            // Ver : Accédez à votre compte
            // Vas au : Gérer votre compte Google // Sécurité  // Accès moins sécurisé des applications//
            // et activer 

            initializeSmtpClient(
                senderMail: "yassiramid1@gmail.com", //Placez l'adresse e-mail qui enverra les messages ici // Place the email address that will send the messages here
                password: "IloveYouYa:*",
                host: "smtp.gmail.com",
                port: 587,
                ssl: true);

           
        }


    }
}
