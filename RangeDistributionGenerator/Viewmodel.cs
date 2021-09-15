using LehmerSequenceGenerator;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wpf.Commands;

namespace RangeDistributionGenerator
{
    public enum DistributionAlgorithm { Even, Gauss, Exponential, Gamma, Triangle, Simpson }
    public class Viewmodel : INotifyPropertyChanged
    {
        private const int ValueCount = 200000;
        private const int IntervalCount = 20;
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public ICommand CalculateCommand { get; set; }

        public double Multiplier { get; set; } = 751299;
        public double StartingValue { get; set; } = 871;
        public double Module { get; set; } = 1871341;
        public int SelectedAlgorithm { get; set; } = 0;

        public double MinValue { get; set; } = 0;
        public double MaxValue { get; set; } = 10;

        public double GaussExpectation { get; set; } = 3;
        public double GaussDeviation { get; set; } = 5;

        public int Eta { get; set; } = 2;
        public double Lambda { get; set; } = 4;
        public double MathExpectation { get; set; }
        public double Dispersion { get; set; }
        public double MeanSquareDeviation { get; set; }

        public double XAxisMin { get; set; }
        public double XAxisMax { get; set; }

        public PlotModel PlotModel { get; set;}

        public Viewmodel()
        {
            CalculateCommand = new RelayCommand(() =>
            {
                List<double> sequence = new LehmerAlgorithm(Multiplier, StartingValue, Module).GenerateSequence(ValueCount);

                switch (SelectedAlgorithm)
                {
                    case 0:
                        sequence = Distributions.ToEvenDistribution(sequence,MinValue,MaxValue);
                        break;
                    case 1:
                        sequence = Distributions.ToGaussDistribution(sequence,GaussExpectation,GaussDeviation);
                        break;
                    case 2:
                        sequence = Distributions.ToExponentialDistribution(sequence,Lambda);
                        break;
                    case 3:
                        sequence = Distributions.ToGammaDistribution(sequence,Eta,Lambda);
                        break;
                    case 4:
                        sequence = Distributions.ToTriangleDistribution(sequence,MinValue,MaxValue);
                        break;
                    case 5:
                        sequence = Distributions.ToSimpsonDistribution(sequence, MinValue, MaxValue);
                        break;
                }

                var chars = new Characteristics(sequence);
                MathExpectation = chars.FindMathExpection();
                Dispersion = chars.FindDispersion();
                MeanSquareDeviation = chars.FindMeanSquareDeviation();

                XAxisMin = sequence.Min();
                XAxisMax = sequence.Max();
                var step = (XAxisMax - XAxisMin) /IntervalCount;
                var datapoints = new List<DataPoint>();


                int[] countInIntervals = new int[IntervalCount];
                foreach (double value in sequence)
                {
                    for (int i = 0; i < IntervalCount; i++)
                    {
                        if (value >= XAxisMin + step*i && value <= XAxisMin + step * (i+1))
                        {
                            countInIntervals[i]++;
                        }
                    }
                }
                for (int i = 0; i < countInIntervals.Length; i++)
                {
                    datapoints.Add(new DataPoint(i * step, (double)countInIntervals[i] / ValueCount));
                }
                var plot = new PlotModel { Title = "Гистограмма" };
                plot.Axes.Add(new CategoryAxis { Position = AxisPosition.Bottom, Minimum = -1, Maximum = 20 });
                plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 0.1 });

                plot.Series.Add(new ColumnSeries { ItemsSource = datapoints, ValueField="Y"});

                PlotModel = plot;
            }, null);
        }
    }
}
