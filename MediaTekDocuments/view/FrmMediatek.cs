using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        private readonly BindingSource bdgGenresAjout = new BindingSource();
        private readonly BindingSource bdgPublicsAjout = new BindingSource();
        private readonly BindingSource bdgRayonsAjout = new BindingSource();

        private readonly Utilisateur utilisateur;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek(Utilisateur utilisateur)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            this.utilisateur = utilisateur;
            this.GererAccesParService();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public static void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);

            RemplirComboCategorie(controller.GetAllGenres(), bdgGenresAjout, cbxLivresGenresAjout);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublicsAjout, cbxLivresPublicsAjout);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayonsAjout, cbxLivresRayonsAjout);

            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// Suppression d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void BtnLivresSupprimer_Click(object sender, EventArgs e)
#pragma warning disable CS1587 // Le commentaire XML n'est pas placé dans un élément valide du langage
        {
            // Cette méthode est intentionnellement laissée vide.
            /**
            if (dgvLivresListe.SelectedRows.Count > 0)
            {
                Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                if (controller.DeleteLivre(livre.Id))
                {
                    lesLivres.Remove(livre);
                    RemplirLivresListeComplete();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression du livre");
                }
            }
            */
        }
#pragma warning restore CS1587 // Le commentaire XML n'est pas placé dans un élément valide du langage

        /// <summary>
        /// Ajout d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAjouter_Click(object sender, EventArgs e)
#pragma warning disable CS1587 // Le commentaire XML n'est pas placé dans un élément valide du langage
        {
            // Cette méthode est intentionnellement laissée vide.
            /**
            string id = txbLivresNumeroAjout.Text.Trim();
            string titre = txbLivresTitreAjout.Text.Trim();
            string image = txbLivresImageAjout.Text.Trim();
            string isbn = txbLivresIsbnAjout.Text.Trim();
            string auteur = txbLivresAuteurAjout.Text.Trim();
            string collection = txbLivresCollectionAjout.Text.Trim();

            Categorie selectedGenre = (Categorie)cbxLivresGenresAjout.SelectedItem;
            Categorie selectedPublic = (Categorie)cbxLivresPublicsAjout.SelectedItem;
            Categorie selectedRayon = (Categorie)cbxLivresRayonsAjout.SelectedItem;

            if (selectedGenre == null || selectedPublic == null || selectedRayon == null)
            {
                MessageBox.Show("Veuillez sélectionner un genre, un public et un rayon.");
                return;
            }

            string genre = selectedGenre.Libelle;
            string lePublic = selectedPublic.Libelle;
            string rayon = selectedRayon.Libelle;

            // Valider les informations
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(titre))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
                return;
            }

            // Créer un nouvel objet Livre
            Livre nouveauLivre = new Livre(id, titre, image, isbn, auteur, collection,
                                           selectedGenre.Id, genre,
                                           selectedPublic.Id, lePublic,
                                           selectedRayon.Id, rayon);

            // Appeler la méthode du contrôleur pour ajouter le livre
            bool resultat = controller.AjoutLivre(nouveauLivre);

            // Afficher un message de confirmation ou d'erreur
            if (resultat)
            {
                MessageBox.Show("Livre ajouté avec succès.");
                // Mettre à jour la liste des livres
                lesLivres = controller.GetAllLivres();
                RemplirLivresListeComplete();
            }
            else
            {
                MessageBox.Show("Erreur lors de l'ajout du livre.");
            }
            */
        }
#pragma warning restore CS1587 // Le commentaire XML n'est pas placé dans un élément valide du langage
        private void BtnLivresMofifier_Click(object sender, EventArgs e)
