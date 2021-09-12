using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LehmerSequenceGenerator
{
    class LehmerAlgorithm
    {
        private double _multiplier;
        private double _startValue;
        private double _module;

        private double _prev;

        public double Multiplier => _multiplier;
        public double StartValue => _startValue;
        public double Module => _module;

        public LehmerAlgorithm(double multiplier, double startValue, double module)
        {
            _multiplier = multiplier;
            _startValue = startValue;
            _module = module;
            _prev = startValue;
        }

        public double GetNextValue() => (_multiplier * _prev) % _module;

        public List<double> GenerateSequence(int number)
        {
            List<double> generatedValues = new List<double>();

            for (int i = 0; i < number; i++)
            {
                double next = GetNextValue();
                _prev = next;
                generatedValues.Add(next / _module);
            }

            return generatedValues;
        }
    }
}
