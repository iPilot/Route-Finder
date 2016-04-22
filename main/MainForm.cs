using RouteFinderLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace main
{
    public partial class MainForm : Form
    {
        DbForm DataBaseForm;
        public RFDatabase Data { get; }
        BindingSource StDataSource, EndDataSource;
        List<string> CitiesList;

        public MainForm()
        {
            InitializeComponent();
            Data = new RFDatabase();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Top = (Screen.PrimaryScreen.Bounds.Height - Height) / 2;
            Left = (Screen.PrimaryScreen.Bounds.Width - Width) / 2;
        }

        private void MenuItemDb_Click(object sender, EventArgs e)
        {
            DataBaseForm = new DbForm(this);
            //if (!DataBaseForm.Visible)
            //{
            //    Enabled = false;
            //    DataBaseForm.Enabled = true;
            //    DataBaseForm.Location = new Point(Location.X + (Width - DataBaseForm.Width) / 2, Location.Y + (Height - DataBaseForm.Height) / 2);
            //    DataBaseForm.Show();
            //}
        }

        public void refresh()
        {
            StDataSource.ResetBindings(false);
            EndDataSource.ResetBindings(false);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            bool FAIL = false;
            OpenFileDialog newpath = new OpenFileDialog();
            newpath.Multiselect = false;
            newpath.Title = "Укажите файл с базой данных...";
            newpath.Filter = "Файлы структуры путей|*.dbp|Все файлы|*.*";
            int lt = Assembly.GetExecutingAssembly().Location.LastIndexOf('\\');
            newpath.InitialDirectory = Assembly.GetExecutingAssembly().Location.Substring(0, lt);
            DialogResult newfile;
            string path = Path.Combine(newpath.InitialDirectory, Data.DbDefaultFileName);
            while (Data.DbLoad(path) != DBActionResult.OK)
            {
                newfile = MessageBox.Show("Файл базы данных не найден или поврежден.\nОткрыть другой файл?", "Ошибка",  MessageBoxButtons.OKCancel);
                if (newfile == DialogResult.Cancel)
                {
                    FAIL = true;
                    break;
                }
                else
                {
                    newfile = newpath.ShowDialog();
                    if (newfile == DialogResult.Cancel)
                    {
                        FAIL = true;
                        break;
                    }
                    else 
                        if (newfile == DialogResult.OK)
                            path = newpath.FileName;
                }
            }
            if (FAIL) Application.Exit();
            else
            {
                CitiesList = Data.GetCitiesList();
                StDataSource = new BindingSource(CitiesList, "");
                EndDataSource = new BindingSource(CitiesList, "");
                StartPointBox.DataSource = StDataSource;
                EndPointBox.DataSource = EndDataSource;
                RouteCriteriaSelector.SelectedIndex = 0;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //if (StartPointBox.SelectedIndex >= 0 && EndPointBox.SelectedIndex >= 0 && RouteCriteriaSelector.SelectedIndex >= 0)
              //  FindRoute(StartPointBox.SelectedItem.ToString(), EndPointBox.SelectedItem.ToString(), RouteCriteriaSelector.SelectedIndex);
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

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }  
}