#pragma warning disable CS1587 // Le commentaire XML n'est pas placé dans un élément valide du langage
        {
            // Cette méthode est intentionnellement laissée vide.
            /**
            // Récupérer les informations du formulaire
            string id = txbLivresNumero.Text;
            string titre = txbLivresTitre.Text;
            string image = txbLivresImage.Text;
            string isbn = txbLivresIsbn.Text;
            string auteur = txbLivresAuteur.Text;
            string collection = txbLivresCollection.Text;
            string genre = cbxLivresGenresAjout.SelectedItem?.ToString();
            string lePublic = cbxLivresPublicAjout.SelectedItem?.ToString();
            string rayon = cbxLivresRayonAjout.SelectedItem?.ToString();

            // Valider les informations
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(titre) || string.IsNullOrWhiteSpace(isbn) ||
                string.IsNullOrWhiteSpace(auteur) || string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(genre) ||
                string.IsNullOrWhiteSpace(lePublic) || string.IsNullOrWhiteSpace(rayon))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
                return;
            }

            // Créer un objet Livre modifié
            Livre livreModifie = new Livre(id, titre, image, isbn, auteur, collection, genre, lePublic, rayon);

            // Appeler la méthode du contrôleur pour modifier le livre
            bool resultat = controller.ModifierLivre(livreModifie);

            // Afficher un message de confirmation ou d'erreur
            if (resultat)
            {
                MessageBox.Show("Livre modifié avec succès.");
                // Mettre à jour la liste des livres
                lesLivres = controller.GetAllLivres();
                RemplirLivresListeComplete();
            }
            else
            {
                MessageBox.Show("Erreur lors de la modification du livre.");
            }
            */
        }
