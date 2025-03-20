namespace MediaTekDocuments.view
{
    partial class FrmAlerte
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.BtnValider = new System.Windows.Forms.Button();
            this.ListViewAbonnements = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chargement des abonnements en cours...";
            // 
            // BtnValider
            // 
            this.BtnValider.Location = new System.Drawing.Point(165, 269);
            this.BtnValider.Name = "BtnValider";
            this.BtnValider.Size = new System.Drawing.Size(75, 23);
            this.BtnValider.TabIndex = 2;
            this.BtnValider.Text = "Valider";
            this.BtnValider.UseVisualStyleBackColor = true;
            this.BtnValider.Click += new System.EventHandler(this.BtnValider_Click);
            // 
            // ListViewAbonnements
            // 
            this.ListViewAbonnements.FullRowSelect = true;
            this.ListViewAbonnements.HideSelection = false;
            this.ListViewAbonnements.Location = new System.Drawing.Point(20, 50);
            this.ListViewAbonnements.Name = "ListViewAbonnements";
            this.ListViewAbonnements.Size = new System.Drawing.Size(350, 200);
            this.ListViewAbonnements.TabIndex = 1;
            this.ListViewAbonnements.UseCompatibleStateImageBehavior = false;
            this.ListViewAbonnements.View = System.Windows.Forms.View.Details;
            // 
            // FrmAlerte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 304);
            this.Controls.Add(this.BtnValider);
            this.Controls.Add(this.ListViewAbonnements);
            this.Controls.Add(this.label1);
            this.Name = "FrmAlerte";
            this.Text = "FrmAlerte";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnValider;
        private System.Windows.Forms.ListView ListViewAbonnements;
    }
}