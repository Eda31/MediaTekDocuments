using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MediaTekDocuments.dal.Tests
{
    [TestClass]
    public class AccessTests
    {
        [TestMethod]
        public void Test_ParutionDansAbonnement_DansIntervalle()
        {
            DateTime dateCommande = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateFin = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateParution = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);
            Assert.IsTrue(Access.ParutionDansAbonnement(dateCommande, dateFin, dateParution));
        }

        [TestMethod]
        public void Test_ParutionDansAbonnement_HorsIntervalle_Avant()
        {
            DateTime dateCommande = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateFin = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateParution = new DateTime(2023, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            Assert.IsFalse(Access.ParutionDansAbonnement(dateCommande, dateFin, dateParution));
        }

        [TestMethod]
        public void Test_ParutionDansAbonnement_HorsIntervalle_Apres()
        {
            DateTime dateCommande = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateFin = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateParution = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.IsFalse(Access.ParutionDansAbonnement(dateCommande, dateFin, dateParution));
        }
    }
}
