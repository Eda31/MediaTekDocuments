using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Exemplaire (exemplaire d'une revue)
    /// </summary>
    public class Exemplaire
    {
        /// <summary>
        /// Numéro de l'exemplaire
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Photo de l'exemplaire
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Date d'achat de l'exemplaire
        /// </summary>
        public DateTime DateAchat { get; set; }

        /// <summary>
        /// Identifiant de l'état de l'exemplaire
        /// </summary>
        public string IdEtat { get; set; }

        /// <summary>
        /// Identifiant du document associé à l'exemplaire
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Constructeur par exemplaire
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="dateAchat"></param>
        /// <param name="photo"></param>
        /// <param name="idEtat"></param>
        /// <param name="idDocument"></param>
        public Exemplaire(int numero, DateTime dateAchat, string photo, string idEtat, string idDocument)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.Id = idDocument;
        }

    }
}
