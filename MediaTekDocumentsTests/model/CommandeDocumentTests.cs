using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class CommandeDocumentTests
    {
        private const string id = "16";
        private const int nbExemplaire = 5;
        private const string idCommande = "00001";
        private const string idLivreDvd = "00017";
        private const string suiviId = "2";

        private static readonly CommandeDocument commandeDoc = new CommandeDocument(id, nbExemplaire, idCommande, suiviId, idLivreDvd);

        [TestMethod()]
        public void CommandeDocumentTest()
        {
            Assert.AreEqual(id, commandeDoc.Id);
            Assert.AreEqual(nbExemplaire, commandeDoc.NbExemplaire);
            Assert.AreEqual(idCommande, commandeDoc.IdCommande);
            Assert.AreEqual(idLivreDvd, commandeDoc.IdLivreDvd);
            Assert.AreEqual(suiviId, commandeDoc.IdSuivi);
        }
    }
}