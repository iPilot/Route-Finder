using System;
using System.Drawing;
using System.Windows.Forms;

namespace main
{
    public partial class AutoOptForm : Form
    {
        MainForm FMain;

        public AutoOptForm(MainForm _main)
        {
            FMain = _main;
            InitializeComponent();
            if (FMain.YourAuto != null)
            {
                AutoOptMaxSpeed.Text = FMain.YourAuto.MaxSpeed.ToString();
                AutoOptFuelSpend.Text = FMain.YourAuto.FuelSpend.ToString();
            }
        }

        private void AutoOptForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FMain.Enabled = true;
            FMain.refresh();
        }

        private void AutoOptSetButton_Click(object sender, EventArgs e)
        {
            try
            {
                int s = int.Parse(AutoOptMaxSpeed.Text);
                int f = int.Parse(AutoOptFuelSpend.Text);
                if (FMain.YourAuto == null)
                    FMain.YourAuto = new RouteFinderLibrary.AutoParams(s, f);
                else
                {
                    FMain.YourAuto.MaxSpeed = s;
                    FMain.YourAuto.FuelSpend = f;
                }
                ShowResult("Параметры установлены", true);
            }
            catch (FormatException)
            {
                if (string.IsNullOrEmpty(AutoOptMaxSpeed.Text) || string.IsNullOrEmpty(AutoOptFuelSpend.Text))
                {
                    ShowResult("Введите все параметры", false);
                }
                else
                {
                    ShowResult("Параметры должны быть целыми числами", false);
                }
            }
            catch (ArgumentException exp)
            {
                ShowResult(exp.Message, false);
            }
        }

        private void ShowResult(string message, bool ok)
        {
            AutoOptErrorLabel.Text = message;
            AutoOptErrorLabel.Location = new Point((Width - 18 - AutoOptErrorLabel.Width) / 2, AutoOptErrorLabel.Location.Y);
            if (ok)
                AutoOptErrorLabel.ForeColor = Color.Black;
            else
                AutoOptErrorLabel.ForeColor = Color.Red;
        }
    }
}
