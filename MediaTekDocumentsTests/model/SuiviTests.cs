using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class SuiviTests
    {
        private const string id = "1";
        private const string etape = "En cours";

        private static readonly Suivi suivi = new Suivi(id, etape);

        [TestMethod()]
        public void SuiviTest()
        {
            Assert.AreEqual(id, suivi.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(etape, suivi.Etape, "devrait réussir : etape valorisée");
        }
    }
}