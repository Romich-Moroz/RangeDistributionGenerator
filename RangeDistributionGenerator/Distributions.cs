using System;
using System.Collections.Generic;
using System.Text;

namespace RangeDistributionGenerator
{
    public static class Distributions
    {
        private const int GaussN = 6;
        public static List<double> ToEvenDistribution(List<double> values, double min, double max)
        {
            var result = new List<double>();
            for (int i = 0; i < values.Count; i++)
            {
                result.Add(min + (max - min) * values[i]);
            }
            return result;
        }

        public static List<double> ToGaussDistribution(List<double> values, double expectation, double deviation)
        {
            var rnd = new Random();
            var result = new List<double>();
            for (int i = 0; i < values.Count; i++)
            {
                double sum = 0;
                for (int j = 0; j < GaussN; j++)
                {
                    sum += values[rnd.Next(values.Count)];
                }
                result.Add(expectation + deviation * Math.Sqrt(12 / GaussN) * (sum - GaussN / 2));
            }
            return result;
        }

        public static List<double> ToExponentialDistribution(List<double> values, double lambda)
        {
            var result = new List<double>();
            for (int i = 0; i < values.Count; i++)
            {
                result.Add(-1 / lambda * Math.Log(values[i]));
            }
            return result;
        }

        public static List<double> ToGammaDistribution(List<double> values, double eta, double lambda)
        {
            var rnd = new Random();
            var result = new List<double>();
            for (int i = 0; i < values.Count; i++)
            {
                double prod = 1;
                for (int j = 0; j < eta; j++)
                {
                    prod *= values[rnd.Next(values.Count)];
                }
                result.Add(-1 / lambda * Math.Log(prod));
            }
            return result;
        }

        public static List<double> ToTriangleDistribution(List<double> values, double min, double max)
        {
            var rnd = new Random();
            var result = new List<double>();
            for (int i = 0; i < values.Count; i++)
            {
                result.Add(min + (max - min) * Math.Max(values[rnd.Next(values.Count)], values[rnd.Next(values.Count)]));
            }
            return result;
        }

        public static List<double> ToSimpsonDistribution(List<double> values, double min, double max)
        {
            var rnd = new Random();
            var result = new List<double>();
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = min / 2 + (max / 2 - min / 2) * values[i];
            }
            for (int i = 0; i < values.Count; i++)
            {
                result.Add(values[rnd.Next(values.Count)] + values[rnd.Next(values.Count)]);
            }
            return result;
        }

    }
}
