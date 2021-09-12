using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LehmerSequenceGenerator
{
    class Characteristics
    {
        private List<double> _generatedValues = new List<double>();
        
        private double _mathExpectation;
        private double _dispersion;
        private double _meanSquareDeviation;
        private int _period;
        private int _aperiodicityLength;

        private bool _mathExpectationFound;
        private bool _dispersionFound;
        private bool _meanSquareDeviationFound;
        private bool _periodFound;
        private bool _aperiodicityLengthFound;

        public Characteristics(List<double> sequence)
        {
            if (sequence.Count == 0)
            {
                throw new ArgumentOutOfRangeException("The length of the sequence cannot be zero");
            }

            _generatedValues = sequence;
        }

        public double FindMathExpection()
        {
            if (!_mathExpectationFound)
            {
                double tempSum = 0;
                int count = _generatedValues.Count;

                for (int i = 0; i < count; i++)
                {
                    tempSum += _generatedValues[i];
                }

                _mathExpectation = tempSum / count;
                _mathExpectationFound = true;
            }

            return _mathExpectation;
        }

        public double FindDispersion()
        {
            if (!_dispersionFound)
            {
                double _mathException = FindMathExpection();
                double tempSum = 0;
                int count = _generatedValues.Count;

                foreach (double x in _generatedValues)
                {
                    tempSum += (x - _mathException) * (x - _mathException);
                }

                _dispersion = tempSum / count;
                _dispersionFound = true;
            }

            return _dispersion;
        }

        public double FindMeanSquareDeviation()
        {
            if (!_meanSquareDeviationFound)
            {
                _meanSquareDeviation = Math.Sqrt(_dispersion);
                _meanSquareDeviationFound = true;
            }

            return _meanSquareDeviation;
        }

        public int FindPeriod()
        {
            if (!_periodFound)
            {
                double last = _generatedValues.Last();
                int leftBorder = 0;
                int rightBorder = 0;
                bool switcher = false;

                //int count = _generatedValues.Count;

                for (int i = 0; i < _generatedValues.Count; i++)
                {
                    if (Math.Abs(_generatedValues[i] - last) < 10e-20)
                    {
                        if (!switcher)
                        {
                            switcher = true;
                            leftBorder = i;
                        }
                        else
                        {
                            rightBorder = i;
                            break;
                        }
                    }
                }

                _period = rightBorder - leftBorder;
                _periodFound = true;
            }

            return _period;
        }

        public int FindAperiodicityLength()
        {
            if (!_aperiodicityLengthFound)
            {
                int idx = 0;
                while (_generatedValues[idx] != _generatedValues[idx + _period])
                {
                    idx++;
                }

                _aperiodicityLength = idx + _period;
                _aperiodicityLengthFound = true;
            }

            return _aperiodicityLength;
        }

        public double CheckIndirect()
        {
            int numPairs = 0;
            int count = _generatedValues.Count;

            for (int i = 0; i < count; i += 2)
            {
                if (_generatedValues[i] * _generatedValues[i] + _generatedValues[i + 1] * _generatedValues[i + 1] < 1)
                {
                    numPairs++;
                }
            }

            return 2 * (double)numPairs / count;
        }
    }
}
