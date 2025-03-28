using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
    [TestClass]
    public class DocumentTests
    {
        private const string id = "DOC001";
        private const string titre = "Titre Test";
        private const string image = "image.jpg";
        private const string idGenre = "G01";
        private const string genre = "Science";
        private const string idPublic = "P01";
        private const string lePublic = "Adulte";
        private const string idRayon = "R01";
        private const string rayon = "Roman";

        private static readonly Document document = new Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);

        [TestMethod]
        public void DocumentTest()
        {
            Assert.AreEqual(id, document.Id);
            Assert.AreEqual(titre, document.Titre);
            Assert.AreEqual(image, document.Image);
            Assert.AreEqual(idGenre, document.IdGenre);
            Assert.AreEqual(genre, document.Genre);
            Assert.AreEqual(idPublic, document.IdPublic);
            Assert.AreEqual(lePublic, document.Public);
            Assert.AreEqual(idRayon, document.IdRayon);
            Assert.AreEqual(rayon, document.Rayon);
        }
    }
}