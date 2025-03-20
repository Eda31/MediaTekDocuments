using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
