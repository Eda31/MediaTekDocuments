using MediaTekDocuments.manager;
using MediaTekDocuments.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        /// </summary>
        private const string PUT = "PUT";
        /// <summary>
        /// méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";

        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if(instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = ConvertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message, String parametres)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String ConvertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }
        /**
        /// <summary>
        /// Suppresion d'un livre dans la base de données
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteLivre(string id)
        {
            try
            {
                string jsonId = ConvertToJson("id", id);
                JObject jsonResponse = api.RecupDistant(DELETE, "livre", "champs=" + jsonId);
                if (jsonResponse != null)
                {
                    if ((int)jsonResponse["code"] == 200)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Erreur lors de la suppression du livre : {(string)jsonResponse["message"]}", "Erreur");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Erreur de connexion à l'API");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        */
        /**
        /// <summary>
        /// Ajout d'un livre dans la base de données
        /// </summary>
        /// <param name="livre"></param>
        /// <returns></returns>
        public bool AjoutLivre(Livre livre)
        {
            try
            {
                string jsonLivre = JsonConvert.SerializeObject(livre, new CustomDateTimeConverter());
                JObject jsonResponse = api.RecupDistant(POST, "livre", "champs=" + jsonLivre);
                // Vérification de la réponse de l'API
                if (jsonResponse != null && (int)jsonResponse["code"] == 200)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Erreur lors de l'ajout du livre : " + (string)jsonResponse["message"]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
        }
        */
        /**
        public bool ModifierLivre(Livre livre)
        {
            try
            {
                string jsonLivre = JsonConvert.SerializeObject(livre, new CustomDateTimeConverter());
                JObject jsonResponse = api.RecupDistant("PUT", "livre", "champs=" + jsonLivre);
                // Vérification de la réponse de l'API
                if (jsonResponse != null && (int)jsonResponse["code"] == 200)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Erreur lors de la modification du livre : " + (string)jsonResponse["message"]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
        }
        */

        /// <summary>
        /// Retourne toutes les étapes de suivi à partir de la BDD
        /// </summary>
        /// <returns></returns>
        public List<Suivi> GetAllSuivi()
        {
            List<Suivi> lesSuivi = TraitementRecup<Suivi>(GET, "suivi", null);
            return lesSuivi;
        }

        /// <summary>
        /// Retourne les commandes des livres
        /// </summary>
        /// <param name="idDocument">id du livre concernée</param>
        /// <returns>Liste d'objets Commande</returns>
        public List<Commande> GetCommandesLivre(string idDocument)
        {
            String jsonIdDocument = ConvertToJson("id", idDocument);
            List<Commande> lesCommandes = TraitementRecup<Commande>(GET, "commande/" + jsonIdDocument, null);
            return lesCommandes;
        }

        /// <summary>
        /// ecriture d'une commande en base de données
        /// </summary>
        /// <param name="commande">commande à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerCommande(Commande commande)
        {
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            try
            {
                JObject jsonResponse = api.RecupDistant(POST, "commande", "champs=" + jsonCommande);
                if (jsonResponse != null && (int)jsonResponse["code"] == 200)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Erreur lors de l'ajout d'une commande : " + (string)jsonResponse["message"]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification d'une commande en base de données
        /// </summary>
        /// <param name="commande">commande à modifier</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool ModifierCommande(Commande commande)
        {
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            try
            {
                JObject jsonResponse = api.RecupDistant(PUT, "commande", "champs=" + jsonCommande);
                if (jsonResponse != null && (int)jsonResponse["code"] == 200)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Erreur lors de la modification de la commande : " + (string)jsonResponse["message"]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Supprime une commande dans la BDD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SupprimerCommande(string id)
        {
            // S'assurer que l'ID est bien structuré sous forme de JSON
            string jsonId = ConvertToJson("Id", id);
            try
            {
                // Envoyer le JSON dans le CORPS de la requête (comme pour ModifierCommande)
                JObject jsonResponse = api.RecupDistant(DELETE, "commande/"+ jsonId, null);
                if (jsonResponse == null)
                {
                    Console.WriteLine("Réponse API null, suppression échouée.");
                    return false;
                }
                if ((int)jsonResponse["code"] == 200)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Erreur lors de la suppression : " + (string)jsonResponse["message"]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception attrapée : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Retourne les commandes de dvd
        /// </summary>
        /// <param name="idDocument">id du dvd concernée</param>
        /// <returns>Liste d'objets Commande</returns>
        public List<Commande> GetCommandesDvd(string idDocument)
        {
            String jsonIdDocument = ConvertToJson("id", idDocument);
            List<Commande> lesCommandesDvd = TraitementRecup<Commande>(GET, "commande/" + jsonIdDocument, null);
            return lesCommandesDvd;
        }

        /// <summary>
        /// Retourne les commandes d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Commande</returns>
        public List<Abonnement> GetCommandesRevue(string idDocument)
        {
            String jsonIdRevue = ConvertToJson("id", idDocument);
            List<Abonnement> lesCommandesRevue = TraitementRecup<Abonnement>(GET, "abonnement/" + jsonIdRevue, null);
            return lesCommandesRevue;
        }

        /// <summary>
        /// ecriture d'une commande en base de données pour une revue
        /// </summary>
        /// <param name="abonnement">commande à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            String jsonCommande = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());
            try
            {
                JObject jsonResponse = api.RecupDistant(POST, "abonnement", "champs=" + jsonCommande);
                if (jsonResponse != null && (int)jsonResponse["code"] == 200)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Erreur lors de l'ajout d'une commande : " + (string)jsonResponse["message"]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification d'un abonnement en base de données
        /// </summary>
        /// <param name="abonnement">abonnement à modifier</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool ModifierAbonnement(Abonnement abonnement)
        {
            String jsonAbonnement = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());
            try
            {
                JObject jsonResponse = api.RecupDistant(PUT, "abonnement", "champs=" + jsonAbonnement);
                if (jsonResponse != null && (int)jsonResponse["code"] == 200)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Erreur lors de la modification de l'abonnement : " + (string)jsonResponse["message"]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Supprime une abonnement dans la BDD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SupprimerAbonnement(string id)
        {
            // S'assurer que l'ID est bien structuré sous forme de JSON
            string jsonId = ConvertToJson("Id", id);
            try
            {
                // Envoyer le JSON dans le CORPS de la requête (comme pour ModifierCommande)
                JObject jsonResponse = api.RecupDistant(DELETE, "abonnement/" + jsonId, null);
                if (jsonResponse == null)
                {
                    Console.WriteLine("Réponse API null, suppression échouée.");
                    return false;
                }
                if ((int)jsonResponse["code"] == 200)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Erreur lors de la suppression : " + (string)jsonResponse["message"]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception attrapée : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Retourne les commandes des revues
        /// </summary>
        /// <param name="dateCommande"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="dateParution"></param>
        /// <returns></returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }

        /// <summary>
        /// Retourne les abonnements des revues qui expirent dans 30 jours
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> GetAbonnementsExpirantDans30Jours()
        {
            List<Abonnement> abonnements = TraitementRecup<Abonnement>(GET, "abonnements", null);
            // On récupère la liste des revues pour faire la correspondance
            List<Revue> revues = GetAllRevues();
            foreach (var abonnement in abonnements)
            {
                // Vérification avant comparaison
                var revue = revues.FirstOrDefault(r => r.Id.ToString().Trim() == abonnement.IdRevue);
                if (revue != null)
                {
                    abonnement.TitreRevue = revue.Titre;
                }
                else
                {
                    Console.WriteLine($"⚠️ Aucune revue trouvée pour l'IdRevue {abonnement.IdRevue}");
                }
            }
            return abonnements;
        }

    }
}
