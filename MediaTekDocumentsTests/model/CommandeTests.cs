using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace MediaTekDocuments.model.Tests
{
    /// <summary>
    /// Classe de test de la classe Commande.
    /// </summary>
    [TestClass()]
    public class CommandeTests
    {
        private const string id = "CMD001";
        private const string idLivreDvd = "LD001";
        private const string dateCommande = "2024-01-01";
        private const int montant = 20;
        private const int nbExemplaire = 5;
        private const int suiviId = 1;
        private const string etapeSuivi = "En cours";

        private static readonly Commande commande = new Commande(
            id,
            idLivreDvd,
            DateTime.Parse(dateCommande, CultureInfo.InvariantCulture),
            montant,
            nbExemplaire,
            suiviId,
            etapeSuivi
        );

        [TestMethod()]
        public void CommandeTest()
        {
            Assert.AreEqual(id, commande.Id);
            Assert.AreEqual(idLivreDvd, commande.IdLivreDvd);
            Assert.AreEqual(DateTime.Parse(dateCommande), commande.DateCommande);
            Assert.AreEqual(montant, commande.Montant);
            Assert.AreEqual(nbExemplaire, commande.NbExemplaire);
            Assert.AreEqual(suiviId, commande.SuiviId);
            Assert.AreEqual(etapeSuivi, commande.EtapeSuivi);
        }
    }
}