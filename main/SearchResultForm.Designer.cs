using System;
using System.Windows.Forms;

namespace main
{
    partial class SearchResultForm
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
            this.SearchResultTabs = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // SearchResultTabs
            // 
            this.SearchResultTabs.Location = new System.Drawing.Point(0, 0);
            this.SearchResultTabs.Name = "SearchResultTabs";
            this.SearchResultTabs.SelectedIndex = 0;
            this.SearchResultTabs.Size = new System.Drawing.Size(586, 363);
            this.SearchResultTabs.TabIndex = 0;
            // 
            // SearchResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.SearchResultTabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SearchResultForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Результаты поиска";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SearchResultForm_FormClosed);
            this.Shown += new System.EventHandler(this.SearchResultForm_Shown);
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.TabControl SearchResultTabs;
        private System.Windows.Forms.TabPage NoResultTab;
        private System.Windows.Forms.Label[][] RouteInfoLabels;
        private System.Windows.Forms.TabPage[] TabPages;
    }
}