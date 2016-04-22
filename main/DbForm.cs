using RouteFinderLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace main
{
    public partial class DbForm : Form
    {
        MainForm Db;
        bool shifted = false;
        LogProcessor Logger;
        List<string> LocalCities;
        List<CRoute> LocalRoutes;
        BindingSource[] bindings;
        BindingSource DGDataSource;

        public DbForm(MainForm db)
        {
            InitializeComponent();
            Db = db;
            Db.Enabled = false;
            Db.Visible = false;
            Logger = new LogProcessor();
            LocalCities = Db.Data.GetCitiesList();
            LocalRoutes = Db.Data.GetRoutesList();
            bindings = new BindingSource[7];
            for (int i = 0; i < 5; i++)
                bindings[i] = new BindingSource(LocalCities, "");
            bindings[5] = new BindingSource(LocalRoutes, "");
            bindings[6] = new BindingSource(LocalRoutes, "");
            DbAddRouteSource.DataSource = bindings[0];
            DbAddRouteDest.DataSource = bindings[1];
            DbDeleteCitySelector.DataSource = bindings[2];
            DbCitiesList.DataSource = bindings[3];
            DbModCitySelector.DataSource = bindings[4];
            DbDeleteRouteSelector.DataSource = bindings[5];
            DbModRouteSelector.DataSource = bindings[6];
            DGDataSource = new BindingSource(LocalRoutes, "");
            DbRoutesDataGrid.DataSource = DGDataSource;
        }
        
        private void DbRefresh()
        {
            for (int i = 0; i < 7; i++)
                bindings[i].ResetBindings(false);
            DGDataSource.ResetBindings(false);
        }

        private void DbRoutesRefresh()
        {
            //for (int i = 0; i < parent.routes.Count; i++)
            //    DbRoutesDataGrid.Rows.Add(i + 1, parent.routes[i].FirstCity, parent.routes[i].SecondCity, parent.routes[i].Options[0], parent.routes[i].Options[1], parent.routes[i].Options[2]);
            if (DbRoutesDataGrid.Rows.Count > 12)
            {
                DbRoutesColumn1.Width = 109;
                DbRoutesColumn2.Width = 109;
            }
            else
            {
                DbRoutesColumn1.Width = 118;
                DbRoutesColumn2.Width = 117;
            }
        }

        private void DbForm_Load(object sender, EventArgs e)
        {
            DbLogText.Lines = Logger.GetLocalRows();
            DbLogText.AppendText("\n");
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
            Logger.UploadLog();
            Db.Visible = true;
            Db.Enabled = true;
            Db.refresh();
            Db.Focus();
        }

        private void DbDeleteRoute_CheckedChanged(object sender, EventArgs e)
        {
            if (DbDeleteRoute.Checked)
            {
                DbDeleteLabel.Text = "Выберите дорогу:";
                DbDeleteRouteSelector.Visible = true;
                DbDeleteCitySelector.Visible = false;
                DbDeleteWarningLabel.Visible = false;
                DbDeleteRouteAll.Visible = true;
            }
            else
            {
                DbDeleteLabel.Text = "Выберите город:";
                DbDeleteRouteSelector.Visible = false;
                DbDeleteCitySelector.Visible = true;
                DbDeleteWarningLabel.Visible = true;
                DbDeleteRouteAll.Visible = false;
            }
        }
        private void DbAddButton_Click(object sender, EventArgs e)
        {
            if (DbAddCity.Checked)
                __AddCity(DbAddNewCityText.Text);
            if (DbAddRoute.Checked)
                __AddRoute(DbAddRouteSource.SelectedItem.ToString(), 
                             DbAddRouteDest.SelectedItem.ToString(), 
                             DbAddRouteLength.Text, 
                             DbAddRouteFuelCost.Text, 
                             DbAddRouteSpeedLimit.Text, 
                             DbAddRouteAddCosts.Text);
        }
        private void DbDeleteButton_Click(object sender, EventArgs e)
        {
            if (DbDeleteCity.Checked)
            {
                __DelCity(DbDeleteCitySelector.SelectedItem.ToString());
            }
            if (DbDeleteRoute.Checked)
            {
                __DelRoute((CRoute)DbDeleteRouteSelector.SelectedItem);
            }
        }
        private void DbShowResultTimer_Tick(object sender, EventArgs e)
        {
            DbActionSuccessLabel.Visible = true;
            DbActionErrorLabel.Visible = false;
            DbShowResultTimer.Enabled = false;
        }

        private void DBShowError(string errortext)
        {
            DbActionErrorLabel.Text = errortext;
            DbActionSuccessLabel.Visible = false;
            DbActionErrorLabel.Visible = true;
            DbShowResultTimer.Stop();
            DbShowResultTimer.Start();
        }
        private void DbClearLog_Click(object sender, EventArgs e)
        {
            Logger.ClearLog();
            DbLogText.Clear();
            DbActionSuccessLabel.Text = "История очищена";
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
            CRoute tmp = (CRoute)DbModRouteSelector.SelectedItem;
            if (tmp != null)
            {
                DbModNewRouteLength.Text = tmp.Options.Length.ToString();
                DbModNewRouteSpeedLimit.Text = tmp.Options.SpeedLimit.ToString();
                DbModNewRouteFuelCost.Text = tmp.Options.FuelCost.ToString();
                DbModNewRouteAddCosts.Text = tmp.Options.AddCosts.ToString();
            }
            else
            {
                DbModNewRouteLength.Text = "";
                DbModNewRouteSpeedLimit.Text = "";
                DbModNewRouteFuelCost.Text = "";
                DbModNewRouteAddCosts.Text = "";
            }
        }
        private void DbModCitySelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DbModCitySelector.SelectedItem != null)
                DbModNewCityName.Text = DbModCitySelector.SelectedItem.ToString();
            else
                DbModNewCityName.Text = "";
        }
        private void DbModButton_Click(object sender, EventArgs e)
        {
            if (DbModCity.Checked)
            {
                __ModCity(DbModCitySelector.SelectedItem.ToString(), DbModNewCityName.Text);
            }
            else
            {
                __ModRoute((CRoute)DbModRouteSelector.SelectedItem, 
                                   DbModNewRouteLength.Text,
                                   DbModNewRouteFuelCost.Text,
                                   DbModNewRouteSpeedLimit.Text,
                                   DbModNewRouteAddCosts.Text);
            }
        }

        private void DbTabs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                switch (DbTabs.SelectedIndex)
                {
                    case 0:
                        {
                            if (DbAddNewCityText.Focused || DbAddRouteSpeedLimit.Focused || DbAddRouteLength.Focused || DbAddRouteFuelCost.Focused || DbAddRouteAddCosts.Focused) DbAddButton_Click(sender, e);
                            break;
                        }
                    case 1:
                        {
                            if (shifted) DbDeleteButton_Click(sender, e);
                            break;
                        }
                    case 2:
                        {
                            if (DbModNewCityName.Focused || DbModNewRouteFuelCost.Focused || DbModNewRouteLength.Focused || DbModNewRouteSpeedLimit.Focused || DbModNewRouteAddCosts.Focused) DbModButton_Click(sender, e);
                            break;
                        }
                }
            }
        }
        private void DbTabs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift) shifted = true;
        }
        private void DbTabs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift) shifted = false;
        }
        private void DbInterfaceRefresh(DbOperation operation, params object[] args)
        {
            switch (operation)
            {
                case DbOperation.DB_ADD_CITY:
                    {
                        DbActionSuccessLabel.Text = string.Format("Добавлено: {0}", args);
                        DbAddNewCityText.Clear();
                        LocalCities.Add(RFDatabase.FormatCity((string)args[0]));
                        break;
                    }
                case DbOperation.DB_ADD_ROUTE:
                    {
                        DbActionSuccessLabel.Text = string.Format("Добавлено: {0} - {1} ({2}, {3}, {4}, {5})", args);
                        LocalRoutes.Add(new CRoute(args[0].ToString(), args[1].ToString(), int.Parse(args[2].ToString()), int.Parse(args[3].ToString()), int.Parse(args[4].ToString()), int.Parse(args[5].ToString())));
                        DbAddRouteLength.Clear();
                        DbAddRouteSpeedLimit.Clear();
                        DbAddRouteAddCosts.Clear();
                        DbAddRouteFuelCost.Clear();
                        break;
                    }
                case DbOperation.DB_REMOVE_CITY:
                    {
                        DbActionSuccessLabel.Text = string.Format("Удалено: {0}", args);
                        LocalCities.RemoveAt(DbDeleteCitySelector.SelectedIndex);
                        LocalRoutes.RemoveAll(x => x.FirstCity == args[0].ToString() || x.SecondCity == args[0].ToString());
                        break;
                    }
                case DbOperation.DB_REMOVE_ROUTE:
                    {
                        DbActionSuccessLabel.Text = string.Format("Удалено: {0} - {1} ({2}, {3}, {4}, {5})", args);
                        LocalRoutes.RemoveAt(DbDeleteRouteSelector.SelectedIndex);
                        break;
                    }
                case DbOperation.DB_MOD_CITY:
                    {
                        DbActionSuccessLabel.Text = string.Format("Изменен: {0} на {1}", args);
                        LocalCities[DbModCitySelector.SelectedIndex] = args[1].ToString();
                        DbModNewCityName.Clear();
                        break;
                    }
                case DbOperation.DB_MOD_ROUTE:
                    {
                        DbActionSuccessLabel.Text = string.Format("Изменен {0} - {1}: ({2}, {3}, {4}, {5}) на ({6}, {7}, {8}, {9})", args);
                        LocalRoutes[DbModRouteSelector.SelectedIndex] = (new CRoute(args[0].ToString(), args[1].ToString(), int.Parse(args[2].ToString()), int.Parse(args[3].ToString()), int.Parse(args[4].ToString()), int.Parse(args[5].ToString())));
                        DbModNewRouteAddCosts.Clear();
                        DbModNewRouteFuelCost.Clear();
                        DbModNewRouteLength.Clear();
                        DbModNewRouteSpeedLimit.Clear();
                        break;
                    }
            }
            DbRefresh();
        }

        private void __AddCity(string city)
        {
            city = RFDatabase.FormatCity(city);
            switch (Db.Data.AddCity(city))
            {
                case DBActionResult.OK:
                {
                    DbInterfaceRefresh(DbOperation.DB_ADD_CITY, city);
                    Logger.AddRow(DbOperation.DB_ADD_CITY, city);
                    DbLogText.AppendText(LogProcessor.GetLocalLogRow(DbOperation.DB_ADD_CITY, city));
                    break;
                }
                case DBActionResult.CITY_ALREADY_EXISTS:
                {
                    DBShowError("Ошибка: такой город уже существует");
                    break;
                }
                case DBActionResult.EMPTY_CITY:
                {
                    DBShowError("Введите название города");
                    break;
                }
                case DBActionResult.INVALID_CITY_NAME:
                {
                    DBShowError("Ошибка: имя города содержит недопустимые символы");
                    break;
                }
            }
        }
        private void __AddRoute(string source, string dest, string len, string fcost, string speed, string addcost)
        {
            try
            {
                int l = int.Parse(len);
                int fc = int.Parse(fcost);
                int s = int.Parse(speed);
                int ac = int.Parse(addcost);   
                switch (Db.Data.AddRoute(source, dest, l, fc, s, ac))
                {
                    case DBActionResult.OK:
                    {
                        DbInterfaceRefresh(DbOperation.DB_ADD_ROUTE, source, dest, len, fcost, speed, addcost);
                        Logger.AddRow(DbOperation.DB_ADD_ROUTE, source, dest, len, fcost, speed, addcost);
                        DbLogText.AppendText(LogProcessor.GetLocalLogRow(DbOperation.DB_ADD_ROUTE, source, dest, len, fcost, speed, addcost));
                        break;
                    }
                    case DBActionResult.ROUTE_SOURCE_EQUALS_DEST:
                    {
                        DBShowError("Ошибка: начало и конец дороги не могут совпадать");
                        break;
                    }
                    case DBActionResult.SAME_ROUTE_ALREADY_EXISTS:
                    {
                        DBShowError("Ошибка: дорога с такими параметрами уже существует");
                        break;
                    }
                    case DBActionResult.SOURCE_OR_DEST_CITY_NOT_EXISTS:
                    {
                        DBShowError("Ошибка: города начала или конца дороги не существует");
                        break;
                    }
                    case DBActionResult.EMPTY_SOURCE_OR_DEST_CITY:
                    {
                        DBShowError("Выберите начало и конец дороги");
                        break;
                    }
                }
            }
            catch (FormatException)
            {
                if (string.IsNullOrEmpty(len) || string.IsNullOrEmpty(fcost) || string.IsNullOrEmpty(speed) || string.IsNullOrEmpty(addcost))
                    DBShowError("Введите все параметры дороги");
                else
                    DBShowError("Ошибка: параметры должны быть целыми числами");
            }
        }
        private void __DelCity(string city)
        {
            switch (Db.Data.DeleteCity(city))
            {
                case DBActionResult.OK:
                    {
                        DbLogText.AppendText(LogProcessor.GetLocalLogRow(DbOperation.DB_REMOVE_CITY, city));
                        Logger.AddRow(DbOperation.DB_REMOVE_CITY, city);
                        DbInterfaceRefresh(DbOperation.DB_REMOVE_CITY, city);
                        break;
                    }
                case DBActionResult.CITY_NOT_EXISTS:
                    {
                        DBShowError("Ошибка: такого города не существует");
                        break;
                    }
                case DBActionResult.EMPTY_CITY:
                    {
                        DBShowError("Выберите город");
                        break;
                    }
            }
        }
        private void __DelRoute(CRoute route)
        {
            switch (Db.Data.DeleteRoute(route, DbDeleteRouteAll.Checked))
            {
                case DBActionResult.OK:
                    {
                        DbLogText.AppendText(LogProcessor.GetLocalLogRow(DbOperation.DB_REMOVE_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts));
                        Logger.AddRow(DbOperation.DB_REMOVE_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts);
                        DbInterfaceRefresh(DbOperation.DB_REMOVE_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts);
                        break;
                    }
                case DBActionResult.ROUTE_NOT_EXISTS:
                    {
                        DBShowError("Ошибка: такой дороги не существует");
                        break;
                    }
            }
        }
        private void __ModCity(string oldcity, string newcity)
        {
            newcity = RFDatabase.FormatCity(newcity);
            switch (Db.Data.ModCity(oldcity, newcity))
            {
                case DBActionResult.OK:
                    {
                        DbLogText.AppendText(LogProcessor.GetLocalLogRow(DbOperation.DB_MOD_CITY, oldcity, newcity));
                        Logger.AddRow(DbOperation.DB_MOD_CITY, oldcity, newcity);
                        DbInterfaceRefresh(DbOperation.DB_MOD_CITY, oldcity, newcity);
                        break;
                    }
                case DBActionResult.INVALID_NEW_CITYNAME:
                    {
                        DBShowError("Ошибка: недопустимое новое имя города");
                        break;
                    }
                case DBActionResult.CITYNAME_NOT_CHANGED:
                    {
                        DBShowError("Предупреждение: имя города не изменено");
                        break;
                    }
                case DBActionResult.CITY_NOT_EXISTS:
                    {
                        DBShowError("Ошибка: такого города не существует");
                        break;
                    }
                case DBActionResult.INVALID_VALUES:
                    {
                        DBShowError("Неизвестная ошибка");
                        break;
                    }
            }
        }
        private void __ModRoute(CRoute route, string nlen, string nfcost, string nspeed, string nacost)
        {
            try
            {
                int l = int.Parse(nlen);
                int fc = int.Parse(nfcost);
                int s = int.Parse(nspeed);
                int ac = int.Parse(nacost);
                switch (Db.Data.ModRoute(route, l, fc, s, ac))
                {
                    case DBActionResult.OK:
                        {
                            Logger.AddRow(DbOperation.DB_MOD_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts, l, fc, s, ac);
                            DbLogText.AppendText(LogProcessor.GetLocalLogRow(DbOperation.DB_MOD_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts, l, fc, s, ac));
                            DbInterfaceRefresh(DbOperation.DB_MOD_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts, l, fc, s, ac);
                            break;
                        }
                    case DBActionResult.ROUTE_NOT_CHANGED:
                        {
                            DBShowError("Предупреждение: параметры дороги не изменены");
                            break;
                        }
                    case DBActionResult.SAME_ROUTE_ALREADY_EXISTS:
                        {
                            DBShowError("Ошибка: дорога с такими параметрами уже существует");
                            break;
                        }
                    case DBActionResult.INVALID_VALUES:
                        {
                            DBShowError("Ошибка: недопустимые значения параметров");
                            break;
                        }
                    case DBActionResult.ROUTE_NOT_EXISTS:
                        {
                            DBShowError("Ошибка: такой дороги не существует");
                            break;
                        }
                }
            }
            catch (FormatException)
            {
                if (string.IsNullOrEmpty(nlen) || string.IsNullOrEmpty(nfcost) || string.IsNullOrEmpty(nspeed) || string.IsNullOrEmpty(nacost))
                    DBShowError("Введите все новые параметры дороги");
                else
                    DBShowError("Ошибка: параметры должны быть целыми числами");
            }
        }
    }
}