using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
    /// <summary>
    /// Classe de test de la classe Rayon.
    /// </summary>
    [TestClass()]
    public class RayonTests
    {
        private const string id = "R01";
        private const string libelle = "Science";

        private static readonly Rayon rayon = new Rayon(id, libelle);

        [TestMethod()]
        public void RayonTest()
        {
            Assert.AreEqual(id, rayon.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, rayon.Libelle, "devrait réussir : libelle valorisé");
        }
    }
}