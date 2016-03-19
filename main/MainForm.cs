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
    public partial class MainForm : Form
    {
        DbForm DataBase;
        bool DbLoaded;
        string DbPath;
        System.IO.StreamReader database;
        public List<string> cities { get; }
        public List<CRoute> routes { get; }
        private List<List<KeyValuePair<int, int[]>>> work_graph;
        private List<List<int>> SearchResult;
        Queue<int> work_queue;
        int[] used;

        public MainForm()
        {
            InitializeComponent();
            cities = new List<string>();
            routes = new List<CRoute>();
            DbLoaded = false;
            work_queue = new Queue<int>();
            work_graph = new List<List<KeyValuePair<int, int[]>>>();
            used = new int[1];
            DbPath = "database.dbp";
            SearchResult = new List<List<int>>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Top = (Screen.PrimaryScreen.Bounds.Height - Height) / 2;
            Left = (Screen.PrimaryScreen.Bounds.Width - Width) / 2;
        }

        private void MenuItemDB_Click(object sender, EventArgs e)
        {
            DataBase = new DbForm(this);
            if (!DataBase.Visible)
            {
                Enabled = false;
                DataBase.Enabled = true;
                Point a = new Point(Location.X + (Width - DataBase.Width) / 2, Location.Y + (Height - DataBase.Height) / 2);
                DataBase.Location = a;
                DataBase.Show();
            }
        }

        public bool DeleteCity(string city)
        {
            cities.Remove(city);
            routes.RemoveAll(x => x.FirstCity == city || x.SecondCity == city);
            return true;
        }

        public int AddCity(string city)
        {
            if (city == "") return 2;
            else
            {
                var _city = city.Substring(0, 1).ToUpper() + city.Substring(1).ToLower();
                if (!cities.Contains(_city))
                {
                    cities.Add(_city);
                    return 0;
                }
                else return 1;
            }
        }

        public int AddRoute(string start, string finish, string lenght, string time, string cost)
        {
            int l, t, c;
            try
            {
                l = int.Parse(lenght);
                t = int.Parse(time);
                c = int.Parse(cost);
            }
            catch (FormatException)
            {
                int q = 0;
                if (lenght == "") q += 1;
                if (time == "") q += 2;
                if (cost == "") q += 4;
                return (q == 0 ? -1 : q);
            }
            if (start.Equals(finish)) return 2;
            if (start.CompareTo(finish) > 0)
            {
                string tmp = start;
                start = finish;
                finish = tmp;
            }
            if (routes.Exists(x => x.FirstCity == start && x.SecondCity == finish && x.Options[0] == l && x.Options[1] == t && x.Options[2] == c))
                return 15;
            else
            {
                routes.Add(new CRoute(start, finish, l, t, c));
                return 0;
            }
        }

        public bool DeleteRoute(string st, string fn)
        {
            routes.RemoveAll(x => x.FirstCity == fn && x.SecondCity == st || x.FirstCity == st && x.SecondCity == fn);
            return true;
        }

        public bool DbUpload()
        {
            System.IO.StreamWriter databaseupload = new System.IO.StreamWriter(DbPath, false);
            databaseupload.WriteLine(cities.Count);
            foreach (var x in cities) databaseupload.WriteLine(x);
            databaseupload.WriteLine(routes.Count);
            foreach (var x in routes) databaseupload.WriteLine(x.FirstCity + ' ' + x.SecondCity + ' ' + x.Options[0] + ' ' + x.Options[1] + ' ' + x.Options[2]);
            databaseupload.Close();
            return true;
        }

        private void DbLoad()
        {
            int size = int.Parse(database.ReadLine());
            for(int i = 0; i < size; i++)
            {
                AddCity(database.ReadLine());
            }
            size = int.Parse(database.ReadLine());
            for (int i = 0; i < size; i++)
            {
                int len, t, cost;
                string[] c = database.ReadLine().Split(' ');
                try
                {
                    len = int.Parse(c[2]);
                    t = int.Parse(c[3]);
                    cost = int.Parse(c[4]);
                }
                catch (IndexOutOfRangeException)
                {
                    // invalid route
                    continue;
                }
                catch (FormatException)
                {
                    // invalid route lenght
                    continue;
                }
                if (cities.Exists(q => q == c[0]) && cities.Exists(q => q == c[1]))
                    routes.Add(new CRoute(c[0], c[1], len, t, cost));
            }
            DbLoaded = true;
            database.Close();
            BuildWorkGraph();
        }

        public bool BuildWorkGraph()
        {
            refresh();
            Array.Resize(ref used, cities.Count);
            work_graph.Clear();
            for(int i = 0; i < cities.Count; i++)
            {
                work_graph.Add(new List<KeyValuePair<int, int[]>>());
            }
            foreach(var x in routes)
            {
                int a = cities.FindIndex(q => q == x.FirstCity);
                int b = cities.FindIndex(q => q == x.SecondCity);
                work_graph[a].Add(new KeyValuePair<int, int[]>(b, x.Options));
                work_graph[b].Add(new KeyValuePair<int, int[]>(a, x.Options));
            }
            return true;
        }

        public void refresh()
        {
            cities.Sort();
            StartPointBox.Items.Clear();
            EndPointBox.Items.Clear();
            for (int i = 0; i < cities.Count; i++)
            {
                StartPointBox.Items.Add(cities[i]);
                EndPointBox.Items.Add(cities[i]);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                database = new System.IO.StreamReader(DbPath);
                DbLoad();
            }
            catch (System.IO.FileNotFoundException)
            {
                DialogResult newfile = MessageBox.Show("Файл базы данных не найден. Может быть он находится в другом месте?", "Файл не найден", MessageBoxButtons.OKCancel);
                if (newfile == DialogResult.Cancel) Application.Exit();    
                else
                {
                    OpenFileDialog newpath = new OpenFileDialog();
                    newpath.Multiselect = false;
                    newpath.Title = "Укажите файл с базой данных...";
                    newpath.Filter = "Файлы структуры путей|*.dbp|Все файлы|*.*";
                    int lt = Application.ExecutablePath.LastIndexOf('\\');
                    newpath.InitialDirectory = Application.ExecutablePath.Substring(0, lt);
                    newfile = newpath.ShowDialog();
                    if (newfile == DialogResult.Cancel) Application.Exit();
                    else
                    {
                        if (newfile == DialogResult.OK)
                        {
                            DbPath = newpath.FileName;
                            database = new System.IO.StreamReader(DbPath);
                            DbLoad();
                        }
                    }
                }
            }
        }

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DbLoaded) DbUpload();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (StartPointBox.SelectedIndex >= 0 && EndPointBox.SelectedIndex >= 0 && RouteCriteriaSelector.SelectedIndex >= 0)
                FindRoute(StartPointBox.SelectedItem.ToString(), EndPointBox.SelectedItem.ToString(), RouteCriteriaSelector.SelectedIndex);
            //else
            //TODO Ошибка поиска
        }

        private bool FindRoute(string start, string finish, int crit)
        {
            RouteSearchLog.Clear();
            for (int i = 0; i < used.Length; i++) used[i] = 0;
            int start_id = cities.FindIndex(q => q == start);
            int finish_id = cities.FindIndex(q => q == finish);
            work_queue.Clear();
            work_queue.Enqueue(start_id);
            used[start_id] = -1;
            RouteSearchLog.AppendText("Поиск маршрута " + cities[start_id] + " - " + cities[finish_id] + ":\n");
            while (work_queue.Count > 0)
            {
                int cur = work_queue.Dequeue();
                for(int i = 0; i < work_graph[cur].Count; i++)
                {
                    if (used[work_graph[cur][i].Key] == 0 || work_graph[cur][i].Value[crit] + used[cur] < used[work_graph[cur][i].Key])
                    {
                        work_queue.Enqueue(work_graph[cur][i].Key);
                        used[work_graph[cur][i].Key] = work_graph[cur][i].Value[crit] + used[cur];
                        //RouteSearchLog.AppendText(cities[work_graph[cur][i].Key] + "\n");
                    }
                }
            }
            if (used[finish_id] > 0)
            {
                SearchResult.Clear();
                bool Done = false;
                int RoutesCount = 1;
                SearchResult.Add(new List<int>());
                SearchResult[RoutesCount - 1].Add(finish_id);
                while (!Done)
                {
                    for(int i = 0; i < RoutesCount; i++)
                    {
                        int d1 = SearchResult[i].Last();
                        if (d1 != start_id)
                        {
                            int d2 = 0;
                            for (int j = 0; j < work_graph[d1].Count; j++)
                            { 
                                if (used[d1] - work_graph[d1][j].Value[crit] == used[work_graph[d1][j].Key])
                                {
                                    if (d2 == 0)
                                    {
                                        d2++;
                                        SearchResult[i].Add(work_graph[d1][j].Key);
                                    }
                                    else
                                    {
                                        SearchResult.Add(SearchResult[i].ToList());
                                        int q = SearchResult.Last().Count - 1;
                                        SearchResult[RoutesCount][q] = work_graph[d1][j].Key;
                                        RoutesCount++;
                                        d2++;
                                    }
                                }
                            }
                        }
                    }
                    Done = true;
                    for (int i = 0; i < RoutesCount; i++) Done = Done && (SearchResult[i].Last() == start_id);
                }
                RouteSearchLog.AppendText("Найдено " + RoutesCount + " маршрутов:\n");
                for(int i = 0; i < RoutesCount; i++)
                {
                    RouteSearchLog.AppendText("Маршрут #" + (i + 1) + ":\n");
                    for(int j = SearchResult[i].Count - 1; j >= 0; j--)
                    {
                        RouteSearchLog.AppendText(cities[SearchResult[i][j]]);
                        if (j > 0) RouteSearchLog.AppendText(" - ");
                    }
                    RouteSearchLog.AppendText("\n\n");
                }
            }
            else
            {
                RouteSearchLog.AppendText("Маршрут не найден.");
            }
            return true;
        }
    }

    public class CRoute
    {
        public string FirstCity { get; }
        public string SecondCity { get; }
        public int[] Options { get; }
        //public int Distance { get; }  // удалено

        public CRoute(string city1, string city2, int lenght, int time, int cost)
        {
            int d = city1.CompareTo(city2);
            if (d > 0)
            {
                FirstCity = city2;
                SecondCity = city1;
            }
            else
            {
                FirstCity = city1;
                SecondCity = city2;
            }
            Options = new int[3]; //расстояние, время, стоимость
            Options[0] = lenght;
            Options[1] = time;
            Options[2] = cost;
        }
    }
}
