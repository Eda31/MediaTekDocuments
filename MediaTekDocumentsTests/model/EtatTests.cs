using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
    /// <summary>
    /// Classe de test de la classe Etat.
    /// </summary>
    [TestClass()]
    public class EtatTests
    {
        private const string id = "1";
        private const string libelle = "Neuf";

        private static readonly Etat etat = new Etat(id, libelle);

        [TestMethod()]
        public void EtatTest()
        {
            Assert.AreEqual(id, etat.Id);
            Assert.AreEqual(libelle, etat.Libelle);
        }
    }
}