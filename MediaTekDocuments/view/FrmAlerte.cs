using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class FrmAlerte : Form
    {
        private readonly FrmMediatekController controller;
        private readonly List<Abonnement> abonnements;
        private readonly Utilisateur utilisateur;

        public FrmAlerte(Utilisateur utilisateur)
        {
            InitializeComponent();
            this.utilisateur = utilisateur;

            // Restriction d'accès
            if (utilisateur.Service != "Administratif")
            {
                MessageBox.Show("Vous n'avez pas accès à l'alerte des abonnements.", "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            controller = new FrmMediatekController();
            abonnements = controller.GetAbonnementsExpirantDans30Jours();
            ChargerListView();
        }

        /// <summary>
        /// Chargement de la liste des abonnements qui expirent dans 30 jours
        /// </summary>
        private void ChargerListView()
        {
            ListViewAbonnements.Items.Clear();
            ListViewAbonnements.Columns.Add("Titre Revue", 200);
            ListViewAbonnements.Columns.Add("Date Fin", 100);

            if (abonnements == null || abonnements.Count == 0)
            {
                MessageBox.Show("Aucun abonnement expirant trouvé.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var abonnement in abonnements)
            {
                ListViewItem item = new ListViewItem(abonnement.TitreRevue);
                item.SubItems.Add(abonnement.DateFinAbonnement.ToShortDateString());
                ListViewAbonnements.Items.Add(item);
            }

            ListViewAbonnements.Refresh();
        }

        /// <summary>
        /// Bouton de validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValider_Click(object sender, EventArgs e)
        {
            FrmMediatek frmMain = new FrmMediatek(utilisateur);
            frmMain.Show();
            this.Hide();
        }
    }
}
