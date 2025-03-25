using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }
        /**
        /// <summary>
        /// Supprimer un livre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteLivre(string id)
        {
            return access.DeleteLivre(id);
        }

        /// <summary>
        /// Ajouter un livre
        /// </summary>
        /// <param name="livre"></param>
        /// <returns></returns>
        public bool AjoutLivre(Livre livre)
        {
            return access.AjoutLivre(livre);
        }

        /// <summary>
        /// Modifier un livre
        /// </summary>
        /// <param name="livre"></param>
        /// <returns></returns>
        public bool ModifierLivre(Livre livre)
        {
            return access.ModifierLivre(livre);
        }
        */
        /// <summary>
        /// getter sur les suivis
        /// </summary>
        /// <returns></returns>
        public List<Suivi> GetAllSuivi()
        {
            return access.GetAllSuivi();
        }

        /// <summary>
        /// récupère les commandes d'un livre
        /// </summary>
        /// <param name="idDocuement">id du livre concernée</param>
        /// <returns>Liste d'objets commande</returns>
        public List<Commande> GetCommandesLivre(string idDocuement)
        {
            return access.GetCommandesLivre(idDocuement);
        }

        /// <summary>
        /// Crée une commande d'un livre dans la bdd
        /// </summary>
        /// <param name="commande">L'objet Commande concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommande(Commande commande)
        {
            return access.CreerCommande(commande);
        }

        /// <summary>
        /// Modifier une commande d'un livre
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public bool ModifierCommande(Commande commande)
        {
            return access.ModifierCommande(commande);
        }

        /// <summary>
        /// Supprimer une commande d'un livre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SupprimerCommande(string id)
        {
            return access.SupprimerCommande(id);
        }

        /// <summary>
        /// récupère les commandes d'un dvd
        /// </summary>
        /// <param name="idDocuement">id du dvd concernée</param>
        /// <returns>Liste d'objets dvd</returns>
        public List<Commande> GetCommandesDvd(string idDocuement)
        {
            return access.GetCommandesDvd(idDocuement);
        }

        /// <summary>
        /// récupère les commandes d'un dvd
        /// </summary>
        /// <param name="idDocuement">id du dvd concernée</param>
        /// <returns>Liste d'objets dvd</returns>
        public List<Abonnement> GetCommandesRevue(string idDocument)
        {
            return access.GetCommandesRevue(idDocument);
        }

        /// <summary>
        /// Crée une commande d'une revue
        /// </summary>
        /// <param name="commande">L'objet Commande concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            return access.CreerAbonnement(abonnement);
        }

        /// <summary>
        /// Modifier une commande d'une revue
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns></returns>
        public bool ModifierAbonnement(Abonnement abonnement)
        {
            return access.ModifierAbonnement(abonnement);
        }

        /// <summary>
        /// Supprimer une commande d'une revue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SupprimerAbonnement(string id)
        {
            return access.SupprimerAbonnement(id);
        }

        /// <summary>
        /// récupère les commandes d'une revue
        /// </summary>
        /// <param name="dateCommande"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="dateParution"></param>
        /// <returns></returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return access.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);
        }

        /// <summary>
        /// récupère les abonnement des revues qui expire dans 30 jours 
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> GetAbonnementsExpirantDans30Jours()
        {
            return access.GetAbonnementsExpirantDans30Jours();
        }

    }
}
