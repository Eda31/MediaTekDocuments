using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class CategorieTests
    {
        private const string id = "10000";
        private const string libelle = "Humour";
        private static readonly Categorie obj = new Categorie(id, libelle);

        [TestMethod()]
        public void CategorieTest()
        {
            Assert.AreEqual(id, obj.Id, "devrait réussir : Id valorisé");
            Assert.AreEqual(libelle, obj.Libelle, "devrait réussir : Libelle valorisé");
        }
    }
}