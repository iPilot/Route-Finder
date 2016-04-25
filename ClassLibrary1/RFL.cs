using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace RouteFinderLibrary
{
    public class RouteParams
    {
        public const int ParamsCount = 4;
        public const int MaxLength = 10000;
        public const int MaxFuelCost = 500;
        public const int MaxSpeedLimit = 150;
        public const int MaxAddCosts = 10000;
        public int Length { get; }
        public int FuelCost { get; }
        public int AddCosts { get; }
        public int SpeedLimit { get; }
        public RouteParams(int length, int fcost, int speed, int addcosts)
        {
            if (length > 0 && fcost > 0 && addcosts >= 0 && speed >= 0)
            {
                Length = length;
                FuelCost = fcost;
                AddCosts = addcosts;
                SpeedLimit = speed;
            }
            else
            {
                Length = 0;
                FuelCost = 0;
                AddCosts = 0;
                SpeedLimit = 0;
                throw new ArgumentException("Values must be positive");
            }
        }
        public bool Equals(RouteParams left)
        {
            if (left == null)
                return false;
            else
                return (Length == left.Length && FuelCost == left.FuelCost && SpeedLimit == left.SpeedLimit && AddCosts == left.AddCosts);
        }
    }
    public enum DBActionResult
    {
        OK,
        CITY_ALREADY_EXISTS,
        CITY_NOT_EXISTS,
        EMPTY_CITY,
        SAME_ROUTE_ALREADY_EXISTS,
        ROUTE_SOURCE_EQUALS_DEST,
        DBFILE_NOT_FOUND,
        INVALID_FILE_FORMAT,
        CITY_NAME_TOO_LONG,
        INVALID_CITY_NAME,
        INVALID_VALUES,
        SOURCE_OR_DEST_CITY_NOT_EXISTS,
        EMPTY_SOURCE_OR_DEST_CITY,
        EMPTY_ROUTE_VALUES,
        ROUTE_NOT_EXISTS,
        INVALID_NEW_CITYNAME,
        CITYNAME_NOT_CHANGED,
        ROUTE_NOT_CHANGED        
    }
    public enum DbOperation
    {
        DB_ADD_CITY,
        DB_REMOVE_CITY,
        DB_MOD_CITY,
        DB_ADD_ROUTE,
        DB_REMOVE_ROUTE,
        DB_MOD_ROUTE
    }

    public class RFDatabase
    {
        SortedSet<string> Cities;
        Dictionary<Tuple<string, string>, List<RouteParams>> Routes;
        public bool DbLoaded { get; private set; }
        int RoutesCount;
        string DbPath;
        public string DbDefaultFileName  { get; }
        Queue<int> bfsqueue;
        List<List<Tuple<int, List<RouteParams>>>> graph;
        List<List<int>> SearchResults;
        int[] used;

        public RFDatabase()
        {
            Cities = new SortedSet<string>();
            Routes = new Dictionary<Tuple<string, string>, List<RouteParams>>();
            DbLoaded = false;
            RoutesCount = 0;
            DbDefaultFileName = "routefinder.dbp";
        }
        public List<string> GetCitiesList()
        {
            return Cities.ToList();
        }
        public List<CRoute> GetRoutesList()
        {
            var tmp = new List<CRoute>(Routes.Select(x => x.Value.Count).Sum());
            foreach (var roads in Routes)
                foreach (var road in roads.Value)
                    tmp.Add(CRoute.Create(roads.Key, road));
            return tmp;
        }
        bool CheckValues(params int[] _values)
        {
            if (_values.Length != 4) return false;
            else
                return (_values[0] > 0 && _values[0] <= RouteParams.MaxLength &&
                        _values[1] > 0 && _values[1] <= RouteParams.MaxFuelCost &&
                        _values[2] >= 0 && _values[2] <= RouteParams.MaxSpeedLimit &&
                        _values[3] >= 0 && _values[3] <= RouteParams.MaxAddCosts);
        }
        public static bool CheckCityName(string city)
        {
            bool result = true;
            foreach (char c in city)
                result &= char.IsLetter(c) | c == '-';
            return result;
        }
        public static string FormatCity(string text)
        {
            bool up = true;
            char[] tmp = text.Trim().ToLowerInvariant().ToCharArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (up)
                {
                    tmp[i] = char.ToUpperInvariant(tmp[i]);
                    up = false;
                }
                if (tmp[i] == ' ' || tmp[i] == '-') up = true;
            }
            return new string(tmp);
        }
        public DBActionResult AddCity(string city)
        {
            if (string.IsNullOrEmpty(city)) return DBActionResult.EMPTY_CITY;
            else
                if (!CheckCityName(city)) return DBActionResult.INVALID_CITY_NAME;
            else
                return (Cities.Add(city) ? DBActionResult.OK : DBActionResult.CITY_ALREADY_EXISTS);
        }
        public DBActionResult AddRoute(string source, string dest, int len, int cost, int speed, int addcost)
        {
            int d = source.CompareTo(dest);
            if (d == 0) return DBActionResult.ROUTE_SOURCE_EQUALS_DEST;
            if (!Cities.Contains(source) || !Cities.Contains(dest)) return DBActionResult.SOURCE_OR_DEST_CITY_NOT_EXISTS;
            if (!CheckValues(len, cost, speed, addcost)) return DBActionResult.INVALID_VALUES;
            else
            {
                if (d > 0)
                {
                    var tmp = source; 
                    source = dest;
                    dest = tmp;
                }
                var key = new Tuple<string, string>(source, dest);
                if (!Routes.ContainsKey(key))
                {
                    Routes.Add(key, new List<RouteParams>());
                    Routes[key].Add(new RouteParams(len, cost, speed, addcost));
                    RoutesCount++;
                    return DBActionResult.OK;
                }
                else
                {
                    if (!Routes[key].Exists(x => x.Length == len && x.FuelCost == cost && x.AddCosts == addcost && x.SpeedLimit == speed))
                    {
                        Routes[key].Add(new RouteParams(len, cost, speed, addcost));
                        RoutesCount++;
                        return DBActionResult.OK;
                    }
                    else
                        return DBActionResult.SAME_ROUTE_ALREADY_EXISTS;
                }
            }
        }
        public DBActionResult DeleteCity(string city)
        {
            if (string.IsNullOrEmpty(city)) return DBActionResult.EMPTY_CITY;
            else
            {
                foreach (var key in Routes.Where(x => x.Key.Item1 == city || x.Key.Item2 == city).ToList())
                {
                    Routes.Remove(key.Key);
                    RoutesCount -= key.Value.Count;
                }
                return (Cities.Remove(city) ? DBActionResult.OK : DBActionResult.CITY_NOT_EXISTS);
            }
        }
        public DBActionResult DeleteRoute(CRoute route, bool all)
        {
            Tuple<string, string> key = new Tuple<string, string>(route.FirstCity, route.SecondCity);
            if (!Routes.ContainsKey(key))
                return DBActionResult.ROUTE_NOT_EXISTS;
            else
            {
                if (all)
                {
                    RoutesCount -= Routes[key].Count;
                    return (Routes.Remove(key) ? DBActionResult.OK : DBActionResult.ROUTE_NOT_EXISTS);
                }
                else
                {
                    RoutesCount--;
                    return (Routes[key].RemoveAll(x => x.Length == route.Length && x.SpeedLimit == route.SpeedLimit && x.FuelCost == route.FuelCost && x.AddCosts == route.AddCosts) > 0 ? DBActionResult.OK : DBActionResult.ROUTE_NOT_EXISTS);
                }
            }
        }
        public DBActionResult ModCity(string city, string newcity)
        {
            if (city == newcity) return DBActionResult.CITYNAME_NOT_CHANGED;
            if (!Cities.Contains(city))
                return DBActionResult.CITY_NOT_EXISTS;
            else if (Cities.Contains(newcity))
                return DBActionResult.INVALID_NEW_CITYNAME;
            else
                return (Cities.Add(newcity) && Cities.Remove(city) ? DBActionResult.OK : DBActionResult.INVALID_VALUES);
        }
        public DBActionResult ModRoute(CRoute route, int newlen, int newfcost, int newspeed, int newaddcost)
        {
            int id;
            if (route == null) return DBActionResult.ROUTE_NOT_EXISTS;
            var key = new Tuple<string, string>(route.FirstCity, route.SecondCity);
            if (!Routes.ContainsKey(key))
                return DBActionResult.ROUTE_NOT_EXISTS;
            else if ((id = Routes[key].FindIndex(x => x.Length == route.Length && x.SpeedLimit == route.SpeedLimit && x.FuelCost == route.FuelCost && x.AddCosts == route.AddCosts)) < 0)
                return DBActionResult.ROUTE_NOT_EXISTS;
            else if (!CheckValues(newlen, newfcost, newspeed, newaddcost))
                return DBActionResult.INVALID_VALUES;
            else
            {
                var tmp = new RouteParams(newlen, newfcost, newspeed, newaddcost);
                if (tmp.Equals(route.Options)) return DBActionResult.ROUTE_NOT_CHANGED;
                int k = Routes[key].FindIndex(x => x.Length == tmp.Length && x.SpeedLimit == tmp.SpeedLimit && x.FuelCost == tmp.FuelCost && x.AddCosts == tmp.AddCosts);
                if (k >= 0) return DBActionResult.SAME_ROUTE_ALREADY_EXISTS;
                else
                {
                    Routes[key][id] = tmp;
                    return DBActionResult.OK;
                }
            }
        }
        public DBActionResult DbLoad(string path)
        {
            string ptr;
            if (string.IsNullOrEmpty(path))
                return DBActionResult.DBFILE_NOT_FOUND;
            else
                DbPath = path;
            try
            {
                var DbReader = new StreamReader(DbPath, Encoding.UTF8);
                int cc = int.Parse(DbReader.ReadLine());
                for(int i = 0; i < cc; i++)
                {
                    ptr = DbReader.ReadLine();
                    if (ptr.Length > 30)
                        continue;
                    else
                        Cities.Add(ptr);
                }
                RoutesCount = 0;
                int rc = int.Parse(DbReader.ReadLine());
                for(int i = 0; i < rc; i++)
                {
                    ptr = DbReader.ReadLine();
                    var prms = ptr.Split(',');
                    if (prms.Length != RouteParams.ParamsCount + 2) continue;
                    else
                    {
                        try
                        {
                            int[] values = new int[] { int.Parse(prms[2]), int.Parse(prms[3]), int.Parse(prms[4]), int.Parse(prms[5]) };
                            AddRoute(prms[0], prms[1], values[0], values[1], values[2], values[3]);
                        }
                        catch (FormatException)
                        {
                            continue;
                        }
                    }
                }
                DbLoaded = true;  
                DbReader.Close();
                return DBActionResult.OK;
            }
            catch (FileNotFoundException)
            {
                DbLoaded = false;
                return DBActionResult.DBFILE_NOT_FOUND;
            }
            catch (FormatException)
            {
                DbLoaded = false;
                return DBActionResult.INVALID_FILE_FORMAT;
            }
            catch (ArgumentException)
            {
                DbLoaded = false;
                return DBActionResult.DBFILE_NOT_FOUND;
            }
            catch (DirectoryNotFoundException)
            {
                DbLoaded = false;
                return DBActionResult.DBFILE_NOT_FOUND;
            }
        }
        public void DbUpload()
        {
            if (DbLoaded)
            {
                var DbWriter = new StreamWriter(DbPath, false, Encoding.UTF8);
                DbWriter.WriteLine(Cities.Count);
                foreach (var city in Cities)
                    DbWriter.WriteLine(city);
                DbWriter.WriteLine(RoutesCount);
                foreach(var rt in Routes)
                    foreach(var route in rt.Value)
                        DbWriter.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", rt.Key.Item1, rt.Key.Item2, route.Length, route.FuelCost, route.SpeedLimit, route.AddCosts));
                DbWriter.Close();
            }
        }
        public List<List<Tuple<int, List<RouteParams>>>> BuildWorkGraph()
        {
            var tmp = new List<List<Tuple<int, List<RouteParams>>>>(Cities.Count);
            for(int i = 0; i < tmp.Count; i++)
            {
                tmp[i] = new List<Tuple<int, List<RouteParams>>>();
            }
            var ct = Cities.ToList();
            foreach(var x in Routes)
            {
                int a = ct.FindIndex(q => q == x.Key.Item1);
                int b = ct.FindIndex(q => q == x.Key.Item2);
                var lst = new List<RouteParams>(x.Value);
                tmp[a].Add(new Tuple<int, List<RouteParams>>(b, lst));
                tmp[b].Add(new Tuple<int, List<RouteParams>>(a, lst));
            }
            return tmp;
        }
        public void BuildGraph()
        {
            graph = BuildWorkGraph();
            used = new int[graph.Count];
            bfsqueue = new Queue<int>();
            SearchResults = new List<List<int>>();
        }
        private bool FindRoute(string start, string finish, int crit)
        {
            throw new NotImplementedException();
            //RouteSearchLog.Clear();
            //for (int i = 0; i < used.Length; i++) used[i] = 0;
            //int start_id = Cities.FindIndex(q => q == start);
            //int finish_id = cities.FindIndex(q => q == finish);
            //bfsqueue.Clear();
            //bfsqueue.Enqueue(start_id);
            //used[start_id] = -1;
            //RouteSearchLog.AppendText("Поиск маршрута " + cities[start_id] + " - " + cities[finish_id] + ":\n");
            //while (bfsqueue.Count > 0)
            //{
            //    int cur = bfsqueue.Dequeue();
            //    for (int i = 0; i < graph[cur].Count; i++)
            //    {
            //        if (used[graph[cur][i].Item1] == 0 || graph[cur][i].Value[crit] + used[cur] < used[graph[cur][i].Key])
            //        {
            //            bfsqueue.Enqueue(graph[cur][i].Key);
            //            used[graph[cur][i].Key] = graph[cur][i].Value[crit] + used[cur];
            //        }
            //    }
            //}
            //if (used[finish_id] > 0)
            //{
            //    SearchResult.Clear();
            //    bool Done = false;
            //    int RoutesCount = 1;
            //    SearchResult.Add(new List<int>());
            //    SearchResult[RoutesCount - 1].Add(finish_id);
            //    while (!Done)
            //    {
            //        for (int i = 0; i < RoutesCount; i++)
            //        {
            //            int d1 = SearchResult[i].Last();
            //            if (d1 != start_id)
            //            {
            //                int d2 = 0;
            //                for (int j = 0; j < graph[d1].Count; j++)
            //                {
            //                    if (used[d1] - graph[d1][j].Value[crit] == used[graph[d1][j].Key])
            //                    {
            //                        if (d2 == 0)
            //                        {
            //                            d2++;
            //                            SearchResult[i].Add(graph[d1][j].Key);
            //                        }
            //                        else
            //                        {
            //                            SearchResult.Add(SearchResult[i].ToList());
            //                            int q = SearchResult.Last().Count - 1;
            //                            SearchResult[RoutesCount][q] = graph[d1][j].Key;
            //                            RoutesCount++;
            //                            d2++;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        Done = true;
            //        for (int i = 0; i < RoutesCount; i++) Done = Done && (SearchResult[i].Last() == start_id);
            //    }
            //    RouteSearchLog.AppendText("Найдено " + RoutesCount + " маршрутов:\n");
            //    for (int i = 0; i < RoutesCount; i++)
            //    {
            //        RouteSearchLog.AppendText("Маршрут #" + (i + 1) + ":\n");
            //        for (int j = SearchResult[i].Count - 1; j >= 0; j--)
            //        {
            //            RouteSearchLog.AppendText(cities[SearchResult[i][j]]);
            //            if (j > 0) RouteSearchLog.AppendText(" - ");
            //        }
            //        RouteSearchLog.AppendText("\n\n");
            //    }
            //}
            //else
            //{
            //    RouteSearchLog.AppendText("Маршрут не найден.");
            //}
            //return true;
        }
        ~RFDatabase()
        {
            if (DbLoaded) DbUpload();
        }
    }
    public class CRoute
    {
        public string FirstCity { get; }
        public string SecondCity { get; }
        public RouteParams Options { get; }
        public int Length
        {
            get { return Options.Length; }
        }
        public int FuelCost
        {
            get { return Options.FuelCost; }
        }
        public int SpeedLimit
        {
            get { return Options.SpeedLimit; }
        }
        public int AddCosts
        {
            get { return Options.AddCosts; }
        }

        public CRoute(string city1, string city2, int length, int fcost, int speed, int addcosts)
        {
            if (city1.CompareTo(city2) > 0)
            {
                FirstCity = city2;
                SecondCity = city1;
            }
            else
            {
                FirstCity = city1;
                SecondCity = city2;
            }
            Options = new RouteParams(length, fcost, speed, addcosts);
        }
        public override string ToString()
        {
            if (Options != null)
                return string.Format("{0} - {1} ({2} км, {3} руб/л, {4} км/ч, {5} руб)", FirstCity, SecondCity, Options.Length, Options.FuelCost, Options.SpeedLimit, Options.AddCosts);
            else
                return "Invalid route";
        }
        public static CRoute Create(Tuple<string, string> key, RouteParams value)
        {
           return new CRoute(key.Item1, key.Item2, value.Length, value.FuelCost, value.SpeedLimit, value.AddCosts);
        }
    }
    public class __LogProcessor
    {
        const int MaxRows = 50;
        string[] data;
        List<string> newdata;
        bool loaded;
        StreamReader logreader;
        StreamWriter logwriter;
        int position;
        bool logoverflow;
        string localpath;
        string LogDefaultFileName;

        public __LogProcessor()
        {
            LogDefaultFileName = "routefinder.log";
            localpath = Path.Combine(Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')), LogDefaultFileName);
            position = 0;
            data = new string[MaxRows];
            newdata = new List<string>();
            try
            {
                logreader = new StreamReader(localpath);
                LoadLog();
                loaded = true;
            }
            catch (FileNotFoundException)
            {
                loaded = false;
            }
        }
        public void LoadLog()
        {
            if (logreader != null)
            {
                int k = 0;
                while (!logreader.EndOfStream)
                {
                    data[k % MaxRows] = logreader.ReadLine();
                    k++;
                }
                if (k >= MaxRows) logoverflow = true;
                k %= MaxRows;
                position = (k  == 0 ? MaxRows - 1 : k - 1);
                logreader.Close();
            }
            else
                throw new ArgumentNullException("Logreader");
        }
        public void UploadLog()
        {
            logwriter = new StreamWriter(localpath, true, Encoding.UTF8);
            int first = 0;
            for (int i = first; i < newdata.Count; i++)
                logwriter.WriteLine(newdata[i]);
            logwriter.Close();
        }
        public void AddRow(DbOperation operation, params object[] args)
        {
            string timestamp = string.Format("{0:dd,MM,yyyy,HH,mm,ss},", DateTime.Now);
            
        }
        public static string GetLocalLogRow(DbOperation operation, params object[] args)
        {
            return string.Format("{0:dd-MM-yyyy HH:mm:ss} {1}\n", DateTime.Now, GetRow(operation, args));
        }
        public string[] GetLocalRows()
        {
            if (loaded)
            {
                string[] result = new string[(logoverflow ? MaxRows : position + 1)];
                int q = 0;
                string z;
                if (logoverflow)
                {
                    for (int i = position + 1; i < MaxRows; i++)
                    {
                        z = transformrow(data[i]);
                        if (!string.IsNullOrEmpty(z))
                            result[q++] = z;
                    }
                }
                for (int i = 0; i <= position; i++)
                {
                    z = transformrow(data[i]);
                    if (!string.IsNullOrEmpty(z))
                        result[q++] = z;
                }
                return result.TakeWhile(x => !string.IsNullOrEmpty(x)).ToArray();
            }
            else
                return null;
        }
        private static string transformrow(string row)
        {
            try
            {
                var tmp = row.Split(',');
                string timestamp = string.Format("{0}-{1}-{2} {3}:{4}:{5} ", tmp.Take(6).ToArray());
                switch (tmp[6])
                {
                    case "add":
                        {
                            if (tmp[7] == "c")
                                return timestamp + GetRow(DbOperation.DB_ADD_CITY, tmp.Skip(8).ToArray());
                            else if (tmp[7] == "r")
                                return timestamp + GetRow(DbOperation.DB_ADD_ROUTE, tmp.Skip(8).ToArray());
                            else
                                return null;
                        }
                    case "del":
                        {
                            if (tmp[7] == "c")
                                return timestamp + GetRow(DbOperation.DB_REMOVE_CITY, tmp.Skip(8).ToArray());
                            else if (tmp[7] == "r")
                                return timestamp + GetRow(DbOperation.DB_REMOVE_ROUTE, tmp.Skip(8).ToArray());
                            else
                                return null;
                        }
                    case "mod":
                        {
                            if (tmp[7] == "c")
                                return timestamp + GetRow(DbOperation.DB_MOD_CITY, tmp.Skip(8).ToArray());
                            else if (tmp[7] == "r")
                                return timestamp + GetRow(DbOperation.DB_MOD_ROUTE, tmp.Skip(8).ToArray());
                            else
                                return null;
                        }
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static string GetRow(DbOperation op, params object[] args)
        {
            switch (op)
            {
                case DbOperation.DB_ADD_CITY:
                {
                    if (args.Length != 1)
                        throw new ArgumentException("Operation args");
                    else
                        return string.Format("Добавлен город: {0}", args);
                }
                case DbOperation.DB_REMOVE_CITY:
                {
                    if (args.Length != 1)
                        throw new ArgumentException("Operation args");
                    else
                        return string.Format("Удален город: {0}", args);
                }
                case DbOperation.DB_MOD_CITY:
                {
                    if (args.Length != 2)
                        throw new ArgumentException("Operation args");
                    else
                        return string.Format("Изменен город: {0} на {1}", args);
                }
                case DbOperation.DB_ADD_ROUTE:
                {
                    if (args.Length != 6)
                        throw new ArgumentException("Operation args");
                    else
                        return string.Format("Добавлена дорога: {0} - {1} ({2} км, {3} руб/л, {4} км/ч, {5} руб)", args);
                }
                case DbOperation.DB_REMOVE_ROUTE:
                {
                    if (args.Length != 6)
                        throw new ArgumentException("Operation args");
                    else
                        return string.Format("Удалена дорога: {0} - {1} ({2} км, {3} руб/л, {4} км/ч, {5} руб)", args);
                }
                case DbOperation.DB_MOD_ROUTE:
                {
                    if (args.Length != 10)
                        throw new ArgumentException("Operation args");
                    else
                        return string.Format("Изменены параметры дороги {0} - {1}: ({2} км, {3} руб/л, {4} км/ч, {5} руб) на ({6} км, {7} руб/л, {8} км/ч, {9} руб)", args);
                }
                default:
                    throw new ArgumentException("Operation");
            }
        }
        public void ClearLog()
        {
            logoverflow = false;
            if (loaded)
                File.Delete(localpath);
            newdata.Clear();
            position = 0;
        }
    }
    public class AutoParams
    {
        const int SpeedLimit = 250;
        const int FuelSpendLimit = 100; // gallons per mile ~ литров на километр

        int speed, fuel;

        public int MaxSpeed
        {
            get
            {
                return speed;
            }
            set
            {
                if (value > 0 && value <= SpeedLimit) speed = value;
                else throw new ArgumentException(string.Format("MaxSpeed = {2} violates range ({0}; {1}]", 0, SpeedLimit, value));
            }
        }
        public int FuelSpend
        {
            get
            {
                return fuel;
            }
            set
            {
                if (value > 0 && value <= FuelSpendLimit) fuel = value;
                else throw new ArgumentException(string.Format("FuelSpend = {2} violates range ({0}; {1}]", 0, FuelSpendLimit, value));
            }
        }
        public AutoParams(int speed, int spend)
        {
            MaxSpeed = speed;
            FuelSpend = spend;
        }
    }

    public abstract class LogProcessor
    {
        public const int MaxRows = 50;
        protected string TimeFormat;
        protected string[] RowFormats;
        protected static int[] ArgsCounts = { 1, 1, 2, 6, 6, 10 };  
        public abstract void AddRow(DbOperation action, params object[] args);
        public LogProcessor()
        {
        }
        public abstract void ClearLog();
        protected string GetRow(DbOperation action, params object[] args)
        {
            if (args.Length != ArgsCounts[(int)action])
                throw new ArgumentException("Operation args");
            else
                return string.Format(RowFormats[(int)action], args);
        }
    }
    public class LocalLogProcessor : LogProcessor
    {
        RichTextBox LocalLog;
        public override void AddRow(DbOperation action, params object[] args)
        {
            LocalLog.AppendText(string.Format(TimeFormat, DateTime.Now, GetRow(action, args)));
        }
        public override void ClearLog()
        {
            LocalLog.Clear();
        }
        public LocalLogProcessor(RichTextBox source, OuterLogProcessor OutLog)
        {
            LocalLog = source;
            TimeFormat = "{0:dd-MM-yyyy HH:mm:ss} {1}\n";
            RowFormats = new string[] { "Добавлен город: {0}",
                                        "Удален город: {0}",
                                        "Изменен город: {0} на {1}",
                                        "Добавлена дорога: {0} - {1} ({2} км, {3} руб/л, {4} км/ч, {5} руб)",
                                        "Удалена дорога: {0} - {1} ({2} км, {3} руб/л, {4} км/ч, {5} руб)",
                                        "Изменены параметры дороги {0} - {1}: ({2} км, {3} руб/л, {4} км/ч, {5} руб) на ({6} км, {7} руб/л, {8} км/ч, {9} руб)"};
            if (OutLog != null)
            {
                LocalLog.Lines = GetLocalRows(OutLog);
                LocalLog.AppendText("\n");
            }
            else
                LocalLog.Lines = null;
        }
        string TransformRow(string row)
        {
            try
            {
                var tmp = row.Split(',');
                string timestamp = string.Format("{0}-{1}-{2} {3}:{4}:{5} ", tmp.Take(6).ToArray());
                switch (tmp[6])
                {
                    case "add":
                        {
                            if (tmp[7] == "c")
                                return timestamp + GetRow(DbOperation.DB_ADD_CITY, tmp.Skip(8).ToArray());
                            else if (tmp[7] == "r")
                                return timestamp + GetRow(DbOperation.DB_ADD_ROUTE, tmp.Skip(8).ToArray());
                            else
                                return null;
                        }
                    case "del":
                        {
                            if (tmp[7] == "c")
                                return timestamp + GetRow(DbOperation.DB_REMOVE_CITY, tmp.Skip(8).ToArray());
                            else if (tmp[7] == "r")
                                return timestamp + GetRow(DbOperation.DB_REMOVE_ROUTE, tmp.Skip(8).ToArray());
                            else
                                return null;
                        }
                    case "mod":
                        {
                            if (tmp[7] == "c")
                                return timestamp + GetRow(DbOperation.DB_MOD_CITY, tmp.Skip(8).ToArray());
                            else if (tmp[7] == "r")
                                return timestamp + GetRow(DbOperation.DB_MOD_ROUTE, tmp.Skip(8).ToArray());
                            else
                                return null;
                        }
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string[] GetLocalRows(OuterLogProcessor OutLog)
        {
            string[] result = new string[(OutLog.Loaded ? MaxRows : OutLog.Position + 1)];
            int q = 0;
            string z;
            if (OutLog.LogOverflow)
            {
                for (int i = OutLog.Position + 1; i < MaxRows; i++)
                {
                    z = TransformRow(OutLog.data[i]);
                    if (!string.IsNullOrEmpty(z))
                        result[q++] = z;
                }
            }
            for (int i = 0; i <= OutLog.Position; i++)
            {
                z = TransformRow(OutLog.data[i]);
                if (!string.IsNullOrEmpty(z))
                    result[q++] = z;
            }
            return result.TakeWhile(x => !string.IsNullOrEmpty(x)).ToArray();
        }
    }
    public class OuterLogProcessor : LogProcessor
    {
        public bool Loaded { get; }
        public bool LogOverflow { get; private set; }
        string LogPath, LogDefaultFileName;
        public int Position { get; private set; }
        List<string> newdata;
        public string[] data { get; private set; }
        public override void AddRow(DbOperation action, params object[] args)
        {
            newdata.Add(string.Format(TimeFormat, DateTime.Now, GetRow(action, args)));
        }
        public override void ClearLog()
        {
            if (Loaded) File.Delete(LogPath);
            newdata.Clear();
        }
        bool LoadFromFile(string path)
        {
            try
            {
                var LogReader = new StreamReader(LogPath, Encoding.UTF8);
                data = new string[MaxRows];
                int k = 0;
                while (!LogReader.EndOfStream)
                {
                    data[k % MaxRows] = LogReader.ReadLine();
                    k++;
                }
                if (k >= MaxRows) LogOverflow = true;
                k %= MaxRows;
                Position = (k == 0 ? MaxRows - 1 : k - 1);
                LogReader.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void UploadLog()
        {
            if (Loaded)
            {
                var LogWriter = new StreamWriter(LogPath, true, Encoding.UTF8);
                foreach (string row in newdata)
                    LogWriter.WriteLine(row);
                LogWriter.Close();
            }
        }
        public OuterLogProcessor(string path)
        {
            LogDefaultFileName = "routefinder.log";
            LogPath = Path.Combine(path, LogDefaultFileName);
            newdata = new List<string>();
            Loaded = LoadFromFile(LogPath);
            TimeFormat = "{0:dd,MM,yyyy,HH,mm,ss},{1}";
            RowFormats = new string[] { "add,c,{0}",
                                        "del,c,{0}",
                                        "mod,c,{0},{1}",
                                        "add,r,{0},{1},{2},{3},{4},{5}",
                                        "del,r,{0},{1},{2},{3},{4},{5}",
                                        "mod,r,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"};
        }
    }
}   


