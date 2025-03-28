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
    public class LivreDvdTests
    {
        private const string id = "00027";
        private const string titre = "Test";
        private const string image = "image.png";
        private const string idGenre = "10000";
        private const string genre = "Humour";
        private const string idPublic = "00001";
        private const string lePublic = "Jeunesse";
        private const string idRayon = "JN001";
        private const string rayon = "Jeunesse BD";

        private class LivreDvdFake : LivreDvd
        {
            public LivreDvdFake(string id, string titre, string image, string idGenre, string genre,
                string idPublic, string lePublic, string idRayon, string rayon)
                : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon) { }
        }

        private static readonly LivreDvdFake livreDvd = new LivreDvdFake(
            id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon
        );

        [TestMethod()]
        public void LivreDvdTest()
        {
            Assert.AreEqual(id, livreDvd.Id);
            Assert.AreEqual(titre, livreDvd.Titre);
            Assert.AreEqual(image, livreDvd.Image);
            Assert.AreEqual(idGenre, livreDvd.IdGenre);
            Assert.AreEqual(genre, livreDvd.Genre);
            Assert.AreEqual(idPublic, livreDvd.IdPublic);
            Assert.AreEqual(lePublic, livreDvd.Public);
            Assert.AreEqual(idRayon, livreDvd.IdRayon);
            Assert.AreEqual(rayon, livreDvd.Rayon);
        }
    }
}