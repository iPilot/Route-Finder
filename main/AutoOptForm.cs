using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace main
{
    public partial class AutoOptForm : Form
    {
        MainForm FMain;

        string[] ErrorMessages = { "Параметры установлены",
                                   "Введите все параметры",
                                   "Параметры должны быть целыми числами" };
        enum ErrorCodes
        {
            OK,
            EMPTY_FIELDS,
            INVALID_VALUES,
        }

        public AutoOptForm(MainForm _main)
        {
            FMain = _main;
            InitializeComponent();
            if (FMain.YourAuto != null)
            {
                AutoOptMaxSpeed.Text = FMain.YourAuto.MaxSpeed.ToString(CultureInfo.InvariantCulture);
                AutoOptFuelSpend.Text = FMain.YourAuto.FuelSpend.ToString(CultureInfo.InvariantCulture);
            }
            RouteCount.Value = FMain.RouteCount;
            RouteDispersionText.Text = (FMain.Disp * 100).ToString(CultureInfo.InvariantCulture);
        }

        private void AutoOptForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FMain.Enabled = true;
        }

        private void AutoOptSetButton_Click(object sender, EventArgs e)
        {
            try
            {
                int s = int.Parse(AutoOptMaxSpeed.Text);
                double f = double.Parse(AutoOptFuelSpend.Text, CultureInfo.InvariantCulture);
                double dsp = double.Parse(RouteDispersionText.Text.TrimEnd('%'), CultureInfo.InvariantCulture);
                if (FMain.YourAuto == null)
                    FMain.YourAuto = new RouteFinderLibrary.AutoParams(s, f);
                else
                {
                    FMain.YourAuto.MaxSpeed = s;
                    FMain.YourAuto.FuelSpend = f;
                }
                FMain.RouteCount = (int)RouteCount.Value;
                FMain.Disp = dsp / 100.0;
                ShowResult(ErrorMessages[(int)ErrorCodes.OK], true);
            }
            catch (FormatException)
            {
                if (string.IsNullOrEmpty(AutoOptMaxSpeed.Text) || string.IsNullOrEmpty(AutoOptFuelSpend.Text))
                {
                    ShowResult(ErrorMessages[(int)ErrorCodes.EMPTY_FIELDS], false);
                }
                else
                {
                    ShowResult(ErrorMessages[(int)ErrorCodes.INVALID_VALUES], false);
                }
            }
            catch (ArgumentException exp)
            {
                ShowResult(exp.Message, false);
            }
        }

        private void ShowResult(string message, bool ok)
        {
            AutoOptErrorLabel.Show();
            AutoOptErrorLabel.Text = message;
            AutoOptErrorLabel.Location = new Point((Width - 18 - AutoOptErrorLabel.Width) / 2, AutoOptErrorLabel.Location.Y);
            if (ok)
                AutoOptErrorLabel.ForeColor = Color.Black;
            else
                AutoOptErrorLabel.ForeColor = Color.Red;
            AutoOptErrorShowingTimer.Start();
        }

        private void AutoOptErrorShowingTimer_Tick(object sender, EventArgs e)
        {
            AutoOptErrorLabel.Hide();
            AutoOptErrorShowingTimer.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AutoOptForm_Load(object sender, EventArgs e)
        {
            OptionFormTooltips.SetToolTip(RouteDispertionLabel, "Допустимое отклонение параметра поиска для альтернативных маршрутов (от минимального)");
            OptionFormTooltips.SetToolTip(AutoOptFuelSpendLabel, "Количество топлива, потребляемое автомобилем для прохода 100 километров");
            OptionFormTooltips.SetToolTip(AutoOptMaxSpeedLabel, "Максимальная или средняя скорость автомобиля");
            OptionFormTooltips.SetToolTip(RouteCountLabel, "Количество вариантов пути (от 1 до 7)");
        }
    }
}
