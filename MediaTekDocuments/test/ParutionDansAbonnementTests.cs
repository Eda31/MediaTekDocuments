using MediaTekDocuments.controller;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.test
{
    [TestClass]
    public class ParutionDansAbonnementTests
    {
        private Access access;

        [TestInitialize]
        public void Setup()
        {
            access = new Access();
        }

        [TestMethod]
        public void Test_ParutionDansAbonnement_DansIntervalle()
        {
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFin = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2024, 6, 15);

            Assert.IsTrue(access.ParutionDansAbonnement(dateCommande, dateFin, dateParution));
        }

        [TestMethod]
        public void Test_ParutionDansAbonnement_HorsIntervalle_Avant()
        {
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFin = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2023, 12, 31);

            Assert.IsFalse(access.ParutionDansAbonnement(dateCommande, dateFin, dateParution));
        }

        [TestMethod]
        public void Test_ParutionDansAbonnement_HorsIntervalle_Apres()
        {
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFin = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2025, 1, 1);

            Assert.IsFalse(access.ParutionDansAbonnement(dateCommande, dateFin, dateParution));
        }
    }

}
