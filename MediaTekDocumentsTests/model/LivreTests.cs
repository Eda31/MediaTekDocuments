using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
    /// <summary>
    /// Classe de test de la classe Livre.
    /// </summary>
    [TestClass()]
    public class LivreTests
    {
        private const string id = "L123";
        private const string titre = "Le Petit Prince";
        private const string image = "cover.jpg";
        private const string idGenre = "G01";
        private const string genre = "Conte";
        private const string idPublic = "P01";
        private const string lePublic = "Enfant";
        private const string idRayon = "R01";
        private const string rayon = "Littérature";
        private const string isbn = "9782070612758";
        private const string auteur = "Antoine de Saint-Exupéry";
        private const string collection = "Classiques";

        private static readonly Livre livre = new Livre(
            id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, lePublic, idRayon, rayon
        );

        [TestMethod()]
        public void LivreTest()
        {
            Assert.AreEqual(id, livre.Id);
            Assert.AreEqual(titre, livre.Titre);
            Assert.AreEqual(image, livre.Image);
            Assert.AreEqual(idGenre, livre.IdGenre);
            Assert.AreEqual(genre, livre.Genre);
            Assert.AreEqual(idPublic, livre.IdPublic);
            Assert.AreEqual(lePublic, livre.Public);
            Assert.AreEqual(idRayon, livre.IdRayon);
            Assert.AreEqual(rayon, livre.Rayon);
            Assert.AreEqual(isbn, livre.Isbn);
            Assert.AreEqual(auteur, livre.Auteur);
            Assert.AreEqual(collection, livre.Collection);
        }
    }
}