using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

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
            DbModCitySelector.Items.Clear();
            DbModRouteSelector.Items.Clear();
            DbCitiesList.Items.Clear();
            for (int i = 0; i < parent.cities.Count; i++)
            { 
                DbAddRouteStartSelector.Items.Add(parent.cities[i]);
                DbAddRouteEndSelector.Items.Add(parent.cities[i]);
                DbDeleteCitySelector.Items.Add(parent.cities[i]);
                DbCitiesList.Items.Add(parent.cities[i]);
                DbModCitySelector.Items.Add(parent.cities[i]);
            }
            //string tmp;
            for (int i = 0; i < parent.routes.Count; i++)
            {
                //tmp = String.Format("{0} - {1} ({2},{3},{4})", parent.routes[i].FirstCity, parent.routes[i].SecondCity, parent.routes[i].Options[0], parent.routes[i].Options[1], parent.routes[i].Options[2]);
                DbDeleteRouteSelector.Items.Add(parent.routes[i]);
                DbModRouteSelector.Items.Add(parent.routes[i]);
                DbRoutesDataGrid.Rows.Add(i+1, parent.routes[i].FirstCity, parent.routes[i].SecondCity, parent.routes[i].Options[0], parent.routes[i].Options[1], parent.routes[i].Options[2]);
            }
            DbAddRouteEndSelector.SelectedIndex = -1;
            DbAddRouteStartSelector.SelectedIndex = -1;
            DbDeleteCitySelector.SelectedIndex = -1;
            DbDeleteRouteSelector.SelectedIndex = -1;
            DbModCitySelector.SelectedIndex = -1;
            DbModRouteSelector.SelectedIndex = -1;
            DbAddRouteEndSelector.ResetText();
            DbAddRouteStartSelector.ResetText();
            DbDeleteCitySelector.ResetText();
            DbDeleteRouteSelector.ResetText();
            DbModCitySelector.ResetText();
            DbModRouteSelector.ResetText();
        }

        private void DbForm_Load(object sender, EventArgs e)
        {
            DbLogReader = new System.IO.StreamReader("routefinder.log");
            int k = 0;
            bool logoverflow = false;
            int loglength = 50;
            string[] log = new string[loglength];
            while (!DbLogReader.EndOfStream)
            {
                log[k++] = DbLogReader.ReadLine();
                if (k == loglength)
                {
                    k %= loglength;
                    logoverflow = true;
                }
            }
            if (logoverflow)
                for(int i = k - 1; i < loglength; i++)
                {
                    local_log_add_row(log[i]);
                }
            for(int i = 0; i < k; i++)
            {
                local_log_add_row(log[i]);
            }
            DbLogReader.Close();
            DbLogWriter = new System.IO.StreamWriter("routefinder.log", true);
            DbRefresh();
        }

        // удалено
        private void local_log_add_row(string row)
        {
            string[] tmp = row.Split(',');
            int[] time = new int[12];
            string result;
            try
            {
                for (int z = 0; z < 6; z++) time[z] = int.Parse(tmp[z]);
                string[] args = new string[6];
                for (int z = 0; z < 6; z++) args[z] = CRoute.TimeFormat(time[z], tmp[z]);
                result = String.Format("{0}/{1}/{2} {3}:{4}:{5} ", args); //CRoute.TimeFormat(time[0], tmp[0]), (time[1] > 9 ? tmp[1] : "0" + tmp[1]), tmp[2], (time[3] > 9 ? tmp[3] : "0" + tmp[3]), (time[4] > 9 ? tmp[4] : "0" + tmp[4]), (time[5] > 9 ? tmp[5] : "0" + tmp[5])));
                if (tmp[6] == "add")
                {
                    result += "Добавлен";
                }
                if (tmp[6] == "del")
                {
                   result += "Удален";
                }
                if (tmp[6] == "mod")
                {
                    result += "Изменен";
                }
                if (tmp[7] == "c")
                {
                    result += " город: ";
                    if (tmp[6] == "mod")
                            result += String.Format(" {0} на {1}\n", tmp[8], tmp[9]);
                    else
                           result += tmp[8] + "\n";
                }
                if (tmp[7] == "r")
                {
                    
                    if (tmp[6] == "mod")
                    {
                        for (int z = 10; z < 16; z++) int.Parse(tmp[z]);
                        result += String.Format("ы параметры маршрута {0} - {1}: ({2} км, {3} ч, {4} руб) на ({5} км, {6} ч, {7} руб)\n", tmp[8], tmp[9], tmp[10], tmp[11], tmp[12], tmp[13], tmp[14], tmp[15]);
                    }
                    else
                    {
                        for (int z = 10; z < 13; z++) int.Parse(tmp[z]);
                        result += String.Format(" маршрут: {0} - {1} ({2} км, {3} ч, {4} руб)\n",tmp[8], tmp[9], tmp[10], tmp[11], tmp[12]);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
            catch (FormatException)
            {
                return;
            }
            DbLogText.AppendText(result);
        }

        public void log_add_row(int op_type, int obj_type, object obj, object add_obj = null)
        {
            time = DateTime.Now;
            string timestamp = String.Format("{0},{1},{2},{3},{4},{5},", time.Day, time.Month, time.Year, time.Hour, time.Minute, time.Second);
            string local = String.Format("{0}/{1}/{2} {3}:{4}:{5} ", CRoute.TimeFormat(time.Day), CRoute.TimeFormat(time.Month), time.Year, CRoute.TimeFormat(time.Hour), CRoute.TimeFormat(time.Minute), CRoute.TimeFormat(time.Second));
            switch (op_type)
            {
                case 0: { timestamp += "add,"; local += "Добавлен";  break; }
                case 1: { timestamp += "del,"; local += "Удален"; break; }
                case 2: { timestamp += "mod,"; break; }
            }
            switch (obj_type)
            {
                case 0:
                    {
                        if (op_type == 2)
                        {
                            timestamp += String.Format("c,{0},{1}", obj, add_obj);
                            local += String.Format("Изменен город: {0} на {1}\n", obj, add_obj);
                        }
                        else
                        {
                            timestamp += String.Format("c,{0}", obj);
                            local += String.Format(" город: {0}", obj);
                        }
                        break;
                    }
                case 1:
                    {
                        CRoute tmp = (CRoute)obj;
                        if (op_type == 2)
                        {
                            string[] old_c = (string[])add_obj;
                            timestamp += String.Format("r,{0},{1},{2},{3},{4},{5},{6},{7}", tmp.FirstCity, tmp.SecondCity, tmp.Options[0], tmp.Options[1], tmp.Options[2], old_c[0], old_c[1], old_c[2]);
                            DbActionSuccessLabel.Text = String.Format("Изменен маршрут {0} - {1}: ({2} км, {3} ч, {4} руб) на ({5} км, {6} ч, {7} руб)", tmp.FirstCity, tmp.SecondCity, tmp.Options[0], tmp.Options[1], tmp.Options[2], old_c[0], old_c[1], old_c[2]);
                            local += DbActionSuccessLabel.Text;
                        }
                        else
                        {
                            timestamp += String.Format("r,{0},{1},{2},{3},{4}", tmp.FirstCity, tmp.SecondCity, tmp.Options[0], tmp.Options[1], tmp.Options[2]);
                            if (op_type == 1) DbActionSuccessLabel.Text = String.Format("Удалено: дорога {0} - {1} ({2}, {3}, {4})", tmp.FirstCity, tmp.SecondCity, tmp.Options[0], tmp.Options[1], tmp.Options[2]);
                            local += String.Format(" маршрут: {0} - {1} ({2} км, {3} ч, {4} руб)\n", tmp.FirstCity, tmp.SecondCity, tmp.Options[0], tmp.Options[1], tmp.Options[2]);
                        }
                        break;
                    }
            }   
            DbLogWriter.WriteLine(timestamp);
            DbLogText.AppendText(local);
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

        private void DbAddButton_Click(object sender, EventArgs e)
        {
            if (DbAddCity.Checked)
            {
                string newcity = CRoute.FormatCity(DbAddNewCityText.Text);
                int r = parent.AddCity(newcity);
                switch (r)
                {
                    case 0:
                        {
                            DbActionSuccessLabel.Text = "Добавлено: " + newcity;
                            DbAddRouteStartSelector.Items.Add(newcity);
                            DbAddRouteEndSelector.Items.Add(newcity);
                            DbDeleteCitySelector.Items.Add(newcity);
                            DbCitiesList.Items.Add(newcity);
                            DbModCitySelector.Items.Add(newcity);
                            DbAddNewCityText.Clear();
                            log_add_row(0, 0, newcity);
                            break;
                        }
                    case 1:
                        {
                            db_show_error("Ошибка: такой город уже существует");
                            break;
                        }
                    case 2:
                        {
                            db_show_error("Ошибка: введите название города");
                            break;
                        }
                    
                }
            }
            if (DbAddRoute.Checked)
            {
                if (DbAddRouteStartSelector.SelectedIndex >= 0 && DbAddRouteEndSelector.SelectedIndex >= 0)
                {
                    int r = parent.AddRoute(DbAddRouteStartSelector.SelectedItem.ToString(), DbAddRouteEndSelector.SelectedItem.ToString(), DbAddRouteLength.Text, DbAddRouteTime.Text, DbAddRouteCost.Text);
                    switch (r)
                    {
                        case 0:
                            {
                                CRoute qd = parent.routes.Last();
                                string qz = String.Format("{0} - {1} ({2},{3},{4})", qd.FirstCity, qd.SecondCity, qd.Options[0], qd.Options[1], qd.Options[2]);
                                DbDeleteRouteSelector.Items.Add(qz);
                                DbModRouteSelector.Items.Add(qz);
                                DbActionSuccessLabel.Text = String.Format("Добавлено: дорога {0} - {1} ({2} км, {3} ч, {4}$)", DbAddRouteStartSelector.SelectedItem.ToString(), DbAddRouteEndSelector.SelectedItem.ToString(), DbAddRouteLength.Text, DbAddRouteTime.Text, DbAddRouteCost.Text);
                                log_add_row(0, 1, qd);
                                DbRoutesDataGrid.Rows.Add(DbRoutesDataGrid.Rows.Count + 1, DbAddRouteStartSelector.SelectedItem.ToString(), DbAddRouteEndSelector.SelectedItem.ToString(), DbAddRouteLength.Text, DbAddRouteTime.Text, DbAddRouteCost.Text);
                                DbAddRouteEndSelector.ResetText();
                                DbAddRouteEndSelector.SelectedIndex = -1;
                                DbAddRouteStartSelector.ResetText();
                                DbAddRouteStartSelector.SelectedIndex = -1;
                                DbAddRouteLength.Clear();
                                DbAddRouteTime.Clear();
                                DbAddRouteCost.Clear();
                                break;
                            }
                        case 15:
                            {
                                db_show_error("Ошибка: такая дорога уже существует");
                                break;
                            }
                        case -1:
                            {
                                db_show_error("Ошибка: параметры должны быть целыми числами");
                                break;
                            }
                        case 8:
                            {
                                db_show_error("Ошибка: нельзя добавить такую дорогу");
                                break;
                            }
                        case 10:
                            {
                                db_show_error("Ошибка: параметры должны быть положительными");
                                break;
                            }
                        default:
                            {
                                string tmp;
                                tmp = "Ошибка: введите ";
                                if ((r & 1) == 1) tmp += "расстояние; ";
                                if (((r >> 1) & 1) == 1) tmp += "время; ";
                                if (((r >> 2) & 1) == 1) tmp += "стоимость; ";
                                db_show_error(tmp);
                                break;
                            }
                    }
                }
                else
                {
                    db_show_error("Ошибка: укажите начало и конец дороги");
                }
            }
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
                    DbActionSuccessLabel.Text = String.Format("Удалено: город {0} и {1} дорог", DbDeleteCitySelector.SelectedItem.ToString(), sizebefore - sizeafter);
                    log_add_row(1, 0, DbDeleteCitySelector.SelectedItem.ToString());
                    DbDeleteCitySelector.ResetText();
                    DbDeleteCitySelector.SelectedIndex = -1;
                    DbRefresh();
                }
                else
                {
                    db_show_error("Ошибка: выберите город");
                }
            }
            if (DbDeleteRoute.Checked)
            {
                if (DbDeleteRouteSelector.SelectedIndex >= 0)
                {
                    parent.DeleteRoute(DbDeleteRouteSelector.SelectedIndex);
                    log_add_row(1, 1, DbDeleteRouteSelector.Items[DbDeleteRouteSelector.SelectedIndex]);
                    int x = 0;
                    while (DbRoutesDataGrid.Rows[x].Cells[0].ToString() != (DbDeleteRouteSelector.SelectedIndex + 1).ToString()) x++;
                    DbRoutesDataGrid.Rows.RemoveAt(x);
                    DbDeleteRouteSelector.Items.RemoveAt(DbDeleteRouteSelector.SelectedIndex);
                    DbDeleteRouteSelector.ResetText();
                    DbDeleteRouteSelector.SelectedIndex = -1;
                }
                else
                {
                    db_show_error("Ошибка: выберите дорогу");
                }
            }
        }

        private void DbShowResultTimer_Tick(object sender, EventArgs e)
        {
            DbActionSuccessLabel.Visible = true;
            DbActionErrorLabel.Visible = false;
            DbShowResultTimer.Enabled = false;
        }

        private void db_show_error(string errortext)
        {
            DbActionErrorLabel.Text = errortext;
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

        private void DbModRoute_CheckedChanged(object sender, EventArgs e)
        {
            if (DbModRoute.Checked)
            {
                DbModRouteSelector.Show();
                DbModRouteOptions.Show();
                DbModCitySelector.Hide();
                DbModNewCityName.Hide();
                DbModLabel.Text = "Выберите дорогу:";
                DbModNewOptions.Text = "Новые параметры:";
            }
            else
            {
                DbModRouteSelector.Hide();
                DbModRouteOptions.Hide();
                DbModCitySelector.Show();
                DbModNewCityName.Show();
                DbModLabel.Text = "Выберите город:";
                DbModNewOptions.Text = "Новое имя города:";
            }
        }

        private void DbModRouteSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            DbModNewRouteLength.Text = parent.routes[DbModRouteSelector.SelectedIndex].Options[0].ToString();
            DbModNewRouteTime.Text = parent.routes[DbModRouteSelector.SelectedIndex].Options[1].ToString();
            DbModNewRouteCost.Text = parent.routes[DbModRouteSelector.SelectedIndex].Options[2].ToString();
        }

        private void DbModCitySelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            DbModNewCityName.Text = DbModCitySelector.SelectedItem.ToString();
        }

        private void DbModButton_Click(object sender, EventArgs e)
        {
            if (DbModCity.Checked)
            {
                if (DbModCitySelector.SelectedIndex >= 0)
                {
                    if (DbModNewCityName.Text == DbModCitySelector.SelectedItem.ToString())
                    {
                        db_show_error(String.Format("{0}: не изменено", DbModNewCityName.Text));
                        return;
                    }
                    else
                    {
                        string old = DbModCitySelector.SelectedItem.ToString();
                        string newcity = CRoute.FormatCity(DbModNewCityName.Text);
                        int r = parent.ModCity(old, newcity);
                        switch (r)
                        {
                            case 0:
                                {
                                    DbActionSuccessLabel.Text = String.Format("Изменен город: {0} на {1}", DbModCitySelector.SelectedItem.ToString(), newcity);
                                    log_add_row(2, 0, old, newcity);
                                    DbModNewCityName.ResetText();
                                    DbRefresh();
                                    break;
                                }
                            case 1:
                                {
                                    db_show_error("Ошибка: недопустимое новое имя города");
                                    break;
                                }
                        }
                    }
                }
                else db_show_error("Ошибка: выберите город");
            }
            else
            {
                if (DbModRouteSelector.SelectedIndex >= 0)
                {
                    int id = DbModRouteSelector.SelectedIndex;
                    int r = parent.ModRoute(id, DbModNewRouteLength.Text, DbModNewRouteTime.Text, DbModNewRouteCost.Text);
                    switch (r)
                    {
                        case 0:
                            {
                                string[] qd = { DbModNewRouteLength.Text, DbModNewRouteTime.Text, DbModNewRouteCost.Text };
                                log_add_row(2, 1, DbModRouteSelector.Items[id], qd);
                                DbModNewRouteCost.ResetText();
                                DbModNewRouteLength.ResetText();
                                DbModNewRouteTime.ResetText();
                                DbRefresh();
                                break;
                            }
                        case 8:
                            {
                                db_show_error("Не изменено");
                                break;
                            }
                        case 10:
                            {
                                db_show_error("Ошибка: параметры должны быть положительными");
                                break;
                            }
                        case -1:
                            {
                                db_show_error("Ошибка: параметры должны быть целыми числами");
                                break;
                            }
                        default:
                            {
                                string tmp;
                                tmp = "Ошибка: введите ";
                                if ((r & 1) == 1) tmp += "расстояние; ";
                                if (((r >> 1) & 1) == 1) tmp += "время; ";
                                if (((r >> 2) & 1) == 1) tmp += "стоимость; ";
                                db_show_error(tmp);
                                break;
                            }
                    }
                }
                else db_show_error("Ошибка: выберите дорогу");
            }
        }

        private void DbDeleteRouteSelector_Format(object sender, ListControlConvertEventArgs e)
        {
            //e.Value = String.Format("{0} - {1} ({2}, {3}, {4})",)
        }
    }
}
