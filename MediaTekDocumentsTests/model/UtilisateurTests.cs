using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class UtilisateurTests
    {
        private const string id = "1";
        private const string nom = "Martin";
        private const string prenom = "Sophie";
        private const string login = "smartin";
        private const string motDePasse = "MotDePasse123";
        private const string idService = "1";
        private const string service = "Administratif";
        private static readonly Utilisateur utilisateur = new Utilisateur(id, nom, prenom, login, motDePasse, idService, service);

        [TestMethod()]
        public void UtilisateurTest()
        {
            Assert.AreEqual(id, utilisateur.Id);
            Assert.AreEqual(nom, utilisateur.Nom);
            Assert.AreEqual(prenom, utilisateur.Prenom);
            Assert.AreEqual(login, utilisateur.Login);
            Assert.AreEqual(motDePasse, utilisateur.MotDePasse);
            Assert.AreEqual(idService, utilisateur.IdService);
            Assert.AreEqual(service, utilisateur.Service);
        }
    }
}