#pragma warning restore CS1587 // Le commentaire XML n'est pas placé dans un élément valide du langage


        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        #endregion

        #region Onglet Commandes Livre
        private readonly BindingSource bdgCommandesListe = new BindingSource();
        private readonly BindingSource bdgSuivi = new BindingSource();
        private List<Commande> lesCommandes = new List<Commande>();

        /// <summary>
        /// Ouverture de l'onglet Commande de Livres : 
        /// appel des méthodes pour le combo suivi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandeLivre_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            List<Suivi> lesSuivis = controller.GetAllSuivi();
            RemplirComboSuivi(lesSuivis, bdgSuivi, CbxNouvelleEtape);
        }

        /// <summary>
        /// Rechercher une ou des commandes pour un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRechercheCom_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRechercheCom.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRechercheCom.Text));
                if (livre != null)
                {
                    AfficheReceptionCommandesLivre(livre);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné et les commandes liées à ce livre
        /// </summary>
        private void AfficheReceptionCommandesLivre(Livre livre)
        {
            // informations sur le livre
            txbLivreAuteurCom.Text = livre.Auteur;
            txbLivreCollectionCom.Text = livre.Collection;
            txbLivreCheminImgCom.Text = livre.Image;
            txbLivreIsbnCom.Text = livre.Isbn;
            txbLivreGenreCom.Text = livre.Genre;
            txbLivrePublicCom.Text = livre.Public;
            txbLivreRayonCom.Text = livre.Rayon;
            txbLivreTitreCom.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivreImageCom.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivreImageCom.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionCommandesLivre();
        }

        /// <summary>
        /// Récupère et affiche les commandes d'un livre
        /// </summary>
        private void AfficheReceptionCommandesLivre()
        {
            string idDocument = txbLivresNumRechercheCom.Text;
            lesCommandes = controller.GetCommandesLivre(idDocument);
            RemplirReceptionCommandesLivre(lesCommandes);
            AccesReceptionCommandeGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'une commande et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionCommandeGroupBox(bool acces)
        {
            grpAjoutCommandes.Enabled = acces;
            txbNumeroCommandeLivre.Text = "";
            dtpCommandeDate.Value = DateTime.Now;
            txbMontantCommande.Text = "";
            txbNbExemplairesCommande.Text = "";
            txbNumLivreCom.Text = txbLivresNumRechercheCom.Text;
        }

        /// <summary>
        /// Ajout d'une commande d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjoutCommande_Click(object sender, EventArgs e)
        {
            if (!txbNumeroCommandeLivre.Text.Equals(""))
            {
                try
                {
                    // Correction des types des champs
                    string id = txbNumeroCommandeLivre.Text;
                    string idLivreDvd = txbNumLivreCom.Text;
                    DateTime dateCommande = dtpCommandeDate.Value;
                    int montant = int.Parse(txbMontantCommande.Text);
                    int nbExemplaires = int.Parse(txbNbExemplairesCommande.Text);
                    int suiviId = 1; // Étape "en cours"
                    string etapeSuivi = "en cours";

                    Commande commande = new Commande(id, idLivreDvd, dateCommande, montant, nbExemplaires, suiviId, etapeSuivi);

                    if (controller.CreerCommande(commande))
                    {
                        MessageBox.Show("Commande ajoutée avec succès.");
                        AfficheReceptionCommandesLivre();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout de la commande.");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Les champs 'Montant' et 'Nombre d'exemplaires' doivent être numériques.", "Erreur de format");
                    txbNumeroCommandeLivre.Text = "";
                    txbNumeroCommandeLivre.Focus();
                }
            }
            else
            {
                MessageBox.Show("Le numéro de commande est obligatoire.", "Information");
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvReceptionCommandesListe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string titreColonne = DgvReceptionCommandesListe.Columns[e.ColumnIndex].HeaderText;
            List<Commande> sortedList = new List<Commande>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandes.OrderBy(o => o.Id).ToList();
                    break;
                case "IdLivreDvd":
                    sortedList = lesCommandes.OrderBy(o => o.IdLivreDvd).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandes.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandes.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandes.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "IdSuivi":
                    sortedList = lesCommandes.OrderBy(o => o.SuiviId).ToList();
                    break;
                case "EtapeSuivi":
                    sortedList = lesCommandes.OrderBy(o => o.EtapeSuivi).ToList();
                    break;
            }
            RemplirReceptionCommandesLivre(sortedList);
        }

        /// <summary>
        /// Remplit le DataGridView des commandes avec la liste reçue en paramètre
        /// </summary>
        /// <param name="commandes">liste de commandes</param>
        private void RemplirReceptionCommandesLivre(List<Commande> commandes)
        {
            if (commandes != null)
            {
                bdgCommandesListe.DataSource = commandes;
                DgvReceptionCommandesListe.DataSource = bdgCommandesListe;
                DgvReceptionCommandesListe.Columns["Id"].Visible = true;
                DgvReceptionCommandesListe.Columns["DateCommande"].Visible = true;
                DgvReceptionCommandesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                DgvReceptionCommandesListe.Columns["IdLivreDvd"].DisplayIndex = 1;
                DgvReceptionCommandesListe.Columns["DateCommande"].DisplayIndex = 2;
                DgvReceptionCommandesListe.Columns["Montant"].DisplayIndex = 3;
                DgvReceptionCommandesListe.Columns["NbExemplaire"].DisplayIndex = 4;
                DgvReceptionCommandesListe.Columns["SuiviId"].DisplayIndex = 5;
                DgvReceptionCommandesListe.Columns["EtapeSuivi"].DisplayIndex = 6;
            }
            else
            {
                bdgCommandesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Modifier l'etape de la commande du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifCommande_Click(object sender, EventArgs e)
        {
            if (DgvReceptionCommandesListe.SelectedRows.Count > 0)
            {
                // Récupérer la commande sélectionnée
                Commande commande = (Commande)bdgCommandesListe.List[bdgCommandesListe.Position];
                string currentEtape = commande.EtapeSuivi;

                // Récupérer la nouvelle étape sélectionnée dans la comboBox
                if (CbxNouvelleEtape.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner une nouvelle étape de suivi.", "Information");
                    return;
                }
                Suivi newSuivi = (Suivi)CbxNouvelleEtape.SelectedItem;
                string newEtape = newSuivi.Etape;

                // Vérifier les règles de transition d'étape
                if ((currentEtape == "livrée" || currentEtape == "réglée") && (newEtape == "en cours" || newEtape == "relancée"))
                {
                    MessageBox.Show("Une commande livrée ou réglée ne peut pas revenir à une étape précédente (en cours ou relancée).", "Erreur de transition");
                    return;
                }
                if (newEtape == "réglée" && currentEtape != "livrée")
                {
                    MessageBox.Show("Une commande ne peut pas être réglée si elle n'est pas livrée.", "Erreur de transition");
                    return;
                }

                // Mise à jour de la commande avec la nouvelle étape
                commande.SuiviId = int.Parse(newSuivi.Id);
                commande.EtapeSuivi = newEtape;

                // Envoyer la mise à jour à la base de données via l'API
                if (controller.ModifierCommande(commande))
                {
                    MessageBox.Show("Commande modifiée avec succès.");
                    AfficheReceptionCommandesLivre(); // Mettre à jour l'affichage des commandes
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification de la commande.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande à modifier.", "Information");
            }
        }

        /// <summary>
        /// combobox pour le suivi d'une commande d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxNouvelleEtape_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxNouvelleEtape.SelectedIndex >= 0)
            {
                this.Controls.Add(this.CbxNouvelleEtape);
            }
        }

        /// <summary>
        /// Rempli le combo suivi
        /// </summary>
        /// <param name="lesSuivi">liste des objets de type Suivi</param>
        /// <param name="bdgSuivi">bindingsource contenant les informations</param>
        /// <param name="cbxNouvelleEtape">combobox à remplir</param>
        private static void RemplirComboSuivi(List<Suivi> lesSuivi, BindingSource bdgSuivi, ComboBox cbxNouvelleEtape)
        {
            if (lesSuivi != null && lesSuivi.Count > 0)
            {
                bdgSuivi.DataSource = lesSuivi;
                cbxNouvelleEtape.DataSource = bdgSuivi;
                cbxNouvelleEtape.DisplayMember = "Etape";
                cbxNouvelleEtape.ValueMember = "Id";
                cbxNouvelleEtape.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Aucune étape de suivi disponible.", "Information");
            }
        }

        /// <summary>
        /// Suppression d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprCommande_Click(object sender, EventArgs e)
        {
            if (DgvReceptionCommandesListe.SelectedRows.Count > 0)
            {
                Commande commande = (Commande)bdgCommandesListe.List[bdgCommandesListe.Position];
                if (commande.EtapeSuivi == "en cours" || commande.EtapeSuivi == "relancée")
                {
                    if (controller.SupprimerCommande(commande.Id))
                    {
                        MessageBox.Show("Commande supprimée avec succès.");
                        // Mettre à jour l'affichage des commandes après suppression
                        AfficheReceptionCommandesLivre();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de la commande.", "Erreur");
                    }
                }
                else
                {
                    MessageBox.Show("La commande doit être relancée ou en cours pour être supprimée.", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande à supprimer.", "Information");
            }
        }

        #endregion

        #region Onglet Commandes Dvd
        private readonly BindingSource bdgCommandesListeDvd = new BindingSource();
        private readonly BindingSource bdgSuiviDvd = new BindingSource();
        private List<Commande> lesCommandesDvd = new List<Commande>();

        /// <summary>
        /// Ouverture de l'onglet Commande de DVD :
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandeDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboSuiviDvd(controller.GetAllSuivi(), bdgSuiviDvd, cbxNouvelleEtapeDVD);
        }

        /// <summary>
        /// Rechercher une ou des commandes pour un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDVDNumRechercheCom_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRechercheCom.Text.Equals(""))
            {
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRechercheCom.Text));
                if (dvd != null)
                {
                    AfficheReceptionCommandesDvd(dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné et les commandes liées à ce dvd
        /// </summary>
        private void AfficheReceptionCommandesDvd(Dvd dvd)
        {
            AfficheDvdInfosCom(dvd);
            AfficheReceptionCommandesDvd();
        }

        /// <summary>
        /// Récupère et affiche les commandes d'un dvd
        /// </summary>
        private void AfficheReceptionCommandesDvd()
        {
            string idDocument = txbDvdNumRechercheCom.Text;
            lesCommandesDvd = controller.GetCommandesDvd(idDocument);
            RemplirReceptionCommandesDvd(lesCommandesDvd);
            AccesReceptionCommandeGroupBoxDvd(true);
        }

        /// <summary>
        /// Affichage des informations du DVD sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfosCom(Dvd dvd)
        {
            txbDvdRealisateurCom.Text = dvd.Realisateur;
            txbDvdSynopsisCom.Text = dvd.Synopsis;
            txbDvdImageCom.Text = dvd.Image;
            txbDvdDureeCom.Text = dvd.Duree.ToString();
            txbDvdGenreCom.Text = dvd.Genre;
            txbDvdPublicCom.Text = dvd.Public;
            txbDvdRayonCom.Text = dvd.Rayon;
            txbDvdTitreCom.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImageCom.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImageCom.Image = null;
            }
        }

        /// <summary>
        /// Remplit le DataGridView des commandes avec la liste reçue en paramètre
        /// </summary>
        /// <param name="commandes">liste de commandes</param>
        private void RemplirReceptionCommandesDvd(List<Commande> commandes)
        {
            if (commandes != null)
            {
                bdgCommandesListeDvd.DataSource = commandes;
                dgvReceptionCommandesListeDVD.DataSource = bdgCommandesListeDvd;
                dgvReceptionCommandesListeDVD.Columns["Id"].Visible = true;
                dgvReceptionCommandesListeDVD.Columns["DateCommande"].Visible = true;
                dgvReceptionCommandesListeDVD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionCommandesListeDVD.Columns["IdLivreDvd"].DisplayIndex = 1;
                dgvReceptionCommandesListeDVD.Columns["DateCommande"].DisplayIndex = 2;
                dgvReceptionCommandesListeDVD.Columns["Montant"].DisplayIndex = 3;
                dgvReceptionCommandesListeDVD.Columns["NbExemplaire"].DisplayIndex = 4;
                dgvReceptionCommandesListeDVD.Columns["SuiviId"].DisplayIndex = 5;
                dgvReceptionCommandesListeDVD.Columns["EtapeSuivi"].DisplayIndex = 6;
            }
            else
            {
                bdgCommandesListeDvd.DataSource = null;
            }
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'une commande et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionCommandeGroupBoxDvd(bool acces)
        {
            GrpAjoutCommandesDVD.Enabled = acces;
            txbNumeroCommandeDVD.Text = "";
            dtpCommandeDateDVD.Value = DateTime.Now;
            txbMontantCommandeDVD.Text = "";
            txbNbExemplairesCommandeDVD.Text = "";
            txbNumDVDCom.Text = txbDvdNumRechercheCom.Text;
        }

        /// <summary>
        /// Remplit le DataGridView des commandes avec la liste reçue en paramètre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvReceptionCommandesListeDVD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string titreColonne = dgvReceptionCommandesListeDVD.Columns[e.ColumnIndex].HeaderText;
            List<Commande> sortedList = new List<Commande>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "IdLivreDvd":
                    sortedList = lesCommandesDvd.OrderBy(o => o.IdLivreDvd).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandesDvd.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDvd.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandesDvd.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "SuiviId":
                    sortedList = lesCommandesDvd.OrderBy(o => o.SuiviId).ToList();
                    break;
                case "EtapeSuivi":
                    sortedList = lesCommandesDvd.OrderBy(o => o.EtapeSuivi).ToList();
                    break;
            }
            RemplirReceptionCommandesDvd(sortedList);
        }

        /// <summary>
        /// Bouton d'ajout d'une commande de Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjoutCommandeDVD_Click(object sender, EventArgs e)
        {
            if (!txbNumeroCommandeDVD.Text.Equals(""))
            {
                try
                {
                    // Correction des types des champs
                    string id = txbNumeroCommandeDVD.Text;
                    string idLivreDvd = txbNumDVDCom.Text;
                    DateTime dateCommande = dtpCommandeDateDVD.Value;
                    int montant = int.Parse(txbMontantCommandeDVD.Text);
                    int nbExemplaires = int.Parse(txbNbExemplairesCommandeDVD.Text);
                    int suiviId = 1; // Étape "en cours"
                    string etapeSuivi = "en cours";

                    Commande commande = new Commande(id, idLivreDvd, dateCommande, montant, nbExemplaires, suiviId, etapeSuivi);

                    if (controller.CreerCommande(commande))
                    {
                        MessageBox.Show("Commande ajoutée avec succès.");
                        AfficheReceptionCommandesDvd(); // Mettre à jour la liste des commandes
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout de la commande.");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Les champs 'Montant' et 'Nombre d'exemplaires' doivent être numériques.", "Erreur de format");
                    txbNumeroCommandeDVD.Text = "";
                    txbNumeroCommandeDVD.Focus();
                }
            }
            else
            {
                MessageBox.Show("Le numéro de commande est obligatoire.", "Information");
            }
        }

        /// <summary>
        /// Modifier l'etape de la commande du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifCommandeDVD_Click(object sender, EventArgs e)
        {
            if (dgvReceptionCommandesListeDVD.SelectedRows.Count > 0)
            {
                // Récupérer la commande sélectionnée
                Commande commande = (Commande)bdgCommandesListeDvd.List[bdgCommandesListeDvd.Position];
                string currentEtape = commande.EtapeSuivi;

                // Récupérer la nouvelle étape sélectionnée dans la comboBox
                if (cbxNouvelleEtapeDVD.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner une nouvelle étape de suivi.", "Information");
                    return;
                }
                Suivi newSuivi = (Suivi)cbxNouvelleEtapeDVD.SelectedItem;
                string newEtape = newSuivi.Etape;

                // Vérifier les règles de transition d'étape
                if ((currentEtape == "livrée" || currentEtape == "réglée") && (newEtape == "en cours" || newEtape == "relancée"))
                {
                    MessageBox.Show("Une commande livrée ou réglée ne peut pas revenir à une étape précédente (en cours ou relancée).", "Erreur de transition");
                    return;
                }
                if (newEtape == "réglée" && currentEtape != "livrée")
                {
                    MessageBox.Show("Une commande ne peut pas être réglée si elle n'est pas livrée.", "Erreur de transition");
                    return;
                }

                // Mise à jour de la commande avec la nouvelle étape
                commande.SuiviId = int.Parse(newSuivi.Id);
                commande.EtapeSuivi = newEtape;

                // Envoyer la mise à jour à la base de données via l'API
                if (controller.ModifierCommande(commande))
                {
                    MessageBox.Show("Commande modifiée avec succès.");
                    AfficheReceptionCommandesDvd(); // Mettre à jour l'affichage des commandes
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification de la commande.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande à modifier.", "Information");
            }
        }

        /// <summary>
        /// combobox pour le suivi d'une commande d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxNouvelleEtapeDVD_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Bloc vide supprimé car inutile
        }

        /// <summary>
        /// Rempli le combo suivi
        /// </summary>
        /// <param name="lesSuivi">liste des objets de type Suivi</param>
        /// <param name="bdgSuiviDvd">bindingsource contenant les informations</param>
        /// <param name="cbxNouvelleEtapeDVD">combobox à remplir</param>
        private static void RemplirComboSuiviDvd(List<Suivi> lesSuivi, BindingSource bdgSuiviDvd, ComboBox cbxNouvelleEtapeDVD)
        {
            bdgSuiviDvd.DataSource = lesSuivi;
            cbxNouvelleEtapeDVD.DataSource = bdgSuiviDvd;
            cbxNouvelleEtapeDVD.DisplayMember = "Etape";
            cbxNouvelleEtapeDVD.ValueMember = "Id";
            if (cbxNouvelleEtapeDVD.Items.Count > 0)
            {
                cbxNouvelleEtapeDVD.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Suppression d'une commande d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprCommandeDVD_Click(object sender, EventArgs e)
        {
            if (dgvReceptionCommandesListeDVD.SelectedRows.Count > 0)
            {
                Commande commande = (Commande)bdgCommandesListeDvd.List[bdgCommandesListeDvd.Position];
                if (commande.EtapeSuivi == "en cours" || commande.EtapeSuivi == "relancée")
                {
                    if (controller.SupprimerCommande(commande.Id))
                    {
                        MessageBox.Show("Commande supprimée avec succès.");
                        // Mettre à jour l'affichage des commandes après suppression
                        AfficheReceptionCommandesDvd();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de la commande.", "Erreur");
                    }
                }
                else
                {
                    MessageBox.Show("La commande doit être non livrée ou en cours pour être supprimée.", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande à supprimer.", "Information");
            }
        }

        #endregion

        #region Onglet Commandes Revue
        private readonly BindingSource bdgCommandesListeRevue = new BindingSource();
        private List<Abonnement> lesCommandesRevue = new List<Abonnement>();

        /// <summary>
        /// Ouverture de l'onglet Commande de Revue :
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandeRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
        }

        /// <summary>
        /// Rechercher une ou des commandes pour une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevueNumRechercheCom_Click(object sender, EventArgs e)
        {
            if (!TxbRevueNumRechercheCom.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(TxbRevueNumRechercheCom.Text));
                if (revue != null)
                {
                    AfficheReceptionCommandesRevue(revue);
                    TxbNumRevueCom.Text = TxbRevueNumRechercheCom.Text;
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné et les commandes liées à cette revue
        /// </summary>
        private void AfficheReceptionCommandesRevue(Revue revue)
        {
            AfficheRevueInfosCom(revue);
            AfficheReceptionCommandesRevue();
        }

        /// <summary>
        /// Récupère et affiche les commandes d'une revue
        /// </summary>
        private void AfficheReceptionCommandesRevue()
        {
            string idRevue = TxbRevueNumRechercheCom.Text;
            lesCommandesRevue = controller.GetCommandesRevue(idRevue);

            if (lesCommandesRevue != null && lesCommandesRevue.Count > 0)
            {
                lesCommandesRevue = lesCommandesRevue.OrderByDescending(o => o.DateCommande).ToList();
                RemplirReceptionCommandesRevue(lesCommandesRevue);
                AccesReceptionCommandeGroupBoxRevue(true);
            }
            else
            {
                MessageBox.Show("Aucune commande trouvée pour cette revue.");
                DgvReceptionCommandesListeRevue.DataSource = null;
            }
        }

        /// <summary>
        /// Affichage des informations d'une revue sélectionné
        /// </summary>
        /// <param name="revue">le revue</param>
        private void AfficheRevueInfosCom(Revue revue)
        {
            TxbCommandeRevuesTitre.Text = revue.Titre;
            TxbCommandeRevuesPeriodicite.Text = revue.Periodicite;
            TxbCommandeRevuesImage.Text = revue.Image;
            TxbCommandeRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            TxbCommandeRevuesGenre.Text = revue.Genre;
            TxbCommandeRevuesPublic.Text = revue.Public;
            TxbCommandeRevuesRayon.Text = revue.Rayon;
            string image = revue.Image;
            try
            {
                PcbCommandeRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                PcbCommandeRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'une commande et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionCommandeGroupBoxRevue(bool acces)
        {
            GrpAjoutCommandesRevue.Enabled = acces;
            TxbNumeroCommandeRevue.Text = "";
            DtpCommandeDateRevue.Value = DateTime.Now;
            TxbMontantCommandeRevue.Text = "";
            DtpCommandeDateFinRevue.Value = DateTime.Now;
            TxbNumRevueCom.Text = TxbRevueNumRechercheCom.Text;
        }

        /// <summary>
        /// Ajout d'une commande d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjoutCommandeRevue_Click(object sender, EventArgs e)
        {
            if (!TxbRevueNumRechercheCom.Text.Equals(""))
            {
                try
                {
                    // Correction des types des champs
                    string id = TxbNumeroCommandeRevue.Text;
                    string idRevue = TxbNumRevueCom.Text;
                    DateTime dateCommande = DtpCommandeDateRevue.Value;
                    int montant = int.Parse(TxbMontantCommandeRevue.Text);
                    DateTime dateFinAbonnement = DtpCommandeDateFinRevue.Value;
                    Abonnement abonnement = new Abonnement(id, idRevue, dateCommande, montant, dateFinAbonnement);
                    if (controller.CreerAbonnement(abonnement))
                    {
                        MessageBox.Show("Commande ajoutée avec succès.");
                        AfficheReceptionCommandesRevue();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout de la commande.");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Le champ 'Montant' doit être numériques.", "Erreur de format");
                    TxbNumeroCommandeRevue.Text = "";
                    TxbNumeroCommandeRevue.Focus();
                }
            }
            else
            {
                MessageBox.Show("Le numéro de commande est obligatoire.", "Information");
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvReceptionCommandesListeRevue_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string titreColonne = DgvReceptionCommandesListeRevue.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandesRevue.OrderBy(o => o.Id).ToList();
                    break;
                case "IdRevue":
                    sortedList = lesCommandesRevue.OrderBy(o => o.IdRevue).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandesRevue.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesRevue.OrderBy(o => o.Montant).ToList();
                    break;
                case "DateFinAbonnement":
                    sortedList = lesCommandesRevue.OrderBy(o => o.DateFinAbonnement).ToList();
                    break;
            }
            RemplirReceptionCommandesRevue(sortedList);
        }

        /// <summary>
        /// Remplit le DataGridView des commandes avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesCommandesRevue">liste de commandes</param>
        private void RemplirReceptionCommandesRevue(List<Abonnement> lesCommandesRevue)
        {
            if (lesCommandesRevue != null)
            {
                bdgCommandesListeRevue.DataSource = lesCommandesRevue;
                DgvReceptionCommandesListeRevue.DataSource = bdgCommandesListeRevue;
                if (DgvReceptionCommandesListeRevue.Columns.Count >= 6)
                {
                    DgvReceptionCommandesListeRevue.Columns["Id"].DisplayIndex = 1;
                    DgvReceptionCommandesListeRevue.Columns["IdRevue"].DisplayIndex = 2;
                    DgvReceptionCommandesListeRevue.Columns["DateCommande"].DisplayIndex = 3;
                    DgvReceptionCommandesListeRevue.Columns["Montant"].DisplayIndex = 4;
                    DgvReceptionCommandesListeRevue.Columns["DateFinAbonnement"].DisplayIndex = 5;
                }
            }
            else
            {
                bdgCommandesListeRevue.DataSource = null;
            }
        }

        /// <summary>
        /// Modifier l'etape de la commande d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifCommandeRevue_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est bien sélectionnée
            if (string.IsNullOrEmpty(TxbNumComRevueModifier.Text))
            {
                MessageBox.Show("Veuillez sélectionner une commande à modifier.", "Information");
                return;
            }
            // Récupérer la commande en cours de modification
            string idCommande = TxbNumComRevueModifier.Text;
            Abonnement abonnement = lesCommandesRevue.FirstOrDefault(a => a.Id == idCommande);
            if (abonnement == null)
            {
                MessageBox.Show("Erreur lors de la récupération de la commande.", "Erreur");
                return;
            }
            // Vérifier et enregistrer la nouvelle date de fin d'abonnement
            DateTime nouvelleDateFin = DtpCommandeDateFinRevueModifier.Value;
            if (nouvelleDateFin <= abonnement.DateCommande)
            {
                MessageBox.Show("La date de fin d'abonnement doit être postérieure à la date de commande.", "Erreur de saisie");
                return;
            }
            // Vérifier et enregistrer le montant
            if (!int.TryParse(TxbMontantCommandeRevueModifier.Text, out int nouveauMontant) || nouveauMontant <= 0)
            {
                MessageBox.Show("Veuillez entrer un montant valide.", "Erreur de saisie");
                return;
            }
            // Mettre à jour l'abonnement
            abonnement.DateFinAbonnement = nouvelleDateFin;
            abonnement.Montant = nouveauMontant;
            // Mise à jour dans la base de données
            if (controller.ModifierAbonnement(abonnement))
            {
                MessageBox.Show("Commande modifiée avec succès.");
                AfficheReceptionCommandesRevue(); // Rafraîchir la liste
            }
            else
            {
                MessageBox.Show("Erreur lors de la modification de la commande.", "Erreur");
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvReceptionCommandesListeRevue_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Vérifier si une ligne valide est sélectionnée
            if (e.RowIndex >= 0 && DgvReceptionCommandesListeRevue.SelectedRows.Count > 0 &&
                DgvReceptionCommandesListeRevue.SelectedRows[0].DataBoundItem is Abonnement abonnement)
            {
                // Afficher les informations dans les champs de modification
                TxbNumRevueComModifier.Text = abonnement.IdRevue;
                TxbNumComRevueModifier.Text = abonnement.Id;
                DtpCommandeDateRevue.Value = abonnement.DateCommande;
                DtpCommandeDateRevueModifier.Enabled = false; // Empêcher la modification de la date de commande
                DtpCommandeDateFinRevueModifier.Value = abonnement.DateFinAbonnement;
                TxbMontantCommandeRevueModifier.Text = abonnement.Montant.ToString(); // Ajout du montant
            }
        }

        /// <summary>
        /// Suppression d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprCommandeRevue_Click(object sender, EventArgs e)
        {
            if (DgvReceptionCommandesListeRevue.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande à supprimer.", "Information");
                return;
            }
            Abonnement abonnement = (Abonnement)bdgCommandesListeRevue.Current;
            if (abonnement == null)
            {
                MessageBox.Show("Erreur lors de la récupération de la commande.", "Erreur");
                return;
            }
            // Vérifier si des exemplaires sont rattachés à cette commande
            List<Exemplaire> exemplaires = controller.GetExemplairesRevue(abonnement.IdRevue);
            bool exemplaireExiste = exemplaires.Any(ex => FrmMediatekController.ParutionDansAbonnement(abonnement.DateCommande, abonnement.DateFinAbonnement, ex.DateAchat));
            if (exemplaireExiste)
            {
                MessageBox.Show("Impossible de supprimer la commande, des exemplaires y sont rattachés.", "Erreur");
                return;
            }
            if (controller.SupprimerAbonnement(abonnement.Id))
            {
                MessageBox.Show("Commande supprimée avec succès.");
                AfficheReceptionCommandesRevue();
            }
            else
            {
                MessageBox.Show("Erreur lors de la suppression de la commande.", "Erreur");
            }
        }

        #endregion

        #region FrmAuthentification
        /// <summary>
        /// Gere l'acces par rapport au service
        /// </summary>
        private void GererAccesParService()
        {
            switch (utilisateur.Service)
            {
                case "Administratif":
                    break; // tout visible

                case "Bibliothécaire":
                    tabOngletsApplication.TabPages.Remove(tabCommandeLivre);
                    tabOngletsApplication.TabPages.Remove(tabCommandeDvd);
                    tabOngletsApplication.TabPages.Remove(TabCommandeRevue);
                    break;

                default:
                    MessageBox.Show("Votre service ne permet pas d'accéder à cette application.", "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                    break;
            }
        }

        #endregion

    }
}
