namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeDocument (réunit les informations des classes Commande, Suivi et LivreDvd)
    /// </summary>
    public class CommandeDocument
    {
        /// <summary>
        /// Identifiant de la commande
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nombre d'exemplaires commandés
        /// </summary>
        public int NbExemplaire { get; set; }

        /// <summary>
        /// Identifiant de la commande
        /// </summary>
        public string IdCommande { get; set; }

        /// <summary>
        /// Identifiant du suivi
        /// </summary>
        public string IdSuivi { get; set; }

        /// <summary>
        /// Identifiant du livre ou du DVD
        /// </summary>
        public string IdLivreDvd { get; set; }

        /// <summary>
        /// Constructeur de la classe CommandeDocument
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nbExemplaire"></param>
        /// <param name="idCommande"></param>
        /// <param name="idSuivi"></param>
        /// <param name="idLivreDvd"></param>
        public CommandeDocument(string id, int nbExemplaire, string idCommande, string idSuivi, string idLivreDvd)
        {
            Id = id;
            NbExemplaire = nbExemplaire;
            IdCommande = idCommande;
            IdSuivi = idSuivi;
            IdLivreDvd = idLivreDvd;
        }
    }
}

