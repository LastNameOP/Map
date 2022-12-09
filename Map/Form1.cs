using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsPresentation;
using System.Windows;
using GMap.NET;
using GMapMarker = GMap.NET.WindowsForms.GMapMarker;
using GMap.NET.MapProviders;
using MessageBox = System.Windows.Forms.MessageBox;
using GMapRoute = GMap.NET.WindowsForms.GMapRoute;
using DocumentFormat.OpenXml.Drawing;

namespace Map
{
    public partial class Form1 : Form
    {
        const int earthRadius = 6371;
        int p = -1;
        double[] coords = new double[4];
        public Form1()
        {
            InitializeComponent();
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            gMapControl1.MinZoom = 2;
            gMapControl1.MaxZoom = 22;
            gMapControl1.Zoom = 4;
            gMapControl1.Position = new GMap.NET.PointLatLng(56.1366, 40.3966);
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.ViewCenter;
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.ShowCenter = true;
            gMapControl1.ShowTileGridLines = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(numericUpDown1.Text);
            double y = Convert.ToDouble(numericUpDown2.Text);
            gMapControl1.Position = new GMap.NET.PointLatLng(x, y);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            gMapControl1.Overlays.Clear();
            p = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox3.Text = "Широта: " + gMapControl1.Position.Lat.ToString("0.00") + " Долгота: " + gMapControl1.Position.Lng.ToString("0.00");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (p == 3)
            {
                MessageBox.Show("Маркеры уже поставлены");
                return;
            }
            coords[p + 1] = gMapControl1.Position.Lat;
            coords[p + 2] = gMapControl1.Position.Lng;
            PointLatLng point = new PointLatLng(coords[p + 1], coords[p + 2]);
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.green);
            GMapOverlay markers = new GMapOverlay("markers");
            markers.Markers.Add(marker);
            gMapControl1.Overlays.Add(markers);
            p+=2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double value = distanceEarth(coords[0], coords[1], coords[2], coords[3]);
            textBox4.Text = value.ToString("0.00") + " Км";
        }

        double deg2rad(double deg)
        {
            return (deg * Math.PI / 180);
        }
        double rad2deg(double rad)
        {
            return (rad * 180 / Math.PI);
        }
        double distanceEarth(double lat1d, double lon1d, double lat2d, double lon2d)
        {
            double lat1r, lon1r, lat2r, lon2r, u, v;
            lat1r = deg2rad(lat1d);
            lon1r = deg2rad(lon1d);
            lat2r = deg2rad(lat2d);
            lon2r = deg2rad(lon2d);
            u = Math.Sin((lat2r - lat1r) / 2);
            v = Math.Sin((lon2r - lon1r) / 2);
            return 2.0 * earthRadius * Math.Asin(Math.Sqrt(u * u + Math.Cos(lat1r) * Math.Cos(lat2r) * v * v));
        }
    }
}