using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement
    /// </summary>
    public class Abonnement
    {
        /// <summary>
        /// Identifiant de l'abonnement
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Identifiant de la revue
        /// </summary>
        public string IdRevue { get; set; }

        /// <summary>
        /// Date de commande de l'abonnement
        /// </summary>
        public DateTime DateCommande { get; set; }

        /// <summary>
        /// Montant de l'abonnement
        /// </summary>
        public int Montant { get; set; }

        /// <summary>
        /// Date de fin de l'abonnement
        /// </summary>
        public DateTime DateFinAbonnement { get; set; }

        /// <summary>
        /// Titre de la revue
        /// </summary>
        public string TitreRevue { get; set; }

        /// <summary>
        /// Constructeur de la classe Abonnement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idRevue"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        /// <param name="dateFinAbonnement"></param>
        public Abonnement(string id, string idRevue, DateTime dateCommande, int montant, DateTime dateFinAbonnement)
        {
            Id = id;
            IdRevue = idRevue;
            DateCommande = dateCommande;
            Montant = montant;
            DateFinAbonnement = dateFinAbonnement;
        }
    }

}