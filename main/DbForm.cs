using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class DbForm : Form
    {
        MainForm parent;
        System.IO.StreamReader DbLogReader;
        System.IO.StreamWriter DbLogWriter;
        DateTime time;

        public DbForm(MainForm _parent)
        {
            InitializeComponent();
            parent = _parent;
            parent.Enabled = false;
            parent.Visible = false;
        }

        private void DbRefresh()
        {
            DbAddRouteEndSelector.Items.Clear();
            DbAddRouteStartSelector.Items.Clear();
            DbDeleteCitySelector.Items.Clear();
            DbDeleteRouteSelector.Items.Clear();
            DbRoutesDataGrid.Rows.Clear();
            for (int i = 0; i < parent.cities.Count; i++)
            { 
                DbAddRouteStartSelector.Items.Add(parent.cities[i]);
                DbAddRouteEndSelector.Items.Add(parent.cities[i]);
                DbDeleteCitySelector.Items.Add(parent.cities[i]);
            }
            for (int i = 0; i < parent.routes.Count; i++)
            {
                DbDeleteRouteSelector.Items.Add(parent.routes[i].FirstCity + " - " + parent.routes[i].SecondCity + " (" + parent.routes[i].Options[0] + "," + parent.routes[i].Options[1] + "," + parent.routes[i].Options[2] + ")");
                DbRoutesDataGrid.Rows.Add(parent.routes[i].FirstCity, parent.routes[i].SecondCity, parent.routes[i].Options[0], parent.routes[i].Options[1], parent.routes[i].Options[2]);
            }
            DbAddRouteEndSelector.SelectedIndex = -1;
            DbAddRouteStartSelector.SelectedIndex = -1;
            DbDeleteCitySelector.SelectedIndex = -1;
            DbDeleteRouteSelector.SelectedIndex = -1;
            DbAddRouteEndSelector.ResetText();
            DbAddRouteStartSelector.ResetText();
            DbDeleteCitySelector.ResetText();
            DbDeleteRouteSelector.ResetText();
        }

        private void DbForm_Load(object sender, EventArgs e)
        {
            DbLogReader = new System.IO.StreamReader("routefinder.log");
            int k = 0;
            bool logoverflow = false;
            int loglenght = 50;
            string[] log = new string[loglenght];
            while (!DbLogReader.EndOfStream)
            {
                log[k++] = DbLogReader.ReadLine();
                if (k == loglenght)
                {
                    k %= loglenght;
                    logoverflow = true;
                }
            }
            if (logoverflow)
                for(int i = k - 1; i < loglenght; i++)
                {
                    log_add_row(log[i]);
                }
            for(int i = 0; i < k; i++)
            {
                log_add_row(log[i]);
            }
            DbLogReader.Close();
            DbLogWriter = new System.IO.StreamWriter("routefinder.log", true);
            DbRefresh();
            //time = DateTime.Now;
        }

        private void log_add_row(string row)
        {
            string[] tmp = row.Split(',');
            try
            {
                DbLogText.AppendText(tmp[0] + "/" + tmp[1] + "/" + tmp[2] + " " + tmp[3] + ":" + tmp[4] + ":" + tmp[5] + " ");
                if (tmp[6] == "add")
                {
                    DbLogText.AppendText("Добавлен ");
                }
                if (tmp[6] == "del")
                {
                    DbLogText.AppendText("Удален ");
                }
                if (tmp[7] == "c")
                {
                    DbLogText.AppendText("город: " + tmp[8] + "\n");
                }
                if (tmp[7] == "r")
                {
                    DbLogText.AppendText("маршрут: " + tmp[8] + " - " + tmp[9] + "(" + tmp[10] + "," + tmp[11] + "," + tmp[12] + ")\n");
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }

        private void DbAddRoute_CheckedChanged(object sender, EventArgs e)
        {
            if (DbAddRoute.Checked)
            {
                DbAddNewCityLabel.Hide();
                DbAddNewCityText.Hide();
                DbAddRouteOptions.Show();
            }
            else
            {
                DbAddNewCityLabel.Show();
                DbAddNewCityText.Show();
                DbAddRouteOptions.Hide();
            }
        }

        private void DbForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DbLogWriter.Close();
            parent.Visible = true;
            parent.Enabled = true;
            parent.Focus();
            parent.BuildWorkGraph();
        }

        private void DbDeleteRoute_CheckedChanged(object sender, EventArgs e)
        {
            if (DbDeleteRoute.Checked)
            {
                DbDeleteLabel.Text = "Выберите дорогу:";
                DbDeleteRouteSelector.Visible = true;
                DbDeleteCitySelector.Visible = false;
                DbDeleteWarningLabel.Visible = false;
            }
            else
            {
                DbDeleteLabel.Text = "Выберите город:";
                DbDeleteRouteSelector.Visible = false;
                DbDeleteCitySelector.Visible = true;
                DbDeleteWarningLabel.Visible = true;
            }
        }

        private void AddCityButton_Click(object sender, EventArgs e)
        {
            if (DbAddCity.Checked)
            {
                int r = parent.AddCity(DbAddNewCityText.Text);
                switch (r)
                {
                    case 0:
                        {
                            string newcity = DbAddNewCityText.Text.Substring(0, 1).ToUpper() + DbAddNewCityText.Text.Substring(1).ToLower();
                            DbActionSuccessLabel.Text = "Добавлено: " + newcity;
                            DbAddRouteStartSelector.Items.Add(newcity);
                            DbAddRouteEndSelector.Items.Add(newcity);
                            DbDeleteCitySelector.Items.Add(newcity);
                            DbAddNewCityText.Clear();
                            time = DateTime.Now;
                            string tmp = time.Day + "," + time.Month + "," + time.Year + "," + time.Hour + "," + time.Minute + "," + time.Second + ",add,c," + newcity;
                            DbLogWriter.WriteLine(tmp);
                            log_add_row(tmp);
                            break;
                        }
                    case 1:
                        {
                            DbActionErrorLabel.Text = "Ошибка: город \"" + DbAddNewCityText.Text + "\" уже существует";
                            db_show_error();
                            break;
                        }
                    case 2:
                        {
                            DbActionErrorLabel.Text = "Ошибка: введите название города";
                            db_show_error();
                            break;
                        }
                    
                }
            }
            if (DbAddRoute.Checked)
            {
                if (DbAddRouteStartSelector.SelectedIndex >= 0 && DbAddRouteEndSelector.SelectedIndex >= 0)
                {
                    int r = parent.AddRoute(DbAddRouteStartSelector.SelectedItem.ToString(), DbAddRouteEndSelector.SelectedItem.ToString(), DbAddRouteLenght.Text, DbAddRouteTime.Text, DbAddRouteCost.Text);
                    switch (r)
                    {
                        case 0:
                            {
                                DbDeleteRouteSelector.Items.Add(parent.routes.Last().FirstCity + " - " + parent.routes.Last().SecondCity);
                                DbActionSuccessLabel.Text = "Добавлено: " + DbAddRouteStartSelector.SelectedItem.ToString() + " - " + DbAddRouteEndSelector.SelectedItem.ToString() + "(" + DbAddRouteLenght.Text + ", " + DbAddRouteTime.Text + ", " + DbAddRouteCost.Text + ")";
                                time = DateTime.Now;
                                string tmp = time.Day + "," + time.Month + "," + time.Year + "," + time.Hour + "," + time.Minute + "," + time.Second + ",add,r," + DbAddRouteStartSelector.SelectedItem.ToString() + "," + DbAddRouteEndSelector.SelectedItem.ToString() + "," + DbAddRouteLenght.Text + "," + DbAddRouteTime.Text + "," + DbAddRouteCost.Text;
                                DbLogWriter.WriteLine(tmp);
                                log_add_row(tmp);
                                DbRoutesDataGrid.Rows.Add(DbAddRouteStartSelector.SelectedItem.ToString(), DbAddRouteEndSelector.SelectedItem.ToString(), DbAddRouteLenght.Text, DbAddRouteTime.Text, DbAddRouteCost.Text);
                                DbAddRouteEndSelector.ResetText();
                                DbAddRouteEndSelector.SelectedIndex = -1;
                                DbAddRouteStartSelector.ResetText();
                                DbAddRouteStartSelector.SelectedIndex = -1;
                                DbAddRouteLenght.Clear();
                                DbAddRouteTime.Clear();
                                DbAddRouteCost.Clear();
                                break;
                            }
                        case 15:
                            {
                                DbActionErrorLabel.Text = "Ошибка: такая дорога уже существует";
                                db_show_error();
                                break;
                            }
                        case -1:
                            {
                                DbActionErrorLabel.Text = "Ошибка: параметры должны быть целыми числами";
                                db_show_error();
                                break;
                            }
                        default:
                            {
                                DbActionErrorLabel.Text = "Ошибка: введите ";
                                if ((r & 1) == 1) DbActionErrorLabel.Text += "расстояние; ";
                                if (((r >> 1) & 1) == 1) DbActionErrorLabel.Text += "время; ";
                                if (((r >> 2) & 1) == 1) DbActionErrorLabel.Text += "стоимость; ";
                                db_show_error();
                                break;
                            }
                    }
                }
                else
                {
                    DbActionErrorLabel.Text = "Ошибка: укажите начало и конец дороги";
                    db_show_error();
                }
            }
            //DbActionResultLabel.Visible = true;
            //DbShowResultTimer.Stop();
            //DbShowResultTimer.Start();
        }

        private void DbDeleteButton_Click(object sender, EventArgs e)
        {
            if (DbDeleteCity.Checked)
            {
                if (DbDeleteCitySelector.SelectedIndex >= 0)
                {
                    int sizebefore = parent.routes.Count;
                    parent.DeleteCity(DbDeleteCitySelector.SelectedItem.ToString());
                    int sizeafter = parent.routes.Count;
                    DbActionSuccessLabel.Text = "Удалено: " + DbDeleteCitySelector.SelectedItem.ToString() + " и " + (sizebefore - sizeafter) + " дорог";
                    time = DateTime.Now;
                    string tmp = time.Day + "," + time.Month + "," + time.Year + "," + time.Hour + "," + time.Minute + "," + time.Second + ",del,c," + DbDeleteCitySelector.SelectedItem.ToString();
                    DbLogWriter.WriteLine(tmp);
                    log_add_row(tmp);
                    DbDeleteCitySelector.ResetText();
                    DbDeleteCitySelector.SelectedIndex = -1;
                    DbRefresh();
                }
                else
                {
                    DbActionErrorLabel.Text = "Ошибка: выберите город";
                    db_show_error();
                }
            }
            if (DbDeleteRoute.Checked)
            {
                if (DbDeleteRouteSelector.SelectedIndex >= 0)
                {
                    string[] c = DbDeleteRouteSelector.SelectedItem.ToString().Split(' ');
                    string[] d = c[3].Substring(1, c[3].Length - 2).Split(',');
                    parent.DeleteRoute(c[0], c[2]);
                    time = DateTime.Now;
                    string tmp = time.Day + "," + time.Month + "," + time.Year + "," + time.Hour + "," + time.Minute + "," + time.Second + ",del,r," + c[0] + "," + c[2] + "," + d[0] + "," + d[1] + "," + d[2];
                    DbLogWriter.WriteLine(tmp);
                    log_add_row(tmp);
                    DbRoutesDataGrid.Rows.Remove(DbRoutesDataGrid.Rows[DbDeleteRouteSelector.SelectedIndex]);
                    DbDeleteRouteSelector.Items.Remove(DbDeleteRouteSelector.SelectedItem);
                    DbDeleteRouteSelector.ResetText();
                    DbDeleteRouteSelector.SelectedIndex = -1;
                }
                else
                {
                    DbActionErrorLabel.Text = "Ошибка: выберите дорогу";
                    db_show_error();
                }
            }
           // DbShowResultTimer.Stop();
            //DbShowResultTimer.Start();
        }

        private void DbShowResultTimer_Tick(object sender, EventArgs e)
        {
            DbActionSuccessLabel.Visible = true;
            DbActionErrorLabel.Visible = false;
            DbShowResultTimer.Enabled = false;
        }

        private void db_show_error()
        {
            DbActionSuccessLabel.Visible = false;
            DbActionErrorLabel.Visible = true;
            DbShowResultTimer.Stop();
            DbShowResultTimer.Start();
        }

        private void DbClearLog_Click(object sender, EventArgs e)
        {
            DbLogText.Clear();
            DbLogWriter.Close();
            DbActionSuccessLabel.Text = "История очищена";
            DbLogWriter = new System.IO.StreamWriter("routefinder.log", false);
        }
    }
}
