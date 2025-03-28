namespace MediaTekDocuments.model
{
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
