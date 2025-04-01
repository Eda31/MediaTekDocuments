using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande (réunit les informations des classes LivreDvd et Suivi)
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// Identifiant de la commande
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Identifiant du livre ou dvd commandé
        /// </summary>
        public string IdLivreDvd { get; set; }

        /// <summary>
        /// Date de commande
        /// </summary>
        public DateTime DateCommande { get; set; }

        /// <summary>
        /// Montant de la commande
        /// </summary>
        public int Montant { get; set; }

        /// <summary>
        /// Nombre d'exemplaires commandés
        /// </summary>
        public int NbExemplaire { get; set; }

        /// <summary>
        /// Identifiant du suivi
        /// </summary>
        public int SuiviId { get; set; }

        /// <summary>
        /// Etape du suivi
        /// </summary>
        public string EtapeSuivi { get; set; }

        /// <summary>
        /// Constructeur de la classe Commande
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idLivreDvd"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        /// <param name="nbExemplaire"></param>
        /// <param name="suiviId"></param>
        /// <param name="etapeSuivi"></param>
        public Commande(string id, string idLivreDvd, DateTime dateCommande, int montant, int nbExemplaire, int suiviId, string etapeSuivi)
        {
            Id = id;
            IdLivreDvd = idLivreDvd;
            DateCommande = dateCommande;
            Montant = montant;
            NbExemplaire = nbExemplaire;
            SuiviId = suiviId;
            EtapeSuivi = etapeSuivi;
        }
    }
}
