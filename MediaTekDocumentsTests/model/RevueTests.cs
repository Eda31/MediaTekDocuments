using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class RevueTests
    {
        private const string id = "R001";
        private const string titre = "Science & Vie";
        private const string image = "revue.jpg";
        private const string idGenre = "G01";
        private const string genre = "Science";
        private const string idPublic = "P01";
        private const string lePublic = "Adulte";
        private const string idRayon = "R01";
        private const string rayon = "Magazines";
        private const string periodicite = "Mensuel";
        private const int delaiMiseADispo = 3;

        private static readonly Revue revue = new Revue(
            id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo
        );

        [TestMethod()]
        public void RevueTest()
        {
            Assert.AreEqual(id, revue.Id);
            Assert.AreEqual(titre, revue.Titre);
            Assert.AreEqual(image, revue.Image);
            Assert.AreEqual(idGenre, revue.IdGenre);
            Assert.AreEqual(genre, revue.Genre);
            Assert.AreEqual(idPublic, revue.IdPublic);
            Assert.AreEqual(lePublic, revue.Public);
            Assert.AreEqual(idRayon, revue.IdRayon);
            Assert.AreEqual(rayon, revue.Rayon);
            Assert.AreEqual(periodicite, revue.Periodicite);
            Assert.AreEqual(delaiMiseADispo, revue.DelaiMiseADispo);
        }
    }
}