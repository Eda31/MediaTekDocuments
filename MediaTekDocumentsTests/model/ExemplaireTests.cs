using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace MediaTekDocuments.model.Tests
{
    /// <summary>
    /// Classe de test de la classe Exemplaire.
    /// </summary>
    [TestClass()]
    public class ExemplaireTests
    {
        private const string id = "EX001";
        private const int numero = 5;
        private const string dateAchat = "2023-06-01";
        private const string photo = "photo.jpg";
        private const string idEtat = "E001";

        private static readonly DateTime parsedDateAchat = DateTime.Parse(dateAchat, CultureInfo.InvariantCulture);

        private static readonly Exemplaire exemplaire = new Exemplaire(
            numero,
            parsedDateAchat,
            photo,
            idEtat,
            id
        );

        [TestMethod()]
        public void ExemplaireTest()
        {
            Assert.AreEqual(id, exemplaire.Id);
            Assert.AreEqual(numero, exemplaire.Numero);
            Assert.AreEqual(parsedDateAchat, exemplaire.DateAchat);
            Assert.AreEqual(photo, exemplaire.Photo);
            Assert.AreEqual(idEtat, exemplaire.IdEtat);
        }
    }
}