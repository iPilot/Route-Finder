namespace main
{
    partial class AutoOptForm
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
            this.AutoOptMaxSpeed = new System.Windows.Forms.TextBox();
            this.AutoOptSetButton = new System.Windows.Forms.Button();
            this.AutoOptFuelSpend = new System.Windows.Forms.TextBox();
            this.AutoOptMaxSpeedLabel = new System.Windows.Forms.Label();
            this.AutoOptFuelSpendLabel = new System.Windows.Forms.Label();
            this.AutoOptErrorLabel = new System.Windows.Forms.Label();
            this.AutoOptErrorShowingTimer = new System.Windows.Forms.Timer(this.components);
            this.OptionExitButton = new System.Windows.Forms.Button();
            this.RouteCount = new System.Windows.Forms.NumericUpDown();
            this.AutoParamsGroup = new System.Windows.Forms.GroupBox();
            this.SearchParamsGroup = new System.Windows.Forms.GroupBox();
            this.RouteDispersionText = new System.Windows.Forms.TextBox();
            this.RouteDispertionLabel = new System.Windows.Forms.Label();
            this.RouteCountLabel = new System.Windows.Forms.Label();
            this.OptionFormTooltips = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.RouteCount)).BeginInit();
            this.AutoParamsGroup.SuspendLayout();
            this.SearchParamsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutoOptMaxSpeed
            // 
            this.AutoOptMaxSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AutoOptMaxSpeed.Location = new System.Drawing.Point(151, 62);
            this.AutoOptMaxSpeed.Name = "AutoOptMaxSpeed";
            this.AutoOptMaxSpeed.Size = new System.Drawing.Size(68, 24);
            this.AutoOptMaxSpeed.TabIndex = 1;
            this.AutoOptMaxSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // AutoOptSetButton
            // 
            this.AutoOptSetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AutoOptSetButton.Location = new System.Drawing.Point(22, 260);
            this.AutoOptSetButton.Name = "AutoOptSetButton";
            this.AutoOptSetButton.Size = new System.Drawing.Size(100, 30);
            this.AutoOptSetButton.TabIndex = 4;
            this.AutoOptSetButton.Text = "Установить";
            this.AutoOptSetButton.UseVisualStyleBackColor = true;
            this.AutoOptSetButton.Click += new System.EventHandler(this.AutoOptSetButton_Click);
            // 
            // AutoOptFuelSpend
            // 
            this.AutoOptFuelSpend.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AutoOptFuelSpend.Location = new System.Drawing.Point(151, 20);
            this.AutoOptFuelSpend.Name = "AutoOptFuelSpend";
            this.AutoOptFuelSpend.Size = new System.Drawing.Size(68, 24);
            this.AutoOptFuelSpend.TabIndex = 0;
            this.AutoOptFuelSpend.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // AutoOptMaxSpeedLabel
            // 
            this.AutoOptMaxSpeedLabel.AutoSize = true;
            this.AutoOptMaxSpeedLabel.Location = new System.Drawing.Point(6, 67);
            this.AutoOptMaxSpeedLabel.Name = "AutoOptMaxSpeedLabel";
            this.AutoOptMaxSpeedLabel.Size = new System.Drawing.Size(58, 13);
            this.AutoOptMaxSpeedLabel.TabIndex = 3;
            this.AutoOptMaxSpeedLabel.Text = "Скорость:";
            // 
            // AutoOptFuelSpendLabel
            // 
            this.AutoOptFuelSpendLabel.AutoSize = true;
            this.AutoOptFuelSpendLabel.Location = new System.Drawing.Point(6, 26);
            this.AutoOptFuelSpendLabel.Name = "AutoOptFuelSpendLabel";
            this.AutoOptFuelSpendLabel.Size = new System.Drawing.Size(90, 13);
            this.AutoOptFuelSpendLabel.TabIndex = 4;
            this.AutoOptFuelSpendLabel.Text = "Расход топлива:";
            // 
            // AutoOptErrorLabel
            // 
            this.AutoOptErrorLabel.AutoSize = true;
            this.AutoOptErrorLabel.Location = new System.Drawing.Point(135, 230);
            this.AutoOptErrorLabel.Name = "AutoOptErrorLabel";
            this.AutoOptErrorLabel.Size = new System.Drawing.Size(0, 13);
            this.AutoOptErrorLabel.TabIndex = 5;
            // 
            // AutoOptErrorShowingTimer
            // 
            this.AutoOptErrorShowingTimer.Interval = 2500;
            this.AutoOptErrorShowingTimer.Tick += new System.EventHandler(this.AutoOptErrorShowingTimer_Tick);
            // 
            // OptionExitButton
            // 
            this.OptionExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OptionExitButton.Location = new System.Drawing.Point(151, 260);
            this.OptionExitButton.Name = "OptionExitButton";
            this.OptionExitButton.Size = new System.Drawing.Size(100, 30);
            this.OptionExitButton.TabIndex = 5;
            this.OptionExitButton.Text = "Выход";
            this.OptionExitButton.UseVisualStyleBackColor = true;
            this.OptionExitButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // RouteCount
            // 
            this.RouteCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RouteCount.Location = new System.Drawing.Point(151, 21);
            this.RouteCount.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.RouteCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RouteCount.Name = "RouteCount";
            this.RouteCount.Size = new System.Drawing.Size(68, 24);
            this.RouteCount.TabIndex = 2;
            this.RouteCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.RouteCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // AutoParamsGroup
            // 
            this.AutoParamsGroup.Controls.Add(this.AutoOptFuelSpend);
            this.AutoParamsGroup.Controls.Add(this.AutoOptMaxSpeedLabel);
            this.AutoParamsGroup.Controls.Add(this.AutoOptFuelSpendLabel);
            this.AutoParamsGroup.Controls.Add(this.AutoOptMaxSpeed);
            this.AutoParamsGroup.Location = new System.Drawing.Point(22, 12);
            this.AutoParamsGroup.Name = "AutoParamsGroup";
            this.AutoParamsGroup.Size = new System.Drawing.Size(229, 105);
            this.AutoParamsGroup.TabIndex = 7;
            this.AutoParamsGroup.TabStop = false;
            this.AutoParamsGroup.Text = "Параметры автомобиля";
            // 
            // SearchParamsGroup
            // 
            this.SearchParamsGroup.Controls.Add(this.RouteDispersionText);
            this.SearchParamsGroup.Controls.Add(this.RouteDispertionLabel);
            this.SearchParamsGroup.Controls.Add(this.RouteCountLabel);
            this.SearchParamsGroup.Controls.Add(this.RouteCount);
            this.SearchParamsGroup.Location = new System.Drawing.Point(22, 123);
            this.SearchParamsGroup.Name = "SearchParamsGroup";
            this.SearchParamsGroup.Size = new System.Drawing.Size(229, 100);
            this.SearchParamsGroup.TabIndex = 8;
            this.SearchParamsGroup.TabStop = false;
            this.SearchParamsGroup.Text = "Параметры поиска";
            // 
            // RouteDispersionText
            // 
            this.RouteDispersionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RouteDispersionText.Location = new System.Drawing.Point(151, 62);
            this.RouteDispersionText.Name = "RouteDispersionText";
            this.RouteDispersionText.Size = new System.Drawing.Size(68, 24);
            this.RouteDispersionText.TabIndex = 3;
            this.RouteDispersionText.Text = "20%";
            this.RouteDispersionText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.RouteDispersionText.TextChanged += new System.EventHandler(this.RouteDispersionText_TextChanged);
            // 
            // RouteDispertionLabel
            // 
            this.RouteDispertionLabel.AutoSize = true;
            this.RouteDispertionLabel.Location = new System.Drawing.Point(6, 69);
            this.RouteDispertionLabel.Name = "RouteDispertionLabel";
            this.RouteDispertionLabel.Size = new System.Drawing.Size(135, 13);
            this.RouteDispertionLabel.TabIndex = 8;
            this.RouteDispertionLabel.Text = "Допустимое отклонение:";
            // 
            // RouteCountLabel
            // 
            this.RouteCountLabel.AutoSize = true;
            this.RouteCountLabel.Location = new System.Drawing.Point(6, 27);
            this.RouteCountLabel.Name = "RouteCountLabel";
            this.RouteCountLabel.Size = new System.Drawing.Size(128, 13);
            this.RouteCountLabel.TabIndex = 7;
            this.RouteCountLabel.Text = "Количество маршрутов:";
            // 
            // AutoOptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(273, 301);
            this.Controls.Add(this.SearchParamsGroup);
            this.Controls.Add(this.AutoParamsGroup);
            this.Controls.Add(this.OptionExitButton);
            this.Controls.Add(this.AutoOptErrorLabel);
            this.Controls.Add(this.AutoOptSetButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AutoOptForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Настройки";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AutoOptForm_FormClosed);
            this.Load += new System.EventHandler(this.AutoOptForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RouteCount)).EndInit();
            this.AutoParamsGroup.ResumeLayout(false);
            this.AutoParamsGroup.PerformLayout();
            this.SearchParamsGroup.ResumeLayout(false);
            this.SearchParamsGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AutoOptMaxSpeed;
        private System.Windows.Forms.Button AutoOptSetButton;
        private System.Windows.Forms.TextBox AutoOptFuelSpend;
        private System.Windows.Forms.Label AutoOptMaxSpeedLabel;
        private System.Windows.Forms.Label AutoOptFuelSpendLabel;
        private System.Windows.Forms.Label AutoOptErrorLabel;
        private System.Windows.Forms.Timer AutoOptErrorShowingTimer;
        private System.Windows.Forms.Button OptionExitButton;
        private System.Windows.Forms.NumericUpDown RouteCount;
        private System.Windows.Forms.GroupBox AutoParamsGroup;
        private System.Windows.Forms.GroupBox SearchParamsGroup;
        private System.Windows.Forms.TextBox RouteDispersionText;
        private System.Windows.Forms.Label RouteDispertionLabel;
        private System.Windows.Forms.Label RouteCountLabel;
        private System.Windows.Forms.ToolTip OptionFormTooltips;
    }
}