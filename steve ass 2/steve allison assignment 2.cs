using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace steve_ass_2
{
    public partial class Form1 : Form
    {
        class row
        {
            public double time;
            public double altitude;
            public double velocity;
            public double acceleration;
          
        }

        List<row> table = new List<row>();

        public Form1()
        {
            InitializeComponent();
        }

        void derivative()

        {
            for (int i = 1; i < table.Count; i++)
            {
                double dh = table[i].altitude - table[i - 1].altitude;
                double dt = table[i].time - table[i - 1].time;
                table[i].velocity = dh / dt;
            }
        }

        void secondderivative()

        {
            for (int i = 1; i < table.Count; i++)
            {
                double dV = table[i].velocity - table[i - 1].velocity;
                double dt = table[i].time - table[i - 1].time;
                table[i].acceleration = dV / dt;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void currentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series1 = new Series
            {
                Name = "Voltage (V)",
                Color = Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };

            chart1.Series.Add(series1);
            for (int i = 0; i < table.Count; i++)
            {
                series1.Points.AddXY(table[i].time, table[i].altitude);
            }
            chart1.ChartAreas[0].AxisX.Title = "time (s)";
            chart1.ChartAreas[0].AxisY.Title = "Altitude (m)";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "csv files |*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            table.Add(new row());
                            string[] r = sr.ReadLine().Split(',');
                            table.Last().time = double.Parse(r[0]);
                            table.Last().altitude = double.Parse(r[1]);


                        }

                    }
                    derivative();
                    secondderivative();
                }
                catch (IOException)
                {
                    MessageBox.Show(openFileDialog1.FileName + "failed to open.");
                }
                catch (FormatException)
                {
                    MessageBox.Show(openFileDialog1.FileName + "is not in the required format.");
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(openFileDialog1.FileName + "is not in the required format.");
                }
            }
        }
    }
}
