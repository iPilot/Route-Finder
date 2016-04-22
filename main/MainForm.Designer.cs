namespace main
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.SearchButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuItemDb = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StartPointBox = new System.Windows.Forms.ComboBox();
            this.EndPointBox = new System.Windows.Forms.ComboBox();
            this.StartPointLabel = new System.Windows.Forms.Label();
            this.EndPointLabel = new System.Windows.Forms.Label();
            this.RouteSearchLog = new System.Windows.Forms.RichTextBox();
            this.RouteCriteriaSelector = new System.Windows.Forms.ComboBox();
            this.RouteCriteriaLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SearchButton
            // 
            this.SearchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SearchButton.Location = new System.Drawing.Point(280, 98);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(90, 43);
            this.SearchButton.TabIndex = 0;
            this.SearchButton.Text = "Поиск";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemDb,
            this.MenuItemSettings,
            this.ExitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(429, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuItemDb
            // 
            this.MenuItemDb.Name = "MenuItemDb";
            this.MenuItemDb.Size = new System.Drawing.Size(86, 20);
            this.MenuItemDb.Text = "База данных";
            this.MenuItemDb.Click += new System.EventHandler(this.MenuItemDb_Click);
            // 
            // MenuItemSettings
            // 
            this.MenuItemSettings.Name = "MenuItemSettings";
            this.MenuItemSettings.Size = new System.Drawing.Size(83, 20);
            this.MenuItemSettings.Text = "Параметры";
            this.MenuItemSettings.Click += new System.EventHandler(this.MenuItemSettings_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ExitToolStripMenuItem.Text = "Выход";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // StartPointBox
            // 
            this.StartPointBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartPointBox.FormattingEnabled = true;
            this.StartPointBox.IntegralHeight = false;
            this.StartPointBox.Location = new System.Drawing.Point(10, 46);
            this.StartPointBox.Name = "StartPointBox";
            this.StartPointBox.Size = new System.Drawing.Size(160, 28);
            this.StartPointBox.TabIndex = 3;
            // 
            // EndPointBox
            // 
            this.EndPointBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EndPointBox.FormattingEnabled = true;
            this.EndPointBox.IntegralHeight = false;
            this.EndPointBox.Location = new System.Drawing.Point(210, 46);
            this.EndPointBox.Name = "EndPointBox";
            this.EndPointBox.Size = new System.Drawing.Size(160, 28);
            this.EndPointBox.TabIndex = 4;
            // 
            // StartPointLabel
            // 
            this.StartPointLabel.AutoSize = true;
            this.StartPointLabel.Location = new System.Drawing.Point(10, 30);
            this.StartPointLabel.Name = "StartPointLabel";
            this.StartPointLabel.Size = new System.Drawing.Size(43, 13);
            this.StartPointLabel.TabIndex = 5;
            this.StartPointLabel.Text = "Откуда";
            // 
            // EndPointLabel
            // 
            this.EndPointLabel.AutoSize = true;
            this.EndPointLabel.Location = new System.Drawing.Point(210, 30);
            this.EndPointLabel.Name = "EndPointLabel";
            this.EndPointLabel.Size = new System.Drawing.Size(31, 13);
            this.EndPointLabel.TabIndex = 6;
            this.EndPointLabel.Text = "Куда";
            // 
            // RouteSearchLog
            // 
            this.RouteSearchLog.Location = new System.Drawing.Point(10, 166);
            this.RouteSearchLog.Name = "RouteSearchLog";
            this.RouteSearchLog.Size = new System.Drawing.Size(360, 129);
            this.RouteSearchLog.TabIndex = 7;
            this.RouteSearchLog.Text = "";
            // 
            // RouteCriteriaSelector
            // 
            this.RouteCriteriaSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RouteCriteriaSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RouteCriteriaSelector.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RouteCriteriaSelector.FormattingEnabled = true;
            this.RouteCriteriaSelector.Items.AddRange(new object[] {
            "Расстояние",
            "Время",
            "Стоимость"});
            this.RouteCriteriaSelector.Location = new System.Drawing.Point(10, 105);
            this.RouteCriteriaSelector.Name = "RouteCriteriaSelector";
            this.RouteCriteriaSelector.Size = new System.Drawing.Size(160, 28);
            this.RouteCriteriaSelector.TabIndex = 8;
            // 
            // RouteCriteriaLabel
            // 
            this.RouteCriteriaLabel.AutoSize = true;
            this.RouteCriteriaLabel.Location = new System.Drawing.Point(10, 89);
            this.RouteCriteriaLabel.Name = "RouteCriteriaLabel";
            this.RouteCriteriaLabel.Size = new System.Drawing.Size(97, 13);
            this.RouteCriteriaLabel.TabIndex = 9;
            this.RouteCriteriaLabel.Text = "Критерий поиска:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(429, 307);
            this.Controls.Add(this.RouteCriteriaLabel);
            this.Controls.Add(this.RouteCriteriaSelector);
            this.Controls.Add(this.RouteSearchLog);
            this.Controls.Add(this.EndPointLabel);
            this.Controls.Add(this.StartPointLabel);
            this.Controls.Add(this.EndPointBox);
            this.Controls.Add(this.StartPointBox);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Поиск маршрутов";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDb;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSettings;
        private System.Windows.Forms.ComboBox StartPointBox;
        private System.Windows.Forms.ComboBox EndPointBox;
        private System.Windows.Forms.Label StartPointLabel;
        private System.Windows.Forms.Label EndPointLabel;
        private System.Windows.Forms.RichTextBox RouteSearchLog;
        private System.Windows.Forms.ComboBox RouteCriteriaSelector;
        private System.Windows.Forms.Label RouteCriteriaLabel;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
    }
}

