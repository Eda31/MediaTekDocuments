using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Formulaire d'authentification
    /// </summary>
    public partial class FrmAuthentification : Form
    {
        private readonly FrmMediatekController controller;

        /// <summary>
        /// Constructeur de la classe FrmAuthentification
        /// </summary>
        public FrmAuthentification()
        {
            InitializeComponent();
            controller = new FrmMediatekController();
        }

        /// <summary>
        /// Bouton de connexion
        /// </summary>
        private void BtnConnection_Click(object sender, EventArgs e)
        {
            string login = TxtLogin.Text;
            string motDePasse = TxtMotDePasse.Text;
            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Veuillez entrer un login.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Utilisateur utilisateur = controller.AuthentifierUtilisateur(login, motDePasse);
            if (utilisateur == null)
            {
                MessageBox.Show("Login invalide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (utilisateur.Service.Equals("Culture", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Accès refusé. Vous n'avez pas les droits pour utiliser cette application.", "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }
            if (utilisateur.Service.Equals("Administratif", StringComparison.OrdinalIgnoreCase))
            {
                FrmAlerte frmAlerte = new FrmAlerte(utilisateur);
                this.Hide();
                frmAlerte.ShowDialog();
                return;
            }
            FrmMediatek frm = new FrmMediatek(utilisateur);
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }
    }
}
