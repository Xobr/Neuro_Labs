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
            

        }
        private void button1_Click(object sender, EventArgs e)
        {

            int countOfWindow = int.Parse(textBox5.Text);
            var levels = new int[] { int.Parse(textBox4.Text), 1 };
            neuro = new NeuroSystem(levels, countOfWindow);

              
            var data = Constatnts.GetArray(double.Parse(textBox1.Text), double.Parse(textBox2.Text), double.Parse(textBox3.Text));


            double max = double.MinValue;
            double min = double.MaxValue;

            for (int i = 0; i < data.Length; i++)
            {
                max = Math.Max(max, data[i].Y);
                min = Math.Min(min, data[i].Y); 
            }

            Constatnts._XMax = max;
            Constatnts._XMin = min; 

            double E = double.MaxValue; 



            for (int index = countOfWindow; index < data.Length - 2; index++)
            {

                for (int i = countOfWindow; i <= index; i++)
                {
                    double[] arr = new double[countOfWindow];
                    int k = 0;

                    for (int j = i - countOfWindow; j < i; j++)
                    {
                        arr[k] = Constatnts.TransformerAlong(data[j].Y);
                        k++;
                    }

                    neuro.SetX(arr);

                    double d = (Constatnts.TransformerAlong(data[i + 1].Y));
                    double s = neuro.GetResult();
                    E = Math.Pow(d - s, 2);

                    while (E > Constatnts._Epselon)
                    {
                        neuro.Teach(d, s);
                        s = neuro.GetResult();
                        E = Math.Pow(d - s, 2);
                    }
                }
            }
            

            MessageBox.Show("Done!"); 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            

        }
        private void button3_Click(object sender, EventArgs e)
        {


            Clear();

            var res = Constatnts.GetArray(double.Parse(textBox1.Text), double.Parse(textBox2.Text), double.Parse(textBox3.Text));
            foreach (var elem in res)
            {
                chart1.Series[0].Points.AddXY(elem.X, elem.Y);
                chart1.Series[2].Points.AddXY(elem.X, elem.Y);
            }

            int cp = int.Parse(textBox5.Text);
            double step = double.Parse(textBox3.Text);
            double lastPoint = double.Parse(textBox2.Text) - (cp*step); 
            var data = Constatnts.GetArray(double.Parse(textBox1.Text), double.Parse(textBox2.Text), double.Parse(textBox3.Text));
            var ls = new List<double>();

            for (int i = data.Length - 2 * cp; i < data.Length - cp; i++)
                ls.Add(Constatnts.TransformerAlong( data[i].Y));
            // lastPoint += step; 
            int index = data.Length - cp; 
            for (int i = 0; i < cp; i++)
            {
                neuro.SetX(ls.ToArray());
                double s = neuro.GetResult();
                double real = Constatnts.TransformerBack(s); 
                chart1.Series[1].Points.AddXY(lastPoint ,real);
                chart1.Series[3].Points.AddXY(lastPoint, real);
                ls.Add(s);
                ls.RemoveAt(0);
                lastPoint += step;
                dataGridView1.Rows.Add(i+1,data[index+i].Y.ToString("N4"),real.ToString("N4"),(Constatnts.GetPercError(real, data[index + i].Y)*100).ToString("N2")); 
            }
            this.Refresh();
        }
        void Clear()
        {
            for (int i = 0; i < chart1.Series.Count; i++)
                chart1.Series[i].Points.Clear();
            dataGridView1.Rows.Clear(); 
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Clear(); 
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
