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
    public struct Point
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

        public double[] getT(Point p)
        {
            List<Point> m1 = new List<Point>() {new Point(1,2), new Point(1, 4), new Point(2, 4), new Point(2, 2) }; 
            List<Point> m2 = new List<Point>() {new Point(3,2), new Point(3, 4), new Point(4, 4), new Point(4, 2) }; 
            List<Point> m3 = new List<Point>() {new Point(5,2), new Point(5, 4), new Point(6, 4), new Point(6, 2) };

            return new double[] { pointInterceptPolygon(p, m1).GetInt(), pointInterceptPolygon(p, m2).GetInt(), pointInterceptPolygon(p, m3).GetInt() }; 
        } 

        public double Rec(double x, double y)
        {
            return (x - y) > 0 ? 0.0 : 1.0; 
        }
        RBFNeuroSystem neuro;
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

            //chart1.Series[0].Points.AddXY(10,30);
            //chart1.Series[0].Points.AddXY(40, 50);
            //chart1.Series[0].Points.AddXY(50, 30);
            //chart1.Series[0].Points.AddXY(40, 10);
            //chart1.Series[0].Points.AddXY(20, 10);
            //chart1.Series[0].Points.AddXY(10, 30);

            //var levels = new int[] { 5,2 ,1 }; 

        }
        private void button1_Click(object sender, EventArgs e)
        {

            int first = 30;
            int second = 3;
            neuro = new RBFNeuroSystem(first * 3, second);
            int i = 0;
            List<double[]> ls = new List<double[]>(); 

            for (; i < first; i++)
            {
                var nr = (RBFNeuron)neuro.Neurons[0][i];
                nr.X = new double[] {Constatnts.GetDouble(1,2),Constatnts.GetDouble(2,4) };
                ls.Add(nr.X); 
            }

            for ( ; i < 2 * first; i++)
            {
                var nr = (RBFNeuron)neuro.Neurons[0][i];
                nr.X = new double[] { Constatnts.GetDouble(3, 4), Constatnts.GetDouble(2, 4) };
                ls.Add(nr.X);
            }

            for ( ; i < 3 * first; i++)
            {
                var nr = (RBFNeuron)neuro.Neurons[0][i];
                nr.X = new double[] { Constatnts.GetDouble(5,6), Constatnts.GetDouble(2, 4) };
                ls.Add(nr.X);
            }

            int counter = 1; 
            double E =double.MaxValue ;
            while (E > Constatnts._Epselon)
            {
                for (int j = 0; j < counter; j++)
                {
                    while (E > Constatnts._Epselon)
                    {
                        neuro.Teach(ls[j], getT(new Point(ls[j][0], ls[j][1])));
                        E = Constatnts.GetE( neuro.GetOut(), getT(new Point(ls[j][0], ls[j][1])));

                    }
                    E = double.MaxValue;
                }
                if (counter != ls.Count)
                {
                    counter++;
                    E = double.MaxValue;
                }
                else
                {
                    E = double.MinValue;
                }
            }
             


            MessageBox.Show("Done!"); 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Point p = new Point( int.Parse(textBox1.Text), int.Parse(textBox2.Text)); 
            //neuro.X[0] = double.Parse(textBox1.Text); 
            //neuro.X[1] = double.Parse(textBox2.Text);
            //double res = neuro.GetResult();
            //label1.Text = res.ToString("N3");
            bool res = pointInterceptPolygon(p, ls); 
            DataPoint dp = new DataPoint(p.X, p.Y);
            if (res)
                dp.Color = System.Drawing.Color.Red;
            else dp.Color = System.Drawing.Color.Green;

            chart1.Series[1].Points.Add(dp);
            this.Refresh(); 
        }
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                Point P = new Point(Constatnts.GetDouble(0, 7), Constatnts.GetDouble(1, 5));
                double[] x = new double[] { P.X, P.Y };
                neuro.SetX(x); 
                var res = neuro.GetOut();

                DataPoint dp = new DataPoint(P.X, P.Y);
                if (res[0] > 0.9)
                    dp.Color = System.Drawing.Color.Red;
                else if (res[1] > 0.9)
                    dp.Color = System.Drawing.Color.Green;
                else if (res[2] > 0.9)
                    dp.Color = System.Drawing.Color.Blue;
                else dp.Color = System.Drawing.Color.Orange;

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
            //if (saveFileDialog1.ShowDialog() != DialogResult.OK) return; 
            //neuro.VriteW(saveFileDialog1.FileName); 
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
            //if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            //List<List<double[]>> ls = new List<List<double[]>>();
            //using (var sr = new StreamReader(openFileDialog1.FileName))
            //{
            //    while (sr.Peek() >= 0)
            //    {
            //        var crrLs = (new List<double[]>());
            //        string[] line = sr.ReadLine().Split(';');
            //        for (int i = 0; i < line.Length; i++)
            //        {
            //            double[] dArr = Array.ConvertAll(line[i].Split(','), Parse);

            //            crrLs.Add(dArr);
            //        }
            //        ls.Add(crrLs);
            //    }
            //}

            //for (int i = 0; i < ls.Count; i++)
            //    for (int j = 0; j < ls[i].Count; j++)
            //        neuro.neurons[i][j].W = ls[i][j];

        }
    }
}
