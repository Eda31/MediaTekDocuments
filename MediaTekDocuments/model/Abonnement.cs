using System;

namespace MediaTekDocuments.model
{
    public class Abonnement
    {
        public string Id { get; set; }
        public string IdRevue { get; set; }
        public DateTime DateCommande { get; set; }
        public int Montant { get; set; }
        public DateTime DateFinAbonnement { get; set; }
        public string TitreRevue { get; set; }

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