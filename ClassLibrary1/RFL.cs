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
        public string Name { get; }
        public RouteParams(int length, int fcost, int speed, int addcosts, string RoadName = "")
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
    }
    public class __RoadParams : IComparable<__RoadParams>
    {
        public int Length { get; }
        public int Time { get; }
        public int Cost { get; }
        public string Name { get; }
        public RouteCriteria Criteria { get; }
        public __RoadParams(RouteParams road, AutoParams auto, RouteCriteria crit)
        {
            Criteria = crit;
            Length = road.Length;
            Time = (int)(road.Length * 60.0 / Math.Min(road.SpeedLimit, auto.MaxSpeed));
            Cost = (int)(Length * auto.FuelSpend * road.FuelCost / 100) + road.AddCosts + 1;
            Name = road.Name;
        }
        public int CompareTo(__RoadParams obj)
        {
            if (obj == null) return -1;
            else
            {
                switch (Criteria)
                {
                    case RouteCriteria.MIN_LENGTH: return Length - obj.Length;
                    case RouteCriteria.MIN_TIME: return Time - obj.Time;
                    case RouteCriteria.MIN_COST: return Cost - obj.Cost;
                    default: return 0;
                }
            }
        }
    }

    public enum RFLActionResult
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
        ROUTE_NOT_CHANGED,
        START_CITY_EMPTY, 
        END_CITY_EMPTY,
        AUTO_PARAMS_NOT_DEFINED,
        DATABASE_IS_EMPTY,
        ROUTE_CRITERIA_NOT_SELECTED
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
    public enum RouteCriteria
    {
        MIN_LENGTH, MIN_TIME, MIN_COST
    }   

    public class RFDatabase
    {
        SortedSet<string> Cities;
        Dictionary<Tuple<string, string>, List<RouteParams>> Routes;
        public bool DbLoaded { get; private set; }
        int RoutesCount;
        string DbPath;
        public string DbDefaultFileName  { get; }

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
        public RFLActionResult AddCity(string city)
        {
            if (string.IsNullOrEmpty(city)) return RFLActionResult.EMPTY_CITY;
            else
                if (!CheckCityName(city)) return RFLActionResult.INVALID_CITY_NAME;
            else
                return (Cities.Add(city) ? RFLActionResult.OK : RFLActionResult.CITY_ALREADY_EXISTS);
        }
        public RFLActionResult AddRoute(string source, string dest, int len, int cost, int speed, int addcost)
        {
            int d = source.CompareTo(dest);
            if (d == 0) return RFLActionResult.ROUTE_SOURCE_EQUALS_DEST;
            if (!Cities.Contains(source) || !Cities.Contains(dest)) return RFLActionResult.SOURCE_OR_DEST_CITY_NOT_EXISTS;
            if (!CheckValues(len, cost, speed, addcost)) return RFLActionResult.INVALID_VALUES;
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
                    return RFLActionResult.OK;
                }
                else
                {
                    if (!Routes[key].Exists(x => x.Length == len && x.FuelCost == cost && x.AddCosts == addcost && x.SpeedLimit == speed))
                    {
                        Routes[key].Add(new RouteParams(len, cost, speed, addcost));
                        RoutesCount++;
                        return RFLActionResult.OK;
                    }
                    else
                        return RFLActionResult.SAME_ROUTE_ALREADY_EXISTS;
                }
            }
        }
        public RFLActionResult DeleteCity(string city)
        {
            if (string.IsNullOrEmpty(city)) return RFLActionResult.EMPTY_CITY;
            else
            {
                foreach (var key in Routes.Where(x => x.Key.Item1 == city || x.Key.Item2 == city).ToList())
                {
                    Routes.Remove(key.Key);
                    RoutesCount -= key.Value.Count;
                }
                return (Cities.Remove(city) ? RFLActionResult.OK : RFLActionResult.CITY_NOT_EXISTS);
            }
        }
        public RFLActionResult DeleteRoute(CRoute route, bool all)
        {
            Tuple<string, string> key = new Tuple<string, string>(route.FirstCity, route.SecondCity);
            if (!Routes.ContainsKey(key))
                return RFLActionResult.ROUTE_NOT_EXISTS;
            else
            {
                if (all)
                {
                    RoutesCount -= Routes[key].Count;
                    return (Routes.Remove(key) ? RFLActionResult.OK : RFLActionResult.ROUTE_NOT_EXISTS);
                }
                else
                {
                    RoutesCount--;
                    return (Routes[key].RemoveAll(x => x.Length == route.Length && x.SpeedLimit == route.SpeedLimit && x.FuelCost == route.FuelCost && x.AddCosts == route.AddCosts) > 0 ? RFLActionResult.OK : RFLActionResult.ROUTE_NOT_EXISTS);
                }
            }
        }
        public RFLActionResult ModCity(string city, string newcity)
        {
            if (city == newcity) return RFLActionResult.CITYNAME_NOT_CHANGED;
            if (!Cities.Contains(city))
                return RFLActionResult.CITY_NOT_EXISTS;
            else if (Cities.Contains(newcity))
                return RFLActionResult.INVALID_NEW_CITYNAME;
            else
                return (Cities.Add(newcity) && Cities.Remove(city) ? RFLActionResult.OK : RFLActionResult.INVALID_VALUES);
        }
        public RFLActionResult ModRoute(CRoute route, int newlen, int newfcost, int newspeed, int newaddcost)
        {
            int id;
            if (route == null) return RFLActionResult.ROUTE_NOT_EXISTS;
            var key = new Tuple<string, string>(route.FirstCity, route.SecondCity);
            if (!Routes.ContainsKey(key))
                return RFLActionResult.ROUTE_NOT_EXISTS;
            else if ((id = Routes[key].FindIndex(x => x.Length == route.Length && x.SpeedLimit == route.SpeedLimit && x.FuelCost == route.FuelCost && x.AddCosts == route.AddCosts)) < 0)
                return RFLActionResult.ROUTE_NOT_EXISTS;
            else if (!CheckValues(newlen, newfcost, newspeed, newaddcost))
                return RFLActionResult.INVALID_VALUES;
            else
            {
                var tmp = new RouteParams(newlen, newfcost, newspeed, newaddcost);
                if (tmp.Equals(route.Options)) return RFLActionResult.ROUTE_NOT_CHANGED;
                int k = Routes[key].FindIndex(x => x.Length == tmp.Length && x.SpeedLimit == tmp.SpeedLimit && x.FuelCost == tmp.FuelCost && x.AddCosts == tmp.AddCosts);
                if (k >= 0) return RFLActionResult.SAME_ROUTE_ALREADY_EXISTS;
                else
                {
                    Routes[key][id] = tmp;
                    return RFLActionResult.OK;
                }
            }
        }
        public RFLActionResult DbLoad(string path)
        {
            string ptr;
            if (string.IsNullOrEmpty(path))
                return RFLActionResult.DBFILE_NOT_FOUND;
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
                return RFLActionResult.OK;
            }
            catch (FileNotFoundException)
            {
                DbLoaded = false;
                return RFLActionResult.DBFILE_NOT_FOUND;
            }
            catch (FormatException)
            {
                DbLoaded = false;
                return RFLActionResult.INVALID_FILE_FORMAT;
            }
            catch (ArgumentException)
            {
                DbLoaded = false;
                return RFLActionResult.DBFILE_NOT_FOUND;
            }
            catch (DirectoryNotFoundException)
            {
                DbLoaded = false;
                return RFLActionResult.DBFILE_NOT_FOUND;
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
                return string.Format("{6}{0} - {1} ({2} км, {3} руб/л, {4} км/ч, {5} руб)", FirstCity, SecondCity, Options.Length, Options.FuelCost, Options.SpeedLimit, Options.AddCosts, (string.IsNullOrEmpty(Options.Name) ? "" : Options.Name + ": "));
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

        int speed;
        double fuel;

        public int MaxSpeed
        {
            get { return speed; }
            set
            {
                if (value > 0 && value <= SpeedLimit)
                    speed = value;
                else
                    throw new ArgumentException(string.Format("MaxSpeed = {1} violates range (0; {0}]", SpeedLimit, value), "AutoSpeed");
            }
        }
        public double FuelSpend
        {
            get
            {
                return fuel;
            }
            set
            {
                if (value > 0 && value <= FuelSpendLimit)
                    fuel = value;
                else
                    throw new ArgumentException(string.Format("FuelSpend = {1} violates range (0; {0}]", FuelSpendLimit, value), "FuelSpend");
            }
        }
        public AutoParams(int speed, double spend)
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

    public class WorkDbRoute
    {
        public List<string> Cities { get; }
        public List<__RoadParams> Roads { get; }
        public int Length { get; set; }
        public int Time { get; set; }
        public int Cost { get; set; }

        public WorkDbRoute()
        {
            Cities = new List<string>();
            Roads = new List<__RoadParams>();
            Length = 0;
            Time = 0;
            Cost = 0;
        }
        public WorkDbRoute(WorkDbRoute source, Tuple<string, __RoadParams> road)
        {
            Cities = new List<string>(source.Cities);
            Roads = new List<__RoadParams>(source.Roads);
            Cities.Add(road.Item1);
            Roads.Add(road.Item2);
            Length = source.Length + road.Item2.Length;
            Cost = source.Cost + road.Item2.Cost;
            Time = source.Time + road.Item2.Time;
        }
        public int CompareTo(WorkDbRoute other, __RoadParams road)
        {
            int d = 0;
            switch (road.Criteria)
            {
                case RouteCriteria.MIN_COST:
                    d = Cost - (other.Cost + road.Cost); break;
                case RouteCriteria.MIN_LENGTH:
                    d = Length - (other.Length + road.Length); break;
                case RouteCriteria.MIN_TIME:
                    d = Time - (other.Time + road.Time); break;
            }
            if (d != 0) return d;
            else
                if (CompareCities(other, road)) return 0;
                else return -1;
        }
        bool CompareCities(WorkDbRoute other, __RoadParams road)
        {
            if (Cities.Count != other.Cities.Count + 1) return false;
            else
            {
                bool result = true;
                for (int i = 0; i < Cities.Count - 1; i++)
                    result &= (Cities[i] == other.Cities[i]);
                if (result)
                {
                    for (int i = 0; i < Roads.Count - 1; i++)
                        result &= (Roads[i] == other.Roads[i]);
                    result &= (Roads[Roads.Count - 1] == road);
                }
                return result;
            }
        }
        bool CheckDisp(WorkDbRoute other, __RoadParams road, double disp)
        {
            if (Cities == null || Cities.Count == 0) return true;
            else
                switch (road.Criteria)
                {
                    case RouteCriteria.MIN_COST:
                        return (Cost * (1 + disp) >= other.Cost + road.Cost);
                    case RouteCriteria.MIN_LENGTH:
                        return (Length * (1 + disp) >= other.Length + road.Length);
                    case RouteCriteria.MIN_TIME:
                        return (Time * (1 + disp) >= other.Time + road.Time);
                    default:
                        return false;
                }
        }
        bool NoCycle(string city)
        {
            return Cities.Contains(city);
        }
        public bool CheckRoute(WorkDbRoute other, Tuple<string, __RoadParams> road, double disp)
        {
            return NoCycle(road.Item1) && other.CheckDisp(this, road.Item2, disp);
        }
    }

    public class WorkDbItem
    {
        public bool Used;
        int RouteCount;
        double Dispersion;
        public List<WorkDbRoute> Routes { get; private set; }
        public List<Tuple<string, __RoadParams>> Roads { get; private set; }

        public WorkDbItem(int rc, double disp)
        {
            Routes = new List<WorkDbRoute>();
            Roads = new List<Tuple<string, __RoadParams>>();
            Used = false;
            RouteCount = rc;
            Dispersion = disp;
        }

        public bool MergeRoutes(WorkDbItem other, Tuple<string, __RoadParams> road)
        {
            int i = 0, j = 0;
            var tmp = new List<WorkDbRoute>();
            bool merged = false;
            int d;
            while(i < Routes.Count && j < other.Routes.Count && tmp.Count < RouteCount)
            {
                d = Routes[i].CompareTo(other.Routes[j], road.Item2);
                if (d < 0)
                {
                    tmp.Add(Routes[i]);
                    i++;
                }
                else if (d > 0)
                {
                    if (tmp.Count == 0 || other.Routes[j].CheckRoute(tmp[0], road, Dispersion))
                    {
                        tmp.Add(new WorkDbRoute(other.Routes[j], road));
                        merged = true;
                    }
                    j++;
                }
                else if (d == 0)
                {
                    tmp.Add(Routes[i]);
                    i++;
                    j++;
                }
            }
            while (tmp.Count < RouteCount)
            {
                if (i < Routes.Count)
                {
                    tmp.Add(Routes[i]);
                    i++;
                }
                else
                if (j < other.Routes.Count)
                {
                    if (tmp.Count == 0 || other.Routes[j].CheckRoute(tmp[0], road, Dispersion))
                    {
                        tmp.Add(new WorkDbRoute(other.Routes[j], road));
                        merged = true;
                    }
                    j++;
                }
                else break;
            }
            Routes = tmp;
            return merged;
        }
    }

    public class WorkDB
    {
        public string Start { get; }
        public string Finish { get; }

        public Dictionary<string, WorkDbItem> Database { get; }

        public WorkDB(RFDatabase source, AutoParams auto, string FirstCity, string SecondCity, int criteria, int route_count, double disp)
        {
            Start = FirstCity;
            Finish = SecondCity;
            Database = new Dictionary<string, WorkDbItem>();
            foreach(var rt in source.GetRoutesList())
            {
                var tmp = new __RoadParams(rt.Options, auto, (RouteCriteria)criteria);
                if (!Database.ContainsKey(rt.FirstCity))
                    Database.Add(rt.FirstCity, new WorkDbItem(route_count, disp));
                Database[rt.FirstCity].Roads.Add(new Tuple<string, __RoadParams>(rt.SecondCity, tmp));
                if (!Database.ContainsKey(rt.SecondCity))
                    Database.Add(rt.SecondCity, new WorkDbItem(route_count, disp));
                Database[rt.SecondCity].Roads.Add(new Tuple<string, __RoadParams>(rt.FirstCity, tmp));
            }
            foreach (var row in Database)
                row.Value.Roads.Sort(delegate (Tuple<string, __RoadParams> x, Tuple<string, __RoadParams> y)
                 {
                     if (x == null && y == null) return 0;
                     else if (x == null) return 1;
                     else if (y == null) return -1;
                     else return x.Item2.CompareTo(y.Item2);
                 });
        }

        public void FindRoutes(string city)
        {
            if (city == Start)
            {
                Database[Start].Routes.Add(new WorkDbRoute());
                Database[Start].Routes[0].Cities.Add(Start);
            }
            if (city == Finish)
                return;
            else
            {
                foreach (var road in Database[city].Roads)
                {
                    if (!Database[road.Item1].Used)
                        if (Database[road.Item1].MergeRoutes(Database[city], road))
                        {
                            Database[city].Used = true;
                            FindRoutes(road.Item1);
                            Database[city].Used = false;
                        }
                }
            }
        }
    }
}   


