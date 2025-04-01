namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi : contient des propriétés spécifiques aux suivis
    /// </summary>
    public class Suivi
    {
        /// <summary>
        /// Identifiant du suivi
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Etape du suivi
        /// </summary>
        public string Etape { get; }

        /// <summary>
        /// Constructeur de la classe Suivi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="etape"></param>
        public Suivi(string id, string etape)
        {
            Id = id;
            Etape = etape;
        }
    }
}
