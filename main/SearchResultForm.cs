using RouteFinderLibrary;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace main
{
    public partial class SearchResultForm : Form
    {
        WorkDB data;
        MainForm parent;
        Pen PointPen, LinePen;
        Label[][] Annotations_Cities, Annotation_Roads;
        Point[][] Pts;
        Point[] InfoLabelLocation;
        Label NoWayLabel;
        const int CityPointRadius = 10;
        const int CityPointDiameter = CityPointRadius * 2;

        private void AddRouteInfo(TabPage tab, WorkDbRoute route, int index)
        {
            Annotations_Cities[index] = new Label[route.Cities.Count];
            Annotation_Roads[index] = new Label[route.Roads.Count];
            Pts[index] = new Point[route.Cities.Count];

            RouteInfoLabels[index][0].Text = string.Format("Расстояние: {0} км", route.Length);
            RouteInfoLabels[index][1].Text = string.Format("Время: {0} ч {1} мин", route.Time / 60, route.Time % 60);
            RouteInfoLabels[index][2].Text = string.Format("Расходы: {0} руб", route.Cost);
           
            
            for (int i = 0; i < Annotations_Cities[index].Length; i++)
            {
                Annotations_Cities[index][i] = new Label()
                {
                    AutoSize = true,
                    Size = new Size(0, 15),
                    Text = data.Database[data.Finish].Routes[index].Cities[i],
                    Font = new Font("Arial Narrow", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204))),
                    Name = string.Format("RouteCityInfoLabel{0}{1}", index, i),
                    //ForeColor = Color.Transparent,
                    BackColor = Color.Transparent
                };
                tab.Controls.Add(Annotations_Cities[index][i]);
            }

            for (int i = 0; i < Annotation_Roads[index].Length; i++)
            {
                Annotation_Roads[index][i] = new Label()
                {
                    AutoSize = true,
                    Size = new Size(0, 15),
                    Text = (string.IsNullOrEmpty(data.Database[data.Finish].Routes[index].Roads[i].Name) ? "" : data.Database[data.Finish].Routes[index].Roads[i].Name + ", ") + data.Database[data.Finish].Routes[index].Roads[i].Length + " км",
                    Font = new Font("Arial Narrow", 6F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204))),
                    Name = string.Format("RouteRoadInfoLabel{0}{1}", index, i),
                    //ForeColor = Color.Transparent,
                    BackColor = Color.Transparent
                };
                tab.Controls.Add(Annotation_Roads[index][i]);
            }
        }
        private TabPage CreateTab(int index)
        {
            RouteInfoLabels[index] = new Label[3];

            var tmp = new TabPage()
            {
                Text = string.Format("Вариант {0}", index + 1),
                Location = new Point(4, 22),
                Name = string.Format("tabPage{0}", index),
                Size = new Size(SearchResultTabs.Size.Width - 9, SearchResultTabs.Height - 27),
                TabIndex = index + 1,
                UseVisualStyleBackColor = true,
            };
            tmp.Paint += new PaintEventHandler(TabPage_Paint);
            
            for (int j = 0; j < 3; j++)
            {
                RouteInfoLabels[index][j] = new Label()
                {
                    Name = string.Format("RouteSummaryInfoLabel{0}{1}", index, j),
                    Location = InfoLabelLocation[j],
                    Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204))),
                    AutoSize = true,
                    Size = new Size(0, 20),
                    TabIndex = j + 1,
                    BackColor = Color.Transparent
                };
            }

            tmp.Controls.Add(RouteInfoLabels[index][0]);
            tmp.Controls.Add(RouteInfoLabels[index][1]);
            tmp.Controls.Add(RouteInfoLabels[index][2]);

            AddRouteInfo(tmp, data.Database[data.Finish].Routes[index], index);

            return tmp;
        }

        private void SearchNoResult()
        {
            NoResultTab = new TabPage()
            {
                Location = new Point(4, 22),
                Name = "NoResultTab",
                Size = new Size(577, 336),
                TabIndex = 0,
                Text = "Нет маршрутов",
                UseVisualStyleBackColor = true,
            };
            NoResultTab.Paint += new PaintEventHandler(NoResultTab_Paint);
            NoWayLabel = new Label()
            {
                AutoSize = true,
                Name = "NoWayLabel",
                Text = string.Format("Из \"{0}\" нельзя проехать в \"{1}\"", data.Start, data.Finish),
                Location = new Point(15, 25),
                Font = new Font("Arial Narrow", 14F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)))
            };
            NoResultTab.Controls.Add(NoWayLabel);
            SearchResultTabs.Controls.Add(NoResultTab);
        }
        public SearchResultForm(MainForm main, string start, string finish, int crit, int route_count, double disp)
        {
            parent = main;
            data = new WorkDB(parent.Data, parent.YourAuto, start, finish, crit, main.RouteCount, disp);
            InitializeComponent();
            InfoLabelLocation = new Point[3] { new Point(10, 10), new Point(210, 10), new Point(390, 10) };

            if (data.Database.ContainsKey(start) && data.Database.ContainsKey(finish))
            {
                data.FindRoutes(data.Start);
                if (data.Database[data.Finish].Routes.Count > 0)
                {
                    TabPages = new TabPage[data.Database[data.Finish].Routes.Count];
                    RouteInfoLabels = new Label[TabPages.Length][];
                    PointPen = new Pen(Color.Red, 1);
                    LinePen = new Pen(Color.Black, 2);
                    SearchResultTabs.SuspendLayout();
                    Annotations_Cities = new Label[TabPages.Length][];
                    Annotation_Roads = new Label[TabPages.Length][];
                    Pts = new Point[TabPages.Length][];
                    for (int i = 0; i < TabPages.Length; i++)
                    {
                        if (data.Database[data.Finish].Routes[i] != null)
                        {
                            TabPages[i] = CreateTab(i);
                            SearchResultTabs.Controls.Add(TabPages[i]);
                        }
                    }
                    SearchResultTabs.ResumeLayout();
                }
                else
                {
                    SearchNoResult();
                }
            }
            else
                SearchNoResult();
        }

        private void NoResultTab_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.RouteNotFound, (NoResultTab.Width - 450) / 2, 15, 450, 320);
        }
        private void TabPage_Paint(object sender, PaintEventArgs e)
        {
            TabPage p = sender as TabPage;
            int id = int.Parse(p.Name.Substring(7, 1));
            var gr = e.Graphics;
            for (int i = 0; i < Pts[id].Length - 1; i++)
                gr.DrawLine(LinePen, Pts[id][i], Pts[id][i + 1]);
            for (int i = 0; i < data.Database[data.Finish].Routes[id].Cities.Count; i++)
            {
                gr.DrawEllipse(Pens.Red, Pts[id][i].X - CityPointRadius, Pts[id][i].Y - CityPointRadius, CityPointDiameter, CityPointDiameter);
                gr.FillEllipse(Brushes.Red, Pts[id][i].X - CityPointRadius, Pts[id][i].Y - CityPointRadius, CityPointDiameter, CityPointDiameter);
            }
        }

        private void CityLabelPositions(int i)
        {
            for (int j = 0; j < Annotations_Cities[i].Length; j++)
            {
                if (j == 0)
                {
                    if (Pts[i][j + 1].Y < Pts[i][j].Y)
                        Annotations_Cities[i][j].Location = new Point(Math.Max(0, Pts[i][j].X - Annotations_Cities[i][j].Size.Width / 2), Pts[i][j].Y + 5);
                    else
                        Annotations_Cities[i][j].Location = new Point(Math.Max(0, Pts[i][j].X - Annotations_Cities[i][j].Size.Width / 2), Pts[i][j].Y - Annotations_Cities[i][j].Size.Height - 5);
                }
                else if (j == Annotations_Cities[i].Length - 1)
                {
                    if (Pts[i][j - 1].Y < Pts[i][j].Y)
                        Annotations_Cities[i][j].Location = new Point(Math.Min(TabPages[i].Width - Annotations_Cities[i][j].Width, Pts[i][j].X - Annotations_Cities[i][j].Size.Width / 2), Pts[i][j].Y + 5);
                    else
                        Annotations_Cities[i][j].Location = new Point(Math.Min(TabPages[i].Width - Annotations_Cities[i][j].Width, Pts[i][j].X - Annotations_Cities[i][j].Size.Width / 2), Pts[i][j].Y - Annotations_Cities[i][j].Size.Height - 5);
                }
                else
                {
                    if (Pts[i][j].Y <= Pts[i][j - 1].Y && Pts[i][j].Y <= Pts[i][j + 1].Y)
                        Annotations_Cities[i][j].Location = new Point(Pts[i][j].X - Annotations_Cities[i][j].Size.Width / 2, Pts[i][j].Y - Annotations_Cities[i][j].Size.Height - 5);
                    else if (Pts[i][j].Y < Pts[i][j - 1].Y && Pts[i][j].Y > Pts[i][j + 1].Y)
                        Annotations_Cities[i][j].Location = new Point(Pts[i][j].X, Pts[i][j].Y + 5);
                    else if (Pts[i][j].Y >= Pts[i][j - 1].Y && Pts[i][j].Y > Pts[i][j + 1].Y)
                        Annotations_Cities[i][j].Location = new Point(Pts[i][j].X - Annotations_Cities[i][j].Size.Width / 2, Pts[i][j].Y + 5);
                    else if (Pts[i][j].Y > Pts[i][j - 1].Y && Pts[i][j].Y < Pts[i][j + 1].Y)
                        Annotations_Cities[i][j].Location = new Point(Pts[i][j].X, Pts[i][j].Y - Annotations_Cities[i][j].Size.Height - 5);
                }
            }
        }
        private void RoadLabelPositions(int i)
        {
            for (int j = 0; j < Annotation_Roads[i].Length; j++)
            {
                double tg = (Pts[i][j + 1].Y - Pts[i][j].Y) / (Pts[i][j + 1].X - Pts[i][j].X + 0.0);
                Annotation_Roads[i][j].Location = new Point(
                    Pts[i][j].X + (Pts[i][j + 1].X - Pts[i][j].X) / 2,
                    Pts[i][j].Y + (Pts[i][j + 1].Y - Pts[i][j].Y) / 2 + (tg < 0 ? 3 : -(Annotation_Roads[i][j].Height + 3)));
            }
        }
        private void CalculateLabelPositions()
        {
            Random rnd = new Random(DateTime.Now.Second);
            int pad_left = 25;
            int pad_right = 25;
            int pad_top = 50;
            int pad_bottom = 30;
            for (int i = 0; i < Annotations_Cities.Length; i++)
            {
                Point[] tmp_points = new Point[Annotations_Cities[i].Length];
                tmp_points[0] = new Point(pad_left, rnd.Next(pad_top, TabPages[i].Height - pad_bottom));
                for (int j = 1; j < Annotations_Cities[i].Length; j++)
                {
                    int len = data.Database[data.Finish].Routes[i].Roads[j - 1].Length * (TabPages[i].Width - pad_left - pad_right) / data.Database[data.Finish].Routes[i].Length;
                    int angle_up, angle_down;
                    if (tmp_points[j - 1].Y - pad_top < len)
                        angle_up = (int)(Math.Asin((double)(tmp_points[j - 1].Y - pad_top) / len) * 180 / Math.PI);
                    else
                        angle_up = 60;
                    if (TabPages[i].Height - pad_bottom - tmp_points[j - 1].Y < len)
                        angle_down = (int)(Math.Asin((double)(TabPages[i].Height - pad_bottom - tmp_points[j - 1].Y) / len) * -180 / Math.PI);
                    else
                        angle_down = -60;
                    double angle = rnd.Next(angle_down, angle_up) / 180.0 * Math.PI;
                    int cx = tmp_points[j - 1].X + (int)(len * Math.Cos(angle));
                    int cy = tmp_points[j - 1].Y - (int)(len * Math.Sin(angle));
                    tmp_points[j] = new Point(cx, cy);
                }
                double kf = (double)(TabPages[i].Width - pad_left - pad_right) / (tmp_points[tmp_points.Length - 1].X - tmp_points[0].X);
                Pts[i][0] = tmp_points[0];
                for (int j = 1; j < tmp_points.Length; j++)
                    Pts[i][j] = new Point(Pts[i][j - 1].X + (int)((tmp_points[j].X - tmp_points[j - 1].X) * kf), tmp_points[j].Y);
                CityLabelPositions(i);
                RoadLabelPositions(i);
            }
        }

        private void SearchResultForm_Shown(object sender, EventArgs e)
        {
            if (data.Database.ContainsKey(data.Start) && data.Database.ContainsKey(data.Finish))
            {
                if (data.Database[data.Finish].Routes.Count > 0)
                    CalculateLabelPositions();
                else
                    NoWayLabel.Location = new Point((NoResultTab.Width - NoWayLabel.Width) / 2, 10);
            }
            else
                NoWayLabel.Location = new Point((NoResultTab.Width - NoWayLabel.Width) / 2, 10);
        }

        private void SearchResultForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.Enabled = true;
            parent.Focus();
        }
    }
}
