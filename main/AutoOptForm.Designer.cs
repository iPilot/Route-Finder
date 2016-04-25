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
            this.AutoOptMaxSpeed = new System.Windows.Forms.TextBox();
            this.AutoOptSetButton = new System.Windows.Forms.Button();
            this.AutoOptFuelSpend = new System.Windows.Forms.TextBox();
            this.AutoOptMaxSpeedLabel = new System.Windows.Forms.Label();
            this.AutoOptFuelSpendLabel = new System.Windows.Forms.Label();
            this.AutoOptErrorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AutoOptMaxSpeed
            // 
            this.AutoOptMaxSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AutoOptMaxSpeed.Location = new System.Drawing.Point(20, 35);
            this.AutoOptMaxSpeed.Name = "AutoOptMaxSpeed";
            this.AutoOptMaxSpeed.Size = new System.Drawing.Size(100, 22);
            this.AutoOptMaxSpeed.TabIndex = 0;
            // 
            // AutoOptSetButton
            // 
            this.AutoOptSetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AutoOptSetButton.Location = new System.Drawing.Point(91, 73);
            this.AutoOptSetButton.Name = "AutoOptSetButton";
            this.AutoOptSetButton.Size = new System.Drawing.Size(100, 30);
            this.AutoOptSetButton.TabIndex = 1;
            this.AutoOptSetButton.Text = "Установить";
            this.AutoOptSetButton.UseVisualStyleBackColor = true;
            this.AutoOptSetButton.Click += new System.EventHandler(this.AutoOptSetButton_Click);
            // 
            // AutoOptFuelSpend
            // 
            this.AutoOptFuelSpend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AutoOptFuelSpend.Location = new System.Drawing.Point(162, 35);
            this.AutoOptFuelSpend.Name = "AutoOptFuelSpend";
            this.AutoOptFuelSpend.Size = new System.Drawing.Size(100, 22);
            this.AutoOptFuelSpend.TabIndex = 2;
            // 
            // AutoOptMaxSpeedLabel
            // 
            this.AutoOptMaxSpeedLabel.AutoSize = true;
            this.AutoOptMaxSpeedLabel.Location = new System.Drawing.Point(17, 19);
            this.AutoOptMaxSpeedLabel.Name = "AutoOptMaxSpeedLabel";
            this.AutoOptMaxSpeedLabel.Size = new System.Drawing.Size(87, 13);
            this.AutoOptMaxSpeedLabel.TabIndex = 3;
            this.AutoOptMaxSpeedLabel.Text = "Макс. скорость";
            // 
            // AutoOptFuelSpendLabel
            // 
            this.AutoOptFuelSpendLabel.AutoSize = true;
            this.AutoOptFuelSpendLabel.Location = new System.Drawing.Point(159, 19);
            this.AutoOptFuelSpendLabel.Name = "AutoOptFuelSpendLabel";
            this.AutoOptFuelSpendLabel.Size = new System.Drawing.Size(87, 13);
            this.AutoOptFuelSpendLabel.TabIndex = 4;
            this.AutoOptFuelSpendLabel.Text = "Расход топлива";
            // 
            // AutoOptErrorLabel
            // 
            this.AutoOptErrorLabel.AutoSize = true;
            this.AutoOptErrorLabel.Location = new System.Drawing.Point(84, 110);
            this.AutoOptErrorLabel.Name = "AutoOptErrorLabel";
            this.AutoOptErrorLabel.Size = new System.Drawing.Size(0, 13);
            this.AutoOptErrorLabel.TabIndex = 5;
            // 
            // AutoOptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(284, 141);
            this.Controls.Add(this.AutoOptErrorLabel);
            this.Controls.Add(this.AutoOptFuelSpendLabel);
            this.Controls.Add(this.AutoOptMaxSpeedLabel);
            this.Controls.Add(this.AutoOptFuelSpend);
            this.Controls.Add(this.AutoOptSetButton);
            this.Controls.Add(this.AutoOptMaxSpeed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AutoOptForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Параметры автомобиля";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AutoOptForm_FormClosed);
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
    }
}