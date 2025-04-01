namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeDocument (réunit les informations des classes Commande, Suivi et LivreDvd)
    /// </summary>
    public class CommandeDocument
    {
        public string Id { get; set; }
        public int NbExemplaire { get; set; }
        public string IdCommande { get; set; }
        public string IdSuivi { get; set; }
        public string IdLivreDvd { get; set; }

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

