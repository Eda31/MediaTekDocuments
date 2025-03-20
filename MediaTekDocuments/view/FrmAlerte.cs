using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;

namespace MediaTekDocuments.view
{
    public partial class FrmAlerte : Form
    {
        private readonly FrmMediatekController controller;
        private readonly List<Abonnement> abonnements;

        public FrmAlerte()
        {
            InitializeComponent();
            
            controller = new FrmMediatekController();
            abonnements = controller.GetAbonnementsExpirantDans30Jours();
            ChargerListView();
        }

        /// <summary>
        /// Charge la liste des abonnements expirants
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
                // Utiliser le titre au lieu de l'ID
                ListViewItem item = new ListViewItem(abonnement.TitreRevue);
                item.SubItems.Add(abonnement.DateFinAbonnement.ToShortDateString());
                ListViewAbonnements.Items.Add(item);
            }
            // Forcer le rafraîchissement
            ListViewAbonnements.Refresh();
        }

        /// <summary>
        /// Bouton pour valider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValider_Click(object sender, EventArgs e)
        {
            FrmMediatek frmMain = new FrmMediatek();
            frmMain.Show();
            this.Hide();
        }
    }
}
