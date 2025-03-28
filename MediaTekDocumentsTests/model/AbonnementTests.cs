using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class AbonnementTests
    {
        private const string id = "100";
        private const string dateCommande = "2024-01-01";
        private const int montant = 100;
        private const string dateFinAbonnement = "2024-12-31";
        private const string idRevue = "R001";

        private static readonly DateTime parsedDateCommande = DateTime.Parse(dateCommande, CultureInfo.InvariantCulture);
        private static readonly DateTime parsedDateFinAbonnement = DateTime.Parse(dateFinAbonnement, CultureInfo.InvariantCulture);

        private static readonly Abonnement abonnement = new Abonnement(
            id,
            idRevue,
            parsedDateCommande,
            montant,
            parsedDateFinAbonnement
        );

        [TestMethod()]
        public void AbonnementTest()
        {
            Assert.AreEqual(id, abonnement.Id);
            Assert.AreEqual(idRevue, abonnement.IdRevue);
            Assert.AreEqual(parsedDateCommande, abonnement.DateCommande);
            Assert.AreEqual(montant, abonnement.Montant);
            Assert.AreEqual(parsedDateFinAbonnement, abonnement.DateFinAbonnement);
        }
    }
}