namespace main
{
    partial class DbForm
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
            this.components = new System.ComponentModel.Container();
            this.AddCityButton = new System.Windows.Forms.Button();
            this.DbAddNewCityText = new System.Windows.Forms.TextBox();
            this.DbTabs = new System.Windows.Forms.TabControl();
            this.DbAddTab = new System.Windows.Forms.TabPage();
            this.DbAddRouteOptions = new System.Windows.Forms.Panel();
            this.DbAddRouteStartLabel = new System.Windows.Forms.Label();
            this.DbAddRouteStartSelector = new System.Windows.Forms.ComboBox();
            this.DbAddRouteCostLabel = new System.Windows.Forms.Label();
            this.DbAddRouteEndLabel = new System.Windows.Forms.Label();
            this.DbAddRouteTimeLabel = new System.Windows.Forms.Label();
            this.DbAddRouteEndSelector = new System.Windows.Forms.ComboBox();
            this.DbAddRouteCost = new System.Windows.Forms.TextBox();
            this.DbAddRouteLenghtLabel = new System.Windows.Forms.Label();
            this.DbAddRouteTime = new System.Windows.Forms.TextBox();
            this.DbAddRouteLenght = new System.Windows.Forms.TextBox();
            this.DbAddNewCityLabel = new System.Windows.Forms.Label();
            this.DbAddBox = new System.Windows.Forms.GroupBox();
            this.DbAddRoute = new System.Windows.Forms.RadioButton();
            this.DbAddCity = new System.Windows.Forms.RadioButton();
            this.DbDeleteTab = new System.Windows.Forms.TabPage();
            this.DbDeleteWarningLabel = new System.Windows.Forms.Label();
            this.DbDeleteRouteSelector = new System.Windows.Forms.ComboBox();
            this.DbDeleteLabel = new System.Windows.Forms.Label();
            this.DbDeleteCitySelector = new System.Windows.Forms.ComboBox();
            this.DbDeleteButton = new System.Windows.Forms.Button();
            this.DbDeleteBox = new System.Windows.Forms.GroupBox();
            this.DbDeleteRoute = new System.Windows.Forms.RadioButton();
            this.DbDeleteCity = new System.Windows.Forms.RadioButton();
            this.DbCitiesTab = new System.Windows.Forms.TabPage();
            this.DbRoutesTab = new System.Windows.Forms.TabPage();
            this.DbRoutesDataGrid = new System.Windows.Forms.DataGridView();
            this.DbLogTab = new System.Windows.Forms.TabPage();
            this.DbLogText = new System.Windows.Forms.RichTextBox();
            this.DbClearLog = new System.Windows.Forms.Button();
            this.DbShowResultTimer = new System.Windows.Forms.Timer(this.components);
            this.DbActionResult = new System.Windows.Forms.StatusStrip();
            this.DbActionSuccessLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.DbActionErrorLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.DbRoutesColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DbRoutesColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DbRoutesColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DbRoutesColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DbRoutesColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DbTabs.SuspendLayout();
            this.DbAddTab.SuspendLayout();
            this.DbAddRouteOptions.SuspendLayout();
            this.DbAddBox.SuspendLayout();
            this.DbDeleteTab.SuspendLayout();
            this.DbDeleteBox.SuspendLayout();
            this.DbRoutesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DbRoutesDataGrid)).BeginInit();
            this.DbLogTab.SuspendLayout();
            this.DbActionResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddCityButton
            // 
            this.AddCityButton.AutoSize = true;
            this.AddCityButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddCityButton.Location = new System.Drawing.Point(143, 225);
            this.AddCityButton.Name = "AddCityButton";
            this.AddCityButton.Size = new System.Drawing.Size(94, 40);
            this.AddCityButton.TabIndex = 1;
            this.AddCityButton.Text = "Добавить";
            this.AddCityButton.UseVisualStyleBackColor = true;
            this.AddCityButton.Click += new System.EventHandler(this.AddCityButton_Click);
            // 
            // DbAddNewCityText
            // 
            this.DbAddNewCityText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbAddNewCityText.Location = new System.Drawing.Point(70, 84);
            this.DbAddNewCityText.MaxLength = 25;
            this.DbAddNewCityText.Name = "DbAddNewCityText";
            this.DbAddNewCityText.Size = new System.Drawing.Size(240, 26);
            this.DbAddNewCityText.TabIndex = 4;
            // 
            // DbTabs
            // 
            this.DbTabs.Controls.Add(this.DbAddTab);
            this.DbTabs.Controls.Add(this.DbDeleteTab);
            this.DbTabs.Controls.Add(this.DbCitiesTab);
            this.DbTabs.Controls.Add(this.DbRoutesTab);
            this.DbTabs.Controls.Add(this.DbLogTab);
            this.DbTabs.Location = new System.Drawing.Point(-1, 0);
            this.DbTabs.Name = "DbTabs";
            this.DbTabs.SelectedIndex = 0;
            this.DbTabs.Size = new System.Drawing.Size(388, 300);
            this.DbTabs.TabIndex = 8;
            // 
            // DbAddTab
            // 
            this.DbAddTab.Controls.Add(this.DbAddRouteOptions);
            this.DbAddTab.Controls.Add(this.DbAddNewCityLabel);
            this.DbAddTab.Controls.Add(this.DbAddBox);
            this.DbAddTab.Controls.Add(this.AddCityButton);
            this.DbAddTab.Controls.Add(this.DbAddNewCityText);
            this.DbAddTab.Location = new System.Drawing.Point(4, 22);
            this.DbAddTab.Name = "DbAddTab";
            this.DbAddTab.Padding = new System.Windows.Forms.Padding(3);
            this.DbAddTab.Size = new System.Drawing.Size(380, 274);
            this.DbAddTab.TabIndex = 0;
            this.DbAddTab.Text = "Добавить";
            this.DbAddTab.UseVisualStyleBackColor = true;
            // 
            // DbAddRouteOptions
            // 
            this.DbAddRouteOptions.Controls.Add(this.DbAddRouteStartLabel);
            this.DbAddRouteOptions.Controls.Add(this.DbAddRouteStartSelector);
            this.DbAddRouteOptions.Controls.Add(this.DbAddRouteCostLabel);
            this.DbAddRouteOptions.Controls.Add(this.DbAddRouteEndLabel);
            this.DbAddRouteOptions.Controls.Add(this.DbAddRouteTimeLabel);
            this.DbAddRouteOptions.Controls.Add(this.DbAddRouteEndSelector);
            this.DbAddRouteOptions.Controls.Add(this.DbAddRouteCost);
            this.DbAddRouteOptions.Controls.Add(this.DbAddRouteLenghtLabel);
            this.DbAddRouteOptions.Controls.Add(this.DbAddRouteTime);
            this.DbAddRouteOptions.Controls.Add(this.DbAddRouteLenght);
            this.DbAddRouteOptions.Location = new System.Drawing.Point(65, 60);
            this.DbAddRouteOptions.Name = "DbAddRouteOptions";
            this.DbAddRouteOptions.Size = new System.Drawing.Size(250, 155);
            this.DbAddRouteOptions.TabIndex = 18;
            this.DbAddRouteOptions.Visible = false;
            // 
            // DbAddRouteStartLabel
            // 
            this.DbAddRouteStartLabel.AutoSize = true;
            this.DbAddRouteStartLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbAddRouteStartLabel.Location = new System.Drawing.Point(5, 8);
            this.DbAddRouteStartLabel.Name = "DbAddRouteStartLabel";
            this.DbAddRouteStartLabel.Size = new System.Drawing.Size(85, 13);
            this.DbAddRouteStartLabel.TabIndex = 18;
            this.DbAddRouteStartLabel.Text = "Начало дороги:";
            // 
            // DbAddRouteStartSelector
            // 
            this.DbAddRouteStartSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbAddRouteStartSelector.FormattingEnabled = true;
            this.DbAddRouteStartSelector.IntegralHeight = false;
            this.DbAddRouteStartSelector.Location = new System.Drawing.Point(5, 24);
            this.DbAddRouteStartSelector.MaxDropDownItems = 10;
            this.DbAddRouteStartSelector.MaxLength = 35;
            this.DbAddRouteStartSelector.Name = "DbAddRouteStartSelector";
            this.DbAddRouteStartSelector.Size = new System.Drawing.Size(240, 26);
            this.DbAddRouteStartSelector.TabIndex = 13;
            // 
            // DbAddRouteCostLabel
            // 
            this.DbAddRouteCostLabel.AutoSize = true;
            this.DbAddRouteCostLabel.Location = new System.Drawing.Point(175, 109);
            this.DbAddRouteCostLabel.Name = "DbAddRouteCostLabel";
            this.DbAddRouteCostLabel.Size = new System.Drawing.Size(65, 13);
            this.DbAddRouteCostLabel.TabIndex = 17;
            this.DbAddRouteCostLabel.Text = "Стоимость:";
            // 
            // DbAddRouteEndLabel
            // 
            this.DbAddRouteEndLabel.AutoSize = true;
            this.DbAddRouteEndLabel.Location = new System.Drawing.Point(5, 55);
            this.DbAddRouteEndLabel.Margin = new System.Windows.Forms.Padding(0);
            this.DbAddRouteEndLabel.Name = "DbAddRouteEndLabel";
            this.DbAddRouteEndLabel.Size = new System.Drawing.Size(79, 13);
            this.DbAddRouteEndLabel.TabIndex = 9;
            this.DbAddRouteEndLabel.Text = "Конец дороги:";
            // 
            // DbAddRouteTimeLabel
            // 
            this.DbAddRouteTimeLabel.AutoSize = true;
            this.DbAddRouteTimeLabel.Location = new System.Drawing.Point(90, 109);
            this.DbAddRouteTimeLabel.Name = "DbAddRouteTimeLabel";
            this.DbAddRouteTimeLabel.Size = new System.Drawing.Size(43, 13);
            this.DbAddRouteTimeLabel.TabIndex = 16;
            this.DbAddRouteTimeLabel.Text = "Время:";
            // 
            // DbAddRouteEndSelector
            // 
            this.DbAddRouteEndSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbAddRouteEndSelector.FormattingEnabled = true;
            this.DbAddRouteEndSelector.IntegralHeight = false;
            this.DbAddRouteEndSelector.Location = new System.Drawing.Point(5, 71);
            this.DbAddRouteEndSelector.MaxDropDownItems = 10;
            this.DbAddRouteEndSelector.MaxLength = 35;
            this.DbAddRouteEndSelector.Name = "DbAddRouteEndSelector";
            this.DbAddRouteEndSelector.Size = new System.Drawing.Size(240, 26);
            this.DbAddRouteEndSelector.TabIndex = 12;
            // 
            // DbAddRouteCost
            // 
            this.DbAddRouteCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbAddRouteCost.Location = new System.Drawing.Point(175, 125);
            this.DbAddRouteCost.MaxLength = 6;
            this.DbAddRouteCost.Name = "DbAddRouteCost";
            this.DbAddRouteCost.Size = new System.Drawing.Size(70, 22);
            this.DbAddRouteCost.TabIndex = 15;
            this.DbAddRouteCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // DbAddRouteLenghtLabel
            // 
            this.DbAddRouteLenghtLabel.AutoSize = true;
            this.DbAddRouteLenghtLabel.Location = new System.Drawing.Point(5, 109);
            this.DbAddRouteLenghtLabel.Name = "DbAddRouteLenghtLabel";
            this.DbAddRouteLenghtLabel.Size = new System.Drawing.Size(70, 13);
            this.DbAddRouteLenghtLabel.TabIndex = 10;
            this.DbAddRouteLenghtLabel.Text = "Расстояние:";
            // 
            // DbAddRouteTime
            // 
            this.DbAddRouteTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbAddRouteTime.Location = new System.Drawing.Point(90, 125);
            this.DbAddRouteTime.MaxLength = 6;
            this.DbAddRouteTime.Name = "DbAddRouteTime";
            this.DbAddRouteTime.Size = new System.Drawing.Size(70, 22);
            this.DbAddRouteTime.TabIndex = 14;
            this.DbAddRouteTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // DbAddRouteLenght
            // 
            this.DbAddRouteLenght.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbAddRouteLenght.Location = new System.Drawing.Point(5, 125);
            this.DbAddRouteLenght.MaxLength = 6;
            this.DbAddRouteLenght.Name = "DbAddRouteLenght";
            this.DbAddRouteLenght.Size = new System.Drawing.Size(70, 22);
            this.DbAddRouteLenght.TabIndex = 7;
            this.DbAddRouteLenght.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // DbAddNewCityLabel
            // 
            this.DbAddNewCityLabel.AutoSize = true;
            this.DbAddNewCityLabel.Location = new System.Drawing.Point(70, 68);
            this.DbAddNewCityLabel.Margin = new System.Windows.Forms.Padding(0);
            this.DbAddNewCityLabel.Name = "DbAddNewCityLabel";
            this.DbAddNewCityLabel.Size = new System.Drawing.Size(76, 13);
            this.DbAddNewCityLabel.TabIndex = 8;
            this.DbAddNewCityLabel.Text = "Новый город:";
            // 
            // DbAddBox
            // 
            this.DbAddBox.Controls.Add(this.DbAddRoute);
            this.DbAddBox.Controls.Add(this.DbAddCity);
            this.DbAddBox.Location = new System.Drawing.Point(120, 15);
            this.DbAddBox.Name = "DbAddBox";
            this.DbAddBox.Size = new System.Drawing.Size(140, 40);
            this.DbAddBox.TabIndex = 6;
            this.DbAddBox.TabStop = false;
            this.DbAddBox.Text = "Добавить";
            // 
            // DbAddRoute
            // 
            this.DbAddRoute.AutoSize = true;
            this.DbAddRoute.Location = new System.Drawing.Point(70, 15);
            this.DbAddRoute.Name = "DbAddRoute";
            this.DbAddRoute.Size = new System.Drawing.Size(63, 17);
            this.DbAddRoute.TabIndex = 1;
            this.DbAddRoute.Text = "Дорога";
            this.DbAddRoute.UseVisualStyleBackColor = true;
            this.DbAddRoute.CheckedChanged += new System.EventHandler(this.DbAddRoute_CheckedChanged);
            // 
            // DbAddCity
            // 
            this.DbAddCity.AutoSize = true;
            this.DbAddCity.Checked = true;
            this.DbAddCity.Location = new System.Drawing.Point(8, 15);
            this.DbAddCity.Name = "DbAddCity";
            this.DbAddCity.Size = new System.Drawing.Size(55, 17);
            this.DbAddCity.TabIndex = 0;
            this.DbAddCity.TabStop = true;
            this.DbAddCity.Text = "Город";
            this.DbAddCity.UseVisualStyleBackColor = true;
            // 
            // DbDeleteTab
            // 
            this.DbDeleteTab.Controls.Add(this.DbDeleteWarningLabel);
            this.DbDeleteTab.Controls.Add(this.DbDeleteRouteSelector);
            this.DbDeleteTab.Controls.Add(this.DbDeleteLabel);
            this.DbDeleteTab.Controls.Add(this.DbDeleteCitySelector);
            this.DbDeleteTab.Controls.Add(this.DbDeleteButton);
            this.DbDeleteTab.Controls.Add(this.DbDeleteBox);
            this.DbDeleteTab.Location = new System.Drawing.Point(4, 22);
            this.DbDeleteTab.Name = "DbDeleteTab";
            this.DbDeleteTab.Padding = new System.Windows.Forms.Padding(3);
            this.DbDeleteTab.Size = new System.Drawing.Size(380, 274);
            this.DbDeleteTab.TabIndex = 1;
            this.DbDeleteTab.Text = "Удалить";
            this.DbDeleteTab.UseVisualStyleBackColor = true;
            // 
            // DbDeleteWarningLabel
            // 
            this.DbDeleteWarningLabel.AutoSize = true;
            this.DbDeleteWarningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbDeleteWarningLabel.ForeColor = System.Drawing.Color.Red;
            this.DbDeleteWarningLabel.Location = new System.Drawing.Point(73, 115);
            this.DbDeleteWarningLabel.Name = "DbDeleteWarningLabel";
            this.DbDeleteWarningLabel.Size = new System.Drawing.Size(234, 30);
            this.DbDeleteWarningLabel.TabIndex = 6;
            this.DbDeleteWarningLabel.Text = "Удаление города приведет к удалению\r\nвсех дорог с ним связанных!";
            this.DbDeleteWarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DbDeleteRouteSelector
            // 
            this.DbDeleteRouteSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbDeleteRouteSelector.FormattingEnabled = true;
            this.DbDeleteRouteSelector.IntegralHeight = false;
            this.DbDeleteRouteSelector.Location = new System.Drawing.Point(70, 84);
            this.DbDeleteRouteSelector.MaxDropDownItems = 10;
            this.DbDeleteRouteSelector.Name = "DbDeleteRouteSelector";
            this.DbDeleteRouteSelector.Size = new System.Drawing.Size(240, 26);
            this.DbDeleteRouteSelector.TabIndex = 4;
            this.DbDeleteRouteSelector.Visible = false;
            // 
            // DbDeleteLabel
            // 
            this.DbDeleteLabel.AutoSize = true;
            this.DbDeleteLabel.Location = new System.Drawing.Point(70, 68);
            this.DbDeleteLabel.Name = "DbDeleteLabel";
            this.DbDeleteLabel.Size = new System.Drawing.Size(92, 13);
            this.DbDeleteLabel.TabIndex = 3;
            this.DbDeleteLabel.Text = "Выберите город:";
            // 
            // DbDeleteCitySelector
            // 
            this.DbDeleteCitySelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbDeleteCitySelector.FormattingEnabled = true;
            this.DbDeleteCitySelector.IntegralHeight = false;
            this.DbDeleteCitySelector.Location = new System.Drawing.Point(70, 84);
            this.DbDeleteCitySelector.MaxDropDownItems = 10;
            this.DbDeleteCitySelector.Name = "DbDeleteCitySelector";
            this.DbDeleteCitySelector.Size = new System.Drawing.Size(240, 26);
            this.DbDeleteCitySelector.TabIndex = 2;
            // 
            // DbDeleteButton
            // 
            this.DbDeleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbDeleteButton.Location = new System.Drawing.Point(143, 225);
            this.DbDeleteButton.Name = "DbDeleteButton";
            this.DbDeleteButton.Size = new System.Drawing.Size(94, 40);
            this.DbDeleteButton.TabIndex = 1;
            this.DbDeleteButton.Text = "Удалить";
            this.DbDeleteButton.UseVisualStyleBackColor = true;
            this.DbDeleteButton.Click += new System.EventHandler(this.DbDeleteButton_Click);
            // 
            // DbDeleteBox
            // 
            this.DbDeleteBox.Controls.Add(this.DbDeleteRoute);
            this.DbDeleteBox.Controls.Add(this.DbDeleteCity);
            this.DbDeleteBox.Location = new System.Drawing.Point(120, 15);
            this.DbDeleteBox.Name = "DbDeleteBox";
            this.DbDeleteBox.Size = new System.Drawing.Size(140, 40);
            this.DbDeleteBox.TabIndex = 0;
            this.DbDeleteBox.TabStop = false;
            this.DbDeleteBox.Text = "Удалить";
            // 
            // DbDeleteRoute
            // 
            this.DbDeleteRoute.AutoSize = true;
            this.DbDeleteRoute.Location = new System.Drawing.Point(70, 15);
            this.DbDeleteRoute.Name = "DbDeleteRoute";
            this.DbDeleteRoute.Size = new System.Drawing.Size(63, 17);
            this.DbDeleteRoute.TabIndex = 1;
            this.DbDeleteRoute.TabStop = true;
            this.DbDeleteRoute.Text = "Дорога";
            this.DbDeleteRoute.UseVisualStyleBackColor = true;
            this.DbDeleteRoute.CheckedChanged += new System.EventHandler(this.DbDeleteRoute_CheckedChanged);
            // 
            // DbDeleteCity
            // 
            this.DbDeleteCity.AutoSize = true;
            this.DbDeleteCity.Checked = true;
            this.DbDeleteCity.Location = new System.Drawing.Point(8, 15);
            this.DbDeleteCity.Name = "DbDeleteCity";
            this.DbDeleteCity.Size = new System.Drawing.Size(55, 17);
            this.DbDeleteCity.TabIndex = 0;
            this.DbDeleteCity.TabStop = true;
            this.DbDeleteCity.Text = "Город";
            this.DbDeleteCity.UseVisualStyleBackColor = true;
            // 
            // DbCitiesTab
            // 
            this.DbCitiesTab.Location = new System.Drawing.Point(4, 22);
            this.DbCitiesTab.Name = "DbCitiesTab";
            this.DbCitiesTab.Size = new System.Drawing.Size(380, 274);
            this.DbCitiesTab.TabIndex = 2;
            this.DbCitiesTab.Text = "Города";
            this.DbCitiesTab.UseVisualStyleBackColor = true;
            // 
            // DbRoutesTab
            // 
            this.DbRoutesTab.Controls.Add(this.DbRoutesDataGrid);
            this.DbRoutesTab.Location = new System.Drawing.Point(4, 22);
            this.DbRoutesTab.Name = "DbRoutesTab";
            this.DbRoutesTab.Size = new System.Drawing.Size(380, 274);
            this.DbRoutesTab.TabIndex = 3;
            this.DbRoutesTab.Text = "Дороги";
            this.DbRoutesTab.UseVisualStyleBackColor = true;
            // 
            // DbRoutesDataGrid
            // 
            this.DbRoutesDataGrid.AllowUserToAddRows = false;
            this.DbRoutesDataGrid.AllowUserToDeleteRows = false;
            this.DbRoutesDataGrid.AllowUserToResizeColumns = false;
            this.DbRoutesDataGrid.AllowUserToResizeRows = false;
            this.DbRoutesDataGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DbRoutesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DbRoutesDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DbRoutesColumn1,
            this.DbRoutesColumn2,
            this.DbRoutesColumn3,
            this.DbRoutesColumn4,
            this.DbRoutesColumn5});
            this.DbRoutesDataGrid.GridColor = System.Drawing.SystemColors.Desktop;
            this.DbRoutesDataGrid.Location = new System.Drawing.Point(-1, 0);
            this.DbRoutesDataGrid.Name = "DbRoutesDataGrid";
            this.DbRoutesDataGrid.ReadOnly = true;
            this.DbRoutesDataGrid.RowHeadersVisible = false;
            this.DbRoutesDataGrid.RowHeadersWidth = 15;
            this.DbRoutesDataGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DbRoutesDataGrid.Size = new System.Drawing.Size(380, 274);
            this.DbRoutesDataGrid.TabIndex = 0;
            // 
            // DbLogTab
            // 
            this.DbLogTab.Controls.Add(this.DbLogText);
            this.DbLogTab.Controls.Add(this.DbClearLog);
            this.DbLogTab.Location = new System.Drawing.Point(4, 22);
            this.DbLogTab.Name = "DbLogTab";
            this.DbLogTab.Size = new System.Drawing.Size(380, 274);
            this.DbLogTab.TabIndex = 4;
            this.DbLogTab.Text = "История";
            this.DbLogTab.UseVisualStyleBackColor = true;
            // 
            // DbLogText
            // 
            this.DbLogText.Location = new System.Drawing.Point(0, 0);
            this.DbLogText.MaxLength = 10000;
            this.DbLogText.Name = "DbLogText";
            this.DbLogText.Size = new System.Drawing.Size(380, 235);
            this.DbLogText.TabIndex = 1;
            this.DbLogText.Text = "";
            this.DbLogText.WordWrap = false;
            // 
            // DbClearLog
            // 
            this.DbClearLog.AutoSize = true;
            this.DbClearLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbClearLog.Location = new System.Drawing.Point(150, 240);
            this.DbClearLog.Name = "DbClearLog";
            this.DbClearLog.Size = new System.Drawing.Size(80, 26);
            this.DbClearLog.TabIndex = 0;
            this.DbClearLog.Text = "Очистить";
            this.DbClearLog.UseVisualStyleBackColor = true;
            this.DbClearLog.Click += new System.EventHandler(this.DbClearLog_Click);
            // 
            // DbShowResultTimer
            // 
            this.DbShowResultTimer.Interval = 2500;
            this.DbShowResultTimer.Tick += new System.EventHandler(this.DbShowResultTimer_Tick);
            // 
            // DbActionResult
            // 
            this.DbActionResult.BackColor = System.Drawing.SystemColors.Window;
            this.DbActionResult.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DbActionSuccessLabel,
            this.DbActionErrorLabel});
            this.DbActionResult.Location = new System.Drawing.Point(0, 297);
            this.DbActionResult.Name = "DbActionResult";
            this.DbActionResult.Size = new System.Drawing.Size(384, 22);
            this.DbActionResult.SizingGrip = false;
            this.DbActionResult.TabIndex = 9;
            // 
            // DbActionSuccessLabel
            // 
            this.DbActionSuccessLabel.Name = "DbActionSuccessLabel";
            this.DbActionSuccessLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // DbActionErrorLabel
            // 
            this.DbActionErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.DbActionErrorLabel.Name = "DbActionErrorLabel";
            this.DbActionErrorLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // DbRoutesColumn1
            // 
            this.DbRoutesColumn1.HeaderText = "Начало";
            this.DbRoutesColumn1.MaxInputLength = 25;
            this.DbRoutesColumn1.Name = "DbRoutesColumn1";
            this.DbRoutesColumn1.ReadOnly = true;
            this.DbRoutesColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DbRoutesColumn1.Width = 107;
            // 
            // DbRoutesColumn2
            // 
            this.DbRoutesColumn2.HeaderText = "Конец";
            this.DbRoutesColumn2.MaxInputLength = 25;
            this.DbRoutesColumn2.Name = "DbRoutesColumn2";
            this.DbRoutesColumn2.ReadOnly = true;
            this.DbRoutesColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DbRoutesColumn2.Width = 106;
            // 
            // DbRoutesColumn3
            // 
            this.DbRoutesColumn3.HeaderText = "L";
            this.DbRoutesColumn3.MaxInputLength = 6;
            this.DbRoutesColumn3.Name = "DbRoutesColumn3";
            this.DbRoutesColumn3.ReadOnly = true;
            this.DbRoutesColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DbRoutesColumn3.Width = 49;
            // 
            // DbRoutesColumn4
            // 
            this.DbRoutesColumn4.HeaderText = "T";
            this.DbRoutesColumn4.MaxInputLength = 6;
            this.DbRoutesColumn4.Name = "DbRoutesColumn4";
            this.DbRoutesColumn4.ReadOnly = true;
            this.DbRoutesColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DbRoutesColumn4.Width = 49;
            // 
            // DbRoutesColumn5
            // 
            this.DbRoutesColumn5.HeaderText = "C";
            this.DbRoutesColumn5.MaxInputLength = 6;
            this.DbRoutesColumn5.Name = "DbRoutesColumn5";
            this.DbRoutesColumn5.ReadOnly = true;
            this.DbRoutesColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DbRoutesColumn5.Width = 49;
            // 
            // DbForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(384, 319);
            this.Controls.Add(this.DbActionResult);
            this.Controls.Add(this.DbTabs);
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DbForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "База данных";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DbForm_FormClosed);
            this.Load += new System.EventHandler(this.DbForm_Load);
            this.DbTabs.ResumeLayout(false);
            this.DbAddTab.ResumeLayout(false);
            this.DbAddTab.PerformLayout();
            this.DbAddRouteOptions.ResumeLayout(false);
            this.DbAddRouteOptions.PerformLayout();
            this.DbAddBox.ResumeLayout(false);
            this.DbAddBox.PerformLayout();
            this.DbDeleteTab.ResumeLayout(false);
            this.DbDeleteTab.PerformLayout();
            this.DbDeleteBox.ResumeLayout(false);
            this.DbDeleteBox.PerformLayout();
            this.DbRoutesTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DbRoutesDataGrid)).EndInit();
            this.DbLogTab.ResumeLayout(false);
            this.DbLogTab.PerformLayout();
            this.DbActionResult.ResumeLayout(false);
            this.DbActionResult.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddCityButton;
        private System.Windows.Forms.TextBox DbAddNewCityText;
        private System.Windows.Forms.TabControl DbTabs;
        private System.Windows.Forms.TabPage DbAddTab;
        private System.Windows.Forms.TabPage DbDeleteTab;
        private System.Windows.Forms.TextBox DbAddRouteLenght;
        private System.Windows.Forms.GroupBox DbAddBox;
        private System.Windows.Forms.RadioButton DbAddCity;
        private System.Windows.Forms.RadioButton DbAddRoute;
        private System.Windows.Forms.Label DbAddNewCityLabel;
        private System.Windows.Forms.GroupBox DbDeleteBox;
        private System.Windows.Forms.Button DbDeleteButton;
        private System.Windows.Forms.RadioButton DbDeleteRoute;
        private System.Windows.Forms.RadioButton DbDeleteCity;
        private System.Windows.Forms.Label DbAddRouteEndLabel;
        private System.Windows.Forms.Label DbAddRouteLenghtLabel;
        private System.Windows.Forms.Label DbDeleteLabel;
        private System.Windows.Forms.ComboBox DbDeleteCitySelector;
        private System.Windows.Forms.ComboBox DbDeleteRouteSelector;
        private System.Windows.Forms.Timer DbShowResultTimer;
        private System.Windows.Forms.ComboBox DbAddRouteStartSelector;
        private System.Windows.Forms.ComboBox DbAddRouteEndSelector;
        private System.Windows.Forms.Label DbDeleteWarningLabel;
        private System.Windows.Forms.Label DbAddRouteCostLabel;
        private System.Windows.Forms.Label DbAddRouteTimeLabel;
        private System.Windows.Forms.TextBox DbAddRouteCost;
        private System.Windows.Forms.TextBox DbAddRouteTime;
        private System.Windows.Forms.Panel DbAddRouteOptions;
        private System.Windows.Forms.Label DbAddRouteStartLabel;
        private System.Windows.Forms.StatusStrip DbActionResult;
        private System.Windows.Forms.ToolStripStatusLabel DbActionSuccessLabel;
        private System.Windows.Forms.TabPage DbCitiesTab;
        private System.Windows.Forms.TabPage DbRoutesTab;
        private System.Windows.Forms.DataGridView DbRoutesDataGrid;
        private System.Windows.Forms.TabPage DbLogTab;
        private System.Windows.Forms.RichTextBox DbLogText;
        private System.Windows.Forms.Button DbClearLog;
        private System.Windows.Forms.ToolStripStatusLabel DbActionErrorLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn DbRoutesColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn DbRoutesColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn DbRoutesColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn DbRoutesColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn DbRoutesColumn1;
    }
}