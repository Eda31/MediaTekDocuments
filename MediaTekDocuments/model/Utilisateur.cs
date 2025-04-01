namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Utilisateur : contient des propriétés spécifiques aux utilisateurs
    /// </summary>
    public class Utilisateur
    {
        /// <summary>
        /// Identifiant de l'utilisateur
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Prénom de l'utilisateur
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Login de l'utilisateur
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        public string MotDePasse { get; set; }

        /// <summary>
        /// Identifiant du service de l'utilisateur
        /// </summary>
        public string IdService { get; set; }

        /// <summary>
        /// Service de l'utilisateur
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Constructeur de la classe Utilisateur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="login"></param>
        /// <param name="motDePasse"></param>
        /// <param name="idService"></param>
        /// <param name="service"></param>
        public Utilisateur(string id, string nom, string prenom, string login, string motDePasse, string idService, string service)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Login = login;
            MotDePasse = motDePasse;
            IdService = idService;
            Service = service;
        }
    }
}
