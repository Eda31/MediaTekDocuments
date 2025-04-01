namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi : contient des propriétés spécifiques aux suivis
    /// </summary>
    public class Suivi
    {
        public string Id { get; }
        public string Etape { get; }

        public Suivi(string id, string etape)
        {
            Id = id;
            Etape = etape;
        }
    }
}
