using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO; 

namespace lab1Lub
{
    struct Point
    {
        public double X;
        public double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y; 
        }
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Random r = new Random();
        bool pointInterceptPolygon(Point point,List<Point> polygon) 
        {
            var intercept = false;
            for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i, i++)
                if (((polygon[i].Y <= point.Y&& point.Y < polygon[j].Y) || (polygon[j].Y <= point.Y && point.Y < polygon[i].Y)) && (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                    intercept = !intercept;
            return intercept;
        }
        public double Rec(double x, double y)
        {
            return (x - y) > 0 ? 0.0 : 1.0; 
        }
        NeuroSystem neuro;
        List<Point> ls = new List<Point>() { new Point(10/60d, 30 / 60d), new Point(40 / 60d, 50 / 60d), new Point(50 / 60d, 30 / 60d), new Point(40 / 60d, 10 / 60d), new Point(20 / 60d, 10 / 60d) };
        private void Form1_Load(object sender, EventArgs e)
        {
            //while (true)
            //{
            //    double d = r.Next(-1000,1000);
            //    MessageBox.Show(String.Format("F({0}) = {1}",d.ToString(),Constatnts.ActivationFunk(d).ToString("N3"))); 
            //}
            //neuro = new Neuro(2);
            //neuro.W[0] = 10;
            //neuro.W[1] = 10;
            //chart1.Series[0].Points.AddXY(-100, -100);  
            //chart1.Series[0].Points.AddXY(100, 100);
            //chart1.ChartAreas[0].AxisX.Minimum = -100; 
            //chart1.ChartAreas[0].AxisX.Maximum = 100;

            chart1.Series[0].Points.AddXY(10,30);
            chart1.Series[0].Points.AddXY(40, 50);
            chart1.Series[0].Points.AddXY(50, 30);
            chart1.Series[0].Points.AddXY(40, 10);
            chart1.Series[0].Points.AddXY(20, 10);
            chart1.Series[0].Points.AddXY(10, 30);

            var levels = new int[] { 5,2 ,1 };
            neuro = new NeuroSystem(levels, 3);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<double[]> list = new List<double[]>();

            for (int i = 0; i < 100; i++)
            {
                double x = 6 * Math.Truncate(i / 10d);
                double some = i % 10d;
                double y = 6 * some;
                double[] xArr = new double[] { x/60d, y/60d, 1d };
                list.Add(xArr);

            }
            double E = double.MaxValue;
            int counter = 1;
            while (E > Constatnts._Epselon)
            {
                E = double.MinValue;
                for (int j = 0; j < counter; j++)
                {
                    bool b = pointInterceptPolygon(new Point(list[j][0], list[j][1]), ls);
                    double d;
                    if (b) d = 1;
                    else d = 0;
                    neuro.SetX(list[j]);
                    double s = neuro.GetResult();
                    if (Math.Pow(d - s, 2) / 2d > Constatnts._Epselon)
                    {
                        neuro.Teach(d, s);
                        s = neuro.GetResult();
                    }
                    if (Math.Pow(d - s, 2) / 2d > E)
                        E = Math.Pow(d - s, 2) / 2.0;
                }
                if (counter != list.Count && E < Constatnts._Epselon)
                {
                    counter++;
                    E = double.MaxValue;
                }
            }
            /*
            for (int i = 0; i < list.Count; i++)
            {
                double[] xArr = list[i];
                neuro.SetX(xArr);
                double s = neuro.GetResult();
                bool b = pointInterceptPolygon(new Point(xArr[0], xArr[1]), ls);
                double d;
                if (b) d = 1;
                else d = 0;
                double E = Math.Pow(d - s, 2) / 2;
                neuro.SetX(xArr);
                while (E > Constatnts._Epselon)
                { 
                    neuro.Teach(d, s);
                    s = neuro.GetResult();
                    E = Math.Pow(d - s, 2) / 2;
                }
            }*/
            MessageBox.Show("Done!"); 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //Point p = new Point( int.Parse(textBox1.Text), int.Parse(textBox2.Text)); 
            //neuro.X[0] = double.Parse(textBox1.Text); 
            //neuro.X[1] = double.Parse(textBox2.Text);
            //double res = neuro.GetResult();
            //label1.Text = res.ToString("N3");
            //bool res = pointInterceptPolygon(p, ls); 
            //DataPoint dp = new DataPoint(p.X, p.Y);
            //if (res)
            //    dp.Color = System.Drawing.Color.Red;
            //else dp.Color = System.Drawing.Color.Green;

            //chart1.Series[1].Points.Add(dp);
            //this.Refresh(); 
        }
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                double[] x = new double[] { r.Next(0, 70) / 60d, r.Next(0, 70) / 60d, 1d };
                neuro.SetX(x);

                double res = neuro.GetResult();
                //  label1.Text = res.ToString("N3");
                DataPoint dp = new DataPoint(x[0] * 60.0, x[1] * 60.0);
                double d;
                bool b = pointInterceptPolygon(new Point(x[0], x[1]), ls);
                if (b) d = 1;
                else d = 0;
                dp.Color = System.Drawing.Color.Orange;
                if (Math.Pow(res - 1.0, 2) / 2.0 <= Constatnts._Epselon)
                    dp.Color = System.Drawing.Color.Green;
                if (Math.Pow(res - 0.0, 2) / 2.0 <= Constatnts._Epselon)
                    dp.Color = System.Drawing.Color.Red;
                /*
                else if (res > 0.5)
                    dp.Color = System.Drawing.Color.Red;
                else dp.Color = System.Drawing.Color.Green;*/
                chart1.Series[1].Points.Add(dp);
            }
            this.Refresh();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            chart1.Series[1].Points.Clear();
            this.Refresh(); 
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return; 
            neuro.VriteW(saveFileDialog1.FileName); 
            //using (var sw = new StreamWriter(saveFileDialog1.FileName))
            //{
            //    sw.Write(neuro.ToString()); 
            //}
        }
        double Parse(string str)
        {
            double res;
            if (!double.TryParse(str,out res))
            {
                if (str.Contains(',')) str = str.Replace(',', '.'); 
                else str = str.Replace('.', ',');
                res = double.Parse(str); 
            }

            return res; 
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            List<List<double[]>> ls = new List<List<double[]>>();
            using (var sr = new StreamReader(openFileDialog1.FileName))
            {
                while (sr.Peek() >= 0)
                {
                    var crrLs = (new List<double[]>());
                    string[] line = sr.ReadLine().Split(';');
                    for (int i = 0; i < line.Length; i++)
                    {
                        double[] dArr = Array.ConvertAll(line[i].Split(','), Parse);

                        crrLs.Add(dArr);
                    }
                    ls.Add(crrLs);
                }
            }

            for (int i = 0; i < ls.Count; i++)
                for (int j = 0; j < ls[i].Count; j++)
                    neuro.neurons[i][j].W = ls[i][j];

        }
    }
}
