using RouteFinderLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace main
{
    public partial class MainForm : Form
    {
        DbForm DataBaseForm;
        SearchResultForm SearchResult;
        public RFDatabase Data { get; }
        public AutoParams YourAuto { get; set; }
        List<string> CitiesList;
        string ConfigFilePath;

        double d;
        int rc;

        public double Disp
        {
            get
            {
                return d;
            }
            set
            {
                if (value >= 0.0 && value <= 10.0)
                    d = value;
                else
                    throw new ArgumentException("Допустимый разброс должен быть от 0 до 1000%", "Disp");
            }
        }
        public int RouteCount
        {
            get
            {
                return rc;
            }
            set
            {
                if (value > 0 && value < 8)
                    rc = value;
                else
                    throw new ArgumentException("Количество должно быть от 1 до 7","Count");
            }
        }

        public MainForm()
        {
            InitializeComponent();
            Data = new RFDatabase();
            YourAuto = null;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            Top = (Screen.PrimaryScreen.Bounds.Height - Height) / 2;
            Left = (Screen.PrimaryScreen.Bounds.Width - Width) / 2;

            ConfigFilePath = Path.Combine(Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')), "routefinder.ini");
            StreamReader ConfigFile;

            try
            {
                int speed = 0, count = 3;
                double spend = 0.0, disp = 0.5;
                ConfigFile = new StreamReader(ConfigFilePath, Encoding.UTF8);
                string[] line;
                for (int i = 0; i < 4; i++)
                {
                    line = ConfigFile.ReadLine().Split('=');
                    switch (line[0].Trim(' '))
                    {
                        case "Speed":
                            {
                                if (!int.TryParse(line[1].Trim(' '), out speed)) speed = 0;
                                break;
                            }
                        case "Spend":
                            {
                                if (!double.TryParse(line[1].Trim(' '), NumberStyles.AllowThousands | NumberStyles.Float, CultureInfo.InvariantCulture, out spend)) spend = 0.0;
                                break;
                            }
                        case "Count":
                            {
                                if (!int.TryParse(line[1].Trim(' '), out count)) count = 3;
                                break;
                            }
                        case "Disp":
                            {
                                if (!double.TryParse(line[1].Trim(' '), NumberStyles.AllowThousands | NumberStyles.Float, CultureInfo.InvariantCulture, out disp)) disp = 0.5;
                                break;
                            }
                    }
                }
                ConfigFile.Close();
                if (count > 0 && count < 8) RouteCount = count;
                if (disp >= 0 && disp <= 10.0) Disp = disp;
                YourAuto = new AutoParams(speed, spend);
                ConfigFile.Close();
            }
            catch (FileNotFoundException)
            {
                Disp = 0.5;
                RouteCount = 3;
            }
            catch (ArgumentException)
            {
            }
        }
        private void MenuItemDb_Click(object sender, EventArgs e)
        {
            DataBaseForm = new DbForm(this);
            if (!DataBaseForm.Visible)
            {
                Enabled = false;
                DataBaseForm.Enabled = true;
                DataBaseForm.Location = new Point(Location.X + (Width - DataBaseForm.Width) / 2, Location.Y + (Height - DataBaseForm.Height) / 2);
                DataBaseForm.Show();
            }
        }
        public void refresh(bool changes)
        {
            if (changes)
            {
                CitiesList = Data.GetCitiesList();
                StartPointBox.DataSource = new BindingSource(CitiesList, "");
                EndPointBox.DataSource = new BindingSource(CitiesList, "");
            }
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            bool FAIL = false;
            OpenFileDialog newpath = new OpenFileDialog();
            newpath.Multiselect = false;
            newpath.Title = "Укажите файл с базой данных...";
            newpath.Filter = "Файлы структуры путей|*.dbp";
            int lt = Assembly.GetExecutingAssembly().Location.LastIndexOf('\\');
            newpath.InitialDirectory = Assembly.GetExecutingAssembly().Location.Substring(0, lt);
            DialogResult newfile;
            string path = Path.Combine(newpath.InitialDirectory, Data.DbDefaultFileName);
            while (Data.DbLoad(path) != RFLActionResult.OK)
            {
                newfile = MessageBox.Show("Файл базы данных не найден или поврежден.\nСоздать новый файл?", "Ошибка",  MessageBoxButtons.OKCancel);
                if (newfile == DialogResult.Cancel)
                {
                    FAIL = true;
                    break;
                }
                else 
                {
                    if (File.Exists(path)) File.Delete(path);
                    File.AppendAllText(path, "0\n0", Encoding.UTF8);
                }
            }
            if (FAIL) Application.Exit();
            else
            {
                refresh(true);
                RouteCriteriaSelector.SelectedIndex = 0;
            }
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            switch (CheckParams())
            {
                case RFLActionResult.OK:
                    {
                        SearchResult = new SearchResultForm(this, StartPointBox.SelectedItem.ToString(), EndPointBox.SelectedItem.ToString(), RouteCriteriaSelector.SelectedIndex, RouteCount, Disp);
                        if (!SearchResult.Visible)
                        {
                            Enabled = false;
                            SearchResult.Location = new Point(Location.X + (Width - SearchResult.Width) / 2, Location.Y + (Height - SearchResult.Height) / 2);
                            SearchResult.Show();
                        }
                        break;
                    }
                case RFLActionResult.START_CITY_EMPTY:
                    {
                        ShowError("Выберите начальный город");
                        break;
                    }
                case RFLActionResult.END_CITY_EMPTY:
                    {
                        ShowError("Выберите конечный город");
                        break;
                    }
                case RFLActionResult.AUTO_PARAMS_NOT_DEFINED:
                    {
                        //MenuItemSettings_Click(sender, e);
                        ShowError("Параметры автомобиля не заданы");
                        break;
                    }
                case RFLActionResult.ROUTE_CRITERIA_NOT_SELECTED:
                    {
                        ShowError("Параметр поиска не задан");
                        break;
                    }
                case RFLActionResult.DATABASE_IS_EMPTY:
                    {
                        ShowError("База данных не содержит дорог или городов");
                        break;
                    }
                case RFLActionResult.ROUTE_SOURCE_EQUALS_DEST:
                    {
                        ShowError("Начало и конец маршрута должны различаться");
                        break;
                    }
            }
        }
        private RFLActionResult CheckParams()
        {
            if (Data.GetCitiesList().Count == 0 || Data.GetRoutesList().Count == 0) return RFLActionResult.DATABASE_IS_EMPTY;
            else if (StartPointBox.SelectedItem == null) return RFLActionResult.START_CITY_EMPTY;
            else if (EndPointBox.SelectedItem == null) return RFLActionResult.END_CITY_EMPTY;
            else if (YourAuto == null) return RFLActionResult.AUTO_PARAMS_NOT_DEFINED;
            else if (RouteCriteriaSelector.SelectedIndex < 0) return RFLActionResult.ROUTE_CRITERIA_NOT_SELECTED;
            else if (StartPointBox.SelectedIndex == EndPointBox.SelectedIndex) return RFLActionResult.ROUTE_SOURCE_EQUALS_DEST;
            else return RFLActionResult.OK;
        }
        private void ShowError(string message)
        {
            MainFormErrorLabel.Text = message;
            MainFormErrorLabel.Location = new Point((Width - 18 - MainFormErrorLabel.Width) / 2, MainFormErrorLabel.Location.Y);
            MainFormErrorLabel.Show();
            MainFormErrorTimer.Start();
        }
        private void MenuItemSettings_Click(object sender, EventArgs e)
        {
            Enabled = false;
            AutoOptForm Auto = new AutoOptForm(this);
            if (!Auto.Visible)
            {
                Enabled = false;
                Auto.Enabled = true;
                Auto.Location = new Point(Location.X + (Width - Auto.Width) / 2, Location.Y + (Height - Auto.Height) / 2);
                Auto.Show();
            }
        }

        private void MainFormErrorTimer_Tick(object sender, EventArgs e)
        {
            MainFormErrorLabel.Hide();
            MainFormErrorTimer.Stop();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StreamWriter ConfigFile = new StreamWriter(ConfigFilePath, false, Encoding.UTF8);
            if (YourAuto != null)
            {
                ConfigFile.WriteLine("Speed = " + YourAuto.MaxSpeed.ToString(CultureInfo.InvariantCulture));
                ConfigFile.WriteLine("Spend = " + YourAuto.FuelSpend.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                ConfigFile.WriteLine("Speed = 0");
                ConfigFile.WriteLine("Spend = 0");
            }
            ConfigFile.WriteLine("Count = " + RouteCount.ToString(CultureInfo.InvariantCulture));
            ConfigFile.WriteLine("Disp = " + Disp.ToString(CultureInfo.InvariantCulture));
            ConfigFile.Close();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }  
}
