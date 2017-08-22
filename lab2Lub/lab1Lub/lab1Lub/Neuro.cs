using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1Lub
{
    public class RBFNeuron
    {
        public double[] X;
        public double[] C; 
        public double GetSum()
        {
            double sum = 0;
            for (int i = 0; i < X.Length; i++)
                sum += (Math.Pow(X[i] - C[i],2) / Constatnts.Sigma[i]);

            return Math.Exp(-0.5 * sum); 
        }

    }
    public class Neuro
    {
        public double[] W;
        public double[] X;
        public double Alpha;
        public int n;
        
        public Neuro(int n)
        {
            this.n = n;
            W = new double[n];
            X = new double[n];
            for (int i = 0; i < n; i++)
            {
                W[i] = Constatnts.r.NextDouble()/100d; 
            }
        }

        public Neuro() { }

        public double GetSum()
        {
            double sum = 0;
            for (int i = 0; i < n; i++)
                sum += W[i] * X[i];

            return sum;
        }

        public double Sum
        {
            get { return GetSum(); }
        }

        public double GetOUT(double s)
        {
            return Constatnts.ActivationFunk(s);
        }

        public double OUT
        {
            get { return GetOUT(Sum); }
        }

        public double GetAlpha(double OUT, double T)
        {
            return (OUT - T) * OUT * (1 - OUT);
        }

        public double GetResult()
        {
            double sum = GetSum();
            double res = GetOUT(sum);
            return res;
        }

        public void Teach(double trth)
        {

            double sum = GetSum();
            double OUT = GetOUT(sum);
            double alpha = GetAlpha(OUT, trth);
            for (int i = 0; i < n; i++)
            {
                double dw = -Constatnts._Teta * alpha * X[i];
                W[i] = W[i] + dw;
            }
        }

        public void Teach(double trth, double alpha)
        {
            Alpha = alpha;
            for (int i = 0; i < n; i++)
            {
                double dw = -Constatnts._Teta * alpha * X[i];
                W[i] = W[i] + dw;
            }
        }

        public override string ToString()
        {
            string str = null;
            str += W[0].ToString().Replace(',', '.');
            for (int i = 1; i < n; i++)
                str += "," + W[i].ToString().Replace(',','.');

            return str;
        }

    }
    public class NeuroSystem
    {
        public Neuro[][] neurons;
        public int[] levels;
        Random random = new Random();

        public NeuroSystem(int[] levels, int input)
        {

            //this.levels = new int[] { 5, 2, 1 };
            this.levels = levels;
            neurons = new Neuro[levels.Length][];
            for (int i = 0; i < levels.Length; i++)
            {
                neurons[i] = new Neuro[levels[i]];
                for (int j = 0; j < levels[i]; j++)
                    neurons[i][j] = new Neuro();
            }
            for (int i = levels.Length - 1; i >= 1; i--)
            {
                for (int j = 0; j < levels[i]; j++)
                {
                    neurons[i][j].n = levels[i - 1];
                    neurons[i][j].X = new double[levels[i - 1]];
                    neurons[i][j].W = new double[levels[i - 1]];
                    //neurons[i][j].Alpha = 1.0;
                    for (int k = 0; k < neurons[i][j].W.Length; k++) neurons[i][j].W[k] = random.NextDouble();
                }
            }


            for (int j = 0; j < levels[0]; j++)
            {
                neurons[0][j].n = input;
                neurons[0][j].X = new double[input];
                neurons[0][j].W = new double[input];
                //neurons[0][j].Alpha = 1.0; 
                for (int k = 0; k < neurons[0][j].W.Length; k++) neurons[0][j].W[k] = random.NextDouble();
            }


        }

        public void SetX(double[] x)
        {
            for (int i = 0; i < levels[0]; i++)
                neurons[0][i].X = (double[])x.Clone();
        }

        public double GetResult()
        {
            for (int i = 1; i < levels.Length; i++)
            {
                double[] x = new double[levels[i - 1]];
                for (int j = 0; j < levels[i - 1]; j++)
                    x[j] = neurons[i - 1][j].OUT;

                for (int k = 0; k < levels[i]; k++)
                {
                    if (neurons[i][k].X.Length != x.Length) throw new Exception();
                    neurons[i][k].X = (double[])x.Clone();
                }
            }

            return neurons[levels.Length - 1][0].OUT;

        }

        public void Teach(double T)
        {
            double OUT = GetResult();
            Teach(T, OUT); 
        }

        public void Teach(double T, double OUT)
        { 
            double alpha = (OUT - T) * OUT * (1d - OUT);
            neurons[levels.Length - 1][0].Teach(T, alpha);

            for (int i = levels.Length - 2; i >= 0; i--)
            {
                for (int k = 0; k < levels[i]; k++)
                {
                    var nr = neurons[i][k];
                    double currentAlp = nr.OUT * (1 - nr.OUT);
                    double sum = 0;
                    for (int index = 0; index < neurons[i + 1].Length; index++)
                        sum += neurons[i + 1][index].Alpha * neurons[i + 1][index].W[k];
                    nr.Alpha = currentAlp * sum;
                    nr.Teach(T, nr.Alpha);
                }
            }
        }

        public void VriteW(string path)
        {
            using (var sw = new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < levels.Length; i++)
                {
                    sw.Write(neurons[i][0].ToString());
                    for (int j = 1; j < levels[i]; j++)
                    {
                        sw.Write(";" + neurons[i][j].ToString());
                    }
                    sw.WriteLine();
                }
            }
        }

        public override string ToString()
        {
            string res = null; 
            for (int i = 0; i < levels.Length; i++)
            {
                res += neurons[i][0].ToString(); 
                for (int j = 1; j < levels[i]; j++)
                {
                    res+= ";" + neurons[i][j].ToString();
                }
                res += "/n"; 
            }

            return res; 
        }
    }

    public class RBFNeuroSystem
    {
        public Object[][] Neurons;

        public void SetX(double[] X)
        {
            for (int i = 0; i < Neurons[0].Length; i++)
            {
                ((RBFNeuron)Neurons[0][i]).X = X; 
            }
        }
        public RBFNeuroSystem(int first,int second)
        {
            Neurons = new object[2][];
            Neurons[0] = new RBFNeuron[first];
            Neurons[1] = new Neuro[second];
            for (int i = 0; i < first; i++)
            {
                Neurons[0][i] = new RBFNeuron(); 
            }
            for (int i = 0; i < second; i++)
            {

                Neurons[1][i] = new Neuro(first);
            }
        }         
        public void Teach(double[] X,double[] T)
        {
            double[] W = new double[Neurons[0].Length];
            for (int i = 0; i < W.Length; i++)
            {
                var rbf = (RBFNeuron)Neurons[0][i];
                rbf.C = X; 
                W[i] = rbf.GetSum(); 
            }

            for (int i = 0; i < Neurons[1].Length; i++)
            {
                var neuro = (Neuro)Neurons[1][i];
                neuro.X = W;  
                neuro.Teach(T[i]); 
            }
        }
        public double[] GetOut()
        {
            double[] W = new double[Neurons[0].Length];
            for (int i = 0; i < W.Length; i++)
            {
                var rbf = (RBFNeuron)Neurons[0][i]; 
                W[i] = rbf.GetSum();
            }

            for (int i = 0; i < Neurons[1].Length; i++)
            {
                var neuro = (Neuro)Neurons[1][i];
                neuro.X = W;
            }


            double[] res = new double[3];
            for (int i = 0; i < Neurons[1].Length; i++)
            {
                var nr = (Neuro)Neurons[1][i];
                res[i] = nr.OUT; 
            }
            return res; 
        }
    }
}
