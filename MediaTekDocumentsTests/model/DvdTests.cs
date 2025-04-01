using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
    /// <summary>
    /// Classe de test de la classe Dvd.
    /// </summary>
    [TestClass()]
    public class DvdTests
    {
        private const string id = "00001";
        private const string titre = "Inception";
        private const string image = "cover.jpg";
        private const string idGenre = "01";
        private const string genre = "Science-Fiction";
        private const string idPublic = "02";
        private const string publicLabel = "Adulte";
        private const string idRayon = "03";
        private const string rayon = "Rayon A";
        private const string realisateur = "Nolan";
        private const int duree = 180;
        private const string synopsis = "Thriller";

        private static readonly Dvd dvd = new Dvd(
            id, titre, image, duree, realisateur, synopsis,
            idGenre, genre, idPublic, publicLabel, idRayon, rayon
        );

        [TestMethod()]
        public void DvdTest()
        {
            Assert.AreEqual(id, dvd.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, dvd.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, dvd.Image, "devrait réussir : image valorisée");
            Assert.AreEqual(idGenre, dvd.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, dvd.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, dvd.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(publicLabel, dvd.Public, "devrait réussir : public valorisé");
            Assert.AreEqual(idRayon, dvd.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, dvd.Rayon, "devrait réussir : rayon valorisé");
            Assert.AreEqual(realisateur, dvd.Realisateur, "devrait réussir : realisateur valorisé");
            Assert.AreEqual(duree, dvd.Duree, "devrait réussir : duree valorisée");
            Assert.AreEqual(synopsis, dvd.Synopsis, "devrait réussir : synopsis valorisé");
        }
    }
}