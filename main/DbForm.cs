using RouteFinderLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace main
{
    public partial class DbForm : Form
    {
        MainForm Db;
        bool shifted = false;
        LocalLogProcessor LocalLogger;
        OuterLogProcessor OuterLogger;
        List<string> LocalCities;
        List<CRoute> LocalRoutes;
        BindingSource[] bindings;
        BindingSource DGDataSource;
        Random AddRandom;
        bool AddConsole, CityChanged;

        private void SetBindings()
        {
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
            if (LocalRoutes.Count > 12)
            {
                DbRoutesDataGrid.Columns[1].Width = 115;
                DbRoutesDataGrid.Columns[0].Width = 115;
            }
            else
            {
                DbRoutesDataGrid.Columns[1].Width = 124;
                DbRoutesDataGrid.Columns[0].Width = 124;
            }
        }
        
        public DbForm(MainForm db)
        {
            InitializeComponent();

            Db = db;
            Db.Enabled = false;
            Db.Visible = false;
            AddConsole = false;
            CityChanged = false;

            OuterLogger = new OuterLogProcessor(Path.Combine(Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.LastIndexOf('\\'))));
            LocalLogger = new LocalLogProcessor(DbLogText, OuterLogger);
            AddRandom = new Random(DateTime.Now.Second * DateTime.Now.Millisecond);

            LocalCities = Db.Data.GetCitiesList();
            LocalRoutes = Db.Data.GetRoutesList();
            SetBindings();
        }
        private void DbRefresh()
        {
            for (int i = 0; i < 7; i++)
                bindings[i].ResetBindings(false);
            DGDataSource.ResetBindings(false);
            if (LocalRoutes.Count > 12)
            {
                DbRoutesDataGrid.Columns[1].Width = 115;
                DbRoutesDataGrid.Columns[0].Width = 115;
            }
            else
            {
                DbRoutesDataGrid.Columns[1].Width = 124;
                DbRoutesDataGrid.Columns[0].Width = 124;
            }
        }

        private void DbAddRoute_CheckedChanged(object sender, EventArgs e)
        {
            if (DbAddRoute.Checked)
            {
                DbAddNewCityLabel.Hide();
                DbAddNewCityText.Hide();
                if (!AddConsole)
                    DbAddRouteOptions.Show();
                else
                {
                    DbAddRouteConsole.Visible = !DbAddRouteConsole.Visible;
                    DbAddRouteRandom.Visible = !DbAddRouteRandom.Visible;
                    DbAddRandomSetSeed.Visible = !DbAddRandomSetSeed.Visible;
                    DbAddRouteIncludingName.Visible = !DbAddRouteIncludingName.Visible;
                }
            }
            else
            {
                DbAddNewCityLabel.Show();
                DbAddNewCityText.Show();
                if (!AddConsole)
                    DbAddRouteOptions.Hide();
                else
                {
                    DbAddRouteConsole.Visible = !DbAddRouteConsole.Visible;
                    DbAddRouteRandom.Visible = !DbAddRouteRandom.Visible;
                    DbAddRandomSetSeed.Visible = !DbAddRandomSetSeed.Visible;
                    DbAddRouteIncludingName.Visible = !DbAddRouteIncludingName.Visible;
                }
            }
        }
        private void DbForm_FormClosed(object sender, FormClosedEventArgs e)
        {   
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
                DbDeleteRouteAll.Visible = false;
            }
        }
        private void DbAddButton_Click(object sender, EventArgs e)
        {
            if (DbAddCity.Checked)
                __AddCity(DbAddNewCityText.Text);
            if (DbAddRoute.Checked)
            {
                if (LocalCities.Count > 1)
                {
                    if (DbAddRouteConsole.Visible)
                        __AddRouteConsole(DbAddRouteConsole.Text);
                    else
                        __AddRoute(DbAddRouteSource.SelectedItem.ToString(),
                                 DbAddRouteDest.SelectedItem.ToString(),
                                 DbAddRouteLength.Text,
                                 DbAddRouteFuelCost.Text,
                                 DbAddRouteSpeedLimit.Text,
                                 DbAddRouteAddCosts.Text,
                                 DbAddRouteName.Text);
                }
                else
                    DBShowError("Для этого необходимо добавить хотя бы два города");
            }
        }
        private void DbDeleteButton_Click(object sender, EventArgs e)
        {
            if (DbDeleteCity.Checked)
            {
                if (LocalCities.Count > 0)
                    __DelCity(DbDeleteCitySelector.SelectedItem.ToString());
                else
                    DBShowError("Для этого необходимо добавить хотя бы один город");
            }
            if (DbDeleteRoute.Checked)
            {
                if (LocalRoutes.Count > 0)
                    __DelRoute(DbDeleteRouteSelector.SelectedItem as CRoute);
                else
                    DBShowError("Для этого необходимо добавить хотя бы одну дорогу");
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
            LocalLogger.ClearLog();
            OuterLogger.ClearLog();
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
                DbModRouteNewRouteName.Text = tmp.Options.Name;
            }
            else
            {
                DbModNewRouteLength.Text = "";
                DbModNewRouteSpeedLimit.Text = "";
                DbModNewRouteFuelCost.Text = "";
                DbModNewRouteAddCosts.Text = "";
                DbModRouteNewRouteName.Text = "";
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
                if (LocalCities.Count > 0)
                    __ModCity(DbModCitySelector.SelectedItem.ToString(), DbModNewCityName.Text);
                else
                    DBShowError("Для этого необходимо добавить хотя бы один город");
            }
            else
            {
                if (LocalRoutes.Count > 0)
                    __ModRoute(DbModRouteSelector.SelectedItem as CRoute,
                                       DbModNewRouteLength.Text,
                                       DbModNewRouteFuelCost.Text,
                                       DbModNewRouteSpeedLimit.Text,
                                       DbModNewRouteAddCosts.Text,
                                       DbModRouteNewRouteName.Text);
                else
                    DBShowError("Для этого необходимо добавить хотя бы одну дорогу");
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
                            if (DbAddNewCityText.Focused || DbAddRouteSpeedLimit.Focused || DbAddRouteLength.Focused || DbAddRouteFuelCost.Focused || DbAddRouteAddCosts.Focused || DbAddRouteName.Focused) DbAddButton_Click(sender, e);
                            if (DbAddRouteConsole.Visible && !string.IsNullOrEmpty(DbAddRouteConsole.Text)) DbAddButton_Click(sender, e);
                            break;
                        }
                    case 1:
                        {
                            if (shifted) DbDeleteButton_Click(sender, e);
                            break;
                        }
                    case 2:
                        {
                            if (DbModNewCityName.Focused || DbModNewRouteFuelCost.Focused || DbModNewRouteLength.Focused || DbModNewRouteSpeedLimit.Focused || DbModNewRouteAddCosts.Focused || DbModRouteNewRouteName.Focused) DbModButton_Click(sender, e);
                            break;
                        }
                }
            }
            if ((e.KeyChar == 'E' || e.KeyChar == 'У') && DbTabs.SelectedIndex == 0 && DbAddRoute.Checked)
            {
                DbAddRouteOptions.Visible = !DbAddRouteOptions.Visible;
                DbAddRouteConsole.Visible = !DbAddRouteConsole.Visible;
                DbAddRouteRandom.Visible = !DbAddRouteRandom.Visible;
                DbAddRandomSetSeed.Visible = !DbAddRandomSetSeed.Visible;
                AddConsole = !AddConsole;
                DbAddRouteIncludingName.Visible = !DbAddRouteIncludingName.Visible;
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
                        DbActionSuccessLabel.Text = string.Format("Добавлено: {6} {0} - {1} ({2}, {3}, {4}, {5})", args);
                        LocalRoutes.Add(new CRoute(args[0].ToString(), args[1].ToString(), args[6].ToString(), int.Parse(args[2].ToString()), int.Parse(args[3].ToString()), int.Parse(args[4].ToString()), int.Parse(args[5].ToString())));
                        DbAddRouteLength.Clear();
                        DbAddRouteSpeedLimit.Clear();
                        DbAddRouteAddCosts.Clear();
                        DbAddRouteFuelCost.Clear();
                        DbAddRouteName.Clear();
                        DbAddRouteConsole.Clear();
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
                        DbActionSuccessLabel.Text = string.Format("Удалено: {6} {0} - {1} ({2}, {3}, {4}, {5})", args);
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
                        DbActionSuccessLabel.Text = string.Format("Изменено {10} {0} - {1}: ({2}, {3}, {4}, {5}) на ({11}, {6}, {7}, {8}, {9})", args);
                        LocalRoutes[DbModRouteSelector.SelectedIndex] = new CRoute(args[0].ToString(), args[1].ToString(), args[11].ToString(), int.Parse(args[6].ToString()), int.Parse(args[7].ToString()), int.Parse(args[8].ToString()), int.Parse(args[9].ToString()));
                        DbModCitySelector.SelectedIndex = DbModCitySelector.SelectedIndex;
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
                case RFLActionResult.OK:
                {
                    DbInterfaceRefresh(DbOperation.DB_ADD_CITY, city);
                    LocalLogger.AddRow(DbOperation.DB_ADD_CITY, city);
                    OuterLogger.AddRow(DbOperation.DB_ADD_CITY, city);
                    CityChanged = true;
                    break;
                }
                case RFLActionResult.CITY_ALREADY_EXISTS:
                {
                    DBShowError("Ошибка: такой город уже существует");
                    break;
                }
                case RFLActionResult.EMPTY_CITY:
                {
                    DBShowError("Введите название города");
                    break;
                }
                case RFLActionResult.INVALID_CITY_NAME:
                {
                    DBShowError("Ошибка: имя города содержит недопустимые символы");
                    break;
                }
            }
        }
        private void __AddRoute(string source, string dest, string len, string fcost, string speed, string addcost, string name = "")
        {
            try
            {
                int l = int.Parse(len);
                int fc = int.Parse(fcost);
                int s = int.Parse(speed);
                int ac = int.Parse(addcost);
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name)) CheckRouteName(name);
                switch (Db.Data.AddRoute(source, dest, l, fc, s, ac, name))
                {
                    case RFLActionResult.OK:
                    {
                        DbInterfaceRefresh(DbOperation.DB_ADD_ROUTE, source, dest, len, fcost, speed, addcost, name);
                        OuterLogger.AddRow(DbOperation.DB_ADD_ROUTE, source, dest, len, fcost, speed, addcost, name);
                        LocalLogger.AddRow(DbOperation.DB_ADD_ROUTE, source, dest, len, fcost, speed, addcost, name);
                        break;
                    }
                    case RFLActionResult.ROUTE_SOURCE_EQUALS_DEST:
                    {
                        DBShowError("Ошибка: начало и конец дороги не могут совпадать");
                        break;
                    }
                    case RFLActionResult.SAME_ROUTE_ALREADY_EXISTS:
                    {
                        DBShowError("Ошибка: дорога с такими параметрами уже существует");
                        break;
                    }
                    case RFLActionResult.SOURCE_OR_DEST_CITY_NOT_EXISTS:
                    {
                        DBShowError("Ошибка: города начала или конца дороги не существует");
                        break;
                    }
                    case RFLActionResult.EMPTY_SOURCE_OR_DEST_CITY:
                    {
                        DBShowError("Выберите начало и конец дороги");
                        break;
                    }
                    case RFLActionResult.INVALID_VALUES:
                    {
                        DBShowError("Значения выходят за пределы допустимых");
                        break;
                    }
                    case RFLActionResult.ROUTE_NAME_ALREADE_USED:
                    {
                        DBShowError("Такое название уже закреплено за другой дорогой");
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
            catch (ArgumentException exp)
            {
                if (exp.ParamName == "RouteName")
                    DBShowError(exp.Message);
                else
                    DBShowError("Неизвестная ошибка");
            }
        }
        private void __DelCity(string city)
        {
            switch (Db.Data.DeleteCity(city))
            {
                case RFLActionResult.OK:
                    {
                        OuterLogger.AddRow(DbOperation.DB_REMOVE_CITY, city);
                        LocalLogger.AddRow(DbOperation.DB_REMOVE_CITY, city);
                        DbInterfaceRefresh(DbOperation.DB_REMOVE_CITY, city);
                        CityChanged = true;
                        break;
                    }
                case RFLActionResult.CITY_NOT_EXISTS:
                    {
                        DBShowError("Ошибка: такого города не существует");
                        break;
                    }
                case RFLActionResult.EMPTY_CITY:
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
                case RFLActionResult.OK:
                    {
                        LocalLogger.AddRow(DbOperation.DB_REMOVE_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts, route.Options.Name);
                        OuterLogger.AddRow(DbOperation.DB_REMOVE_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts, route.Options.Name);
                        DbInterfaceRefresh(DbOperation.DB_REMOVE_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts, route.Options.Name);
                        break;
                    }
                case RFLActionResult.ROUTE_NOT_EXISTS:
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
                case RFLActionResult.OK:
                    {
                        LocalLogger.AddRow(DbOperation.DB_MOD_CITY, oldcity, newcity);
                        OuterLogger.AddRow(DbOperation.DB_MOD_CITY, oldcity, newcity);
                        DbInterfaceRefresh(DbOperation.DB_MOD_CITY, oldcity, newcity);
                        CityChanged = true;
                        break;
                    }
                case RFLActionResult.INVALID_NEW_CITYNAME:
                    {
                        DBShowError("Ошибка: недопустимое новое имя города");
                        break;
                    }
                case RFLActionResult.CITYNAME_NOT_CHANGED:
                    {
                        DBShowError("Предупреждение: имя города не изменено");
                        break;
                    }
                case RFLActionResult.CITY_NOT_EXISTS:
                    {
                        DBShowError("Ошибка: такого города не существует");
                        break;
                    }
                case RFLActionResult.INVALID_VALUES:
                    {
                        DBShowError("Неизвестная ошибка");
                        break;
                    }
            }
        }
        private void __ModRoute(CRoute route, string nlen, string nfcost, string nspeed, string nacost, string nname = "")
        {
            try
            {
                int l = int.Parse(nlen);
                int fc = int.Parse(nfcost);
                int s = int.Parse(nspeed);
                int ac = int.Parse(nacost);
                if (!string.IsNullOrEmpty(nname) && !string.IsNullOrWhiteSpace(nname)) CheckRouteName(nname);
                switch (Db.Data.ModRoute(route, l, fc, s, ac, nname))
                {
                    case RFLActionResult.OK:
                        {
                            LocalLogger.AddRow(DbOperation.DB_MOD_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts, l, fc, s, ac, route.Options.Name, nname);
                            OuterLogger.AddRow(DbOperation.DB_MOD_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts, l, fc, s, ac, route.Options.Name, nname);
                            DbInterfaceRefresh(DbOperation.DB_MOD_ROUTE, route.FirstCity, route.SecondCity, route.Options.Length, route.Options.FuelCost, route.Options.SpeedLimit, route.Options.AddCosts, l, fc, s, ac, route.Options.Name, nname);
                            break;
                        }
                    case RFLActionResult.ROUTE_NOT_CHANGED:
                        {
                            DBShowError("Параметры дороги не изменены");
                            break;
                        }
                    case RFLActionResult.SAME_ROUTE_ALREADY_EXISTS:
                        {
                            DBShowError("Ошибка: другая дорога уже имеет такие параметры");
                            break;
                        }
                    case RFLActionResult.INVALID_VALUES:
                        {
                            DBShowError("Ошибка: недопустимые значения параметров");
                            break;
                        }
                    case RFLActionResult.ROUTE_NOT_EXISTS:
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
        private void __AddRouteConsole(string consoletext)
        {
            var tmp = DbAddRouteConsole.Text.Split(',');
            if (tmp.Length < 6 || tmp.Length > 7)
                DBShowError("Неверное количество параметров");
            else
                __AddRoute(tmp[0], tmp[1], tmp[2], tmp[3], tmp[4], tmp[5], (tmp.Length == 7 ? tmp[6] : ""));
        }

        private void CheckRouteName(string name)
        {
            for (int i = 0; i < name.Length; i++)
                if (!char.IsLetterOrDigit(name[i]))
                    throw new ArgumentException("Некорректное наименование трассы", "RouteName");
        }

        private void DbAddRouteSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DbAddRouteAddCosts.Text))
                DbAddRouteAddCosts.Text = "0";
            if (string.IsNullOrEmpty(DbAddRouteSpeedLimit.Text))
                DbAddRouteSpeedLimit.Text = "90";
        }
        private void DbAddRouteDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DbAddRouteAddCosts.Text))
                DbAddRouteAddCosts.Text = "0";
            if (string.IsNullOrEmpty(DbAddRouteSpeedLimit.Text))
                DbAddRouteSpeedLimit.Text = "90";
        }
        private void DbAddRouteRandom_Click(object sender, EventArgs e)
        {
            if (LocalCities.Count > 1)
            {
                int c1 = 0, c2 = 0;
                while (c1 == c2)
                {
                    c1 = AddRandom.Next(0, LocalCities.Count);
                    c2 = AddRandom.Next(0, LocalCities.Count);
                }
                int l = AddRandom.Next(50, RouteParams.MaxLength);
                int fc = AddRandom.Next(1, RouteParams.MaxFuelCost);
                int s = AddRandom.Next(40, RouteParams.MaxSpeedLimit);
                int ac = AddRandom.Next(0, RouteParams.MaxAddCosts);
                DbAddRouteConsole.Text = string.Format("{0},{1},{2},{3},{4},{5}", LocalCities[c1], LocalCities[c2], l, fc, s, ac);
                if (DbAddRouteIncludingName.Checked)
                {
                    char c = (char)AddRandom.Next('A', 'Z');
                    char cc = (char)AddRandom.Next('A', 'Z');
                    DbAddRouteConsole.Text += string.Format(",{0}{1}{2}", c, (AddRandom.Next(0, 2) == 1 ? cc.ToString() : ""), AddRandom.Next(1, 1000));
                }
            }
            else
                DBShowError("Для этого необходимо добавить хотя бы два города");
        }
        private void DbAddRandomSetSeed_Click(object sender, EventArgs e)
        {
            AddRandom = new Random(DateTime.Now.Second * DateTime.Now.Millisecond);
        }

        private void DbForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OuterLogger.UploadLog();
            Db.Visible = true;
            Db.Enabled = true;
            Db.refresh(CityChanged);
            Db.Focus();
        }
    }
}