namespace MediaTekDocuments.model
{
    public class Utilisateur
    {
        public string Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Login { get; set; }
        public string MotDePasse { get; set; }
        public string IdService { get; set; }
        public string Service { get; set; }

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
