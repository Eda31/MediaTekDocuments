using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
    /// <summary>
    /// Classe de test de la classe Public.
    /// </summary>
    [TestClass()]
    public class PublicTests
    {
        private const string id = "50";
        private const string libelle = "Adulte";

        private static readonly Public lePublic = new Public(id, libelle);

        [TestMethod()]
        public void PublicTest()
        {
            Assert.AreEqual(id, lePublic.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, lePublic.Libelle, "devrait réussir : libelle valorisé");
        }
    }
}