using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MediaTekDocuments.test
{
    [TestClass] // Assurez-vous que ce soit bien écrit ainsi
    public class ParutionDansAbonnementTests
    {
        [TestMethod]
        public void Test_ParutionDansAbonnement_DansIntervalle()
        {
            DateTime dateCommande = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateFin = new DateTime(2024, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            DateTime dateParution = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);

            bool result = ParutionDansAbonnement(dateCommande, dateFin, dateParution);

            Assert.IsTrue(result); // Vérifier si c'est bien TRUE
        }

        [TestMethod]
        public void Test_ParutionDansAbonnement_HorsIntervalle_Avant()
        {
            DateTime dateCommande = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateFin = new DateTime(2024, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            DateTime dateParution = new DateTime(2023, 12, 31, 0, 0, 0, DateTimeKind.Utc);

            bool result = ParutionDansAbonnement(dateCommande, dateFin, dateParution);

            Assert.IsFalse(result); // Vérifier si c'est bien FALSE
        }

        [TestMethod]
        public void Test_ParutionDansAbonnement_HorsIntervalle_Apres()
        {
            DateTime dateCommande = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateFin = new DateTime(2024, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            DateTime dateParution = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            bool result = ParutionDansAbonnement(dateCommande, dateFin, dateParution);

            Assert.IsFalse(result); // Vérifier si c'est bien FALSE
        }

        private static bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFin, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFin;
        }
    }
}