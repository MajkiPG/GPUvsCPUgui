using System;
using System.Diagnostics;
using Alea;
using Alea.Parallel;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Threading;

namespace GPUvsCPUgui
{
    public partial class Form1 : Form
    {
        private Computation computation;
        private int maxNumberOfIterations = 100000000;
        private int numberOfSteps = 200;
        private int step;
        private int currentStep = 0;

        private Axis xAxis;
        private Axis yAxis;

        private Thread thread;

        private PlotModel plotModel;
        private LineSeries cpuSeries;
        private LineSeries gpuSeries;

        public Form1()
        {
            InitializeComponent();
            step = maxNumberOfIterations / numberOfSteps;

            plotModel = new PlotModel { Title = "CPU vs GPU" };
            this.plotView1.Model = plotModel;

            xAxis = new LinearAxis { Position = AxisPosition.Bottom };
            yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Compute time [ms]" + System.Environment.NewLine + " ", TitleFontSize = 18 };
            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            cpuSeries = new LineSeries { Color = OxyColor.FromArgb(100, 0, 113, 197) };
            gpuSeries = new LineSeries { Color = OxyColor.FromArgb(100, 116, 183, 27) };
            plotModel.Series.Add(cpuSeries);
            plotModel.Series.Add(gpuSeries);
        }

        private void Start()
        {
            MethodInvoker mi = delegate ()
            {
                plotModel.InvalidatePlot(true);
                stepLabel.Text = "Step " + currentStep + " of 350";
            };

            for (int i = 0; i < 5000; i = i + 100)
            {
                Step(i, mi, 0);
            }

            for (int i = 5000; i < step; i=i+5000)
            {
                Step(i, mi, 5000);
            }

            for (int i = step; i <= maxNumberOfIterations; i = i+step)
            {
                Step(i, mi, step);
            }

            startButton.Text = "START";
        }

        private void Step(int i, MethodInvoker invoker, int startValue)
        {
            computation = new Computation(i);
            long cpuTime = computation.MeasureCPUComputationTime();
            long gpuTime = computation.MeasureGPUComputationTime();

            if (i > startValue)
            {
                cpuSeries.Points.Add(new DataPoint(i, cpuTime));
                gpuSeries.Points.Add(new DataPoint(i, gpuTime));
            }

            currentStep++;
            this.Invoke(invoker);
            Debug.WriteLine("CPU: " + cpuTime + ", GPU:" + gpuTime, " i = " + i);

            computation = null;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (thread == null)
            {
                startButton.Text = "STOP";
                thread = new Thread(Start);
                thread.Start();
            }
            else
            {
                startButton.Text = "START";
                thread.Abort();
                thread = null;
            }
        }

        private void ChangeXAxisType(object sender, EventArgs e)
        {
            if (xAxis is LinearAxis)
            {
                plotModel.Axes.Remove(xAxis);
                xAxis = new LogarithmicAxis { Position = AxisPosition.Bottom };
                plotModel.Axes.Add(xAxis);
            }
            else
            {
                plotModel.Axes.Remove(xAxis);
                xAxis = new LinearAxis { Position = AxisPosition.Bottom };
                plotModel.Axes.Add(xAxis);
            }

            plotModel.InvalidatePlot(true);
        }

        private void ChangeYAxisType(object sender, EventArgs e)
        {
            if (yAxis is LinearAxis)
            {
                plotModel.Axes.Remove(yAxis);
                yAxis = new LogarithmicAxis { Position = AxisPosition.Left, Title = "Compute time [ms]" + System.Environment.NewLine + " ", TitleFontSize = 18 };
                plotModel.Axes.Add(yAxis);
            }
            else
            {
                plotModel.Axes.Remove(yAxis);
                yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Compute time [ms]" + System.Environment.NewLine + " ", TitleFontSize = 18 };
                plotModel.Axes.Add(yAxis);
            }

            plotModel.InvalidatePlot(true);
        }

        private void CloseWindow(object sender, FormClosingEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
            }
        }
    }
}
