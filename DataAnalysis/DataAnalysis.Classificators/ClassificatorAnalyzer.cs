using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DataAnalysis.Classificators
{
    public class ClassifiedObject
    {
        public double[] VectorData;
        public bool ActualClass;
        public bool ClassifiedValue;
    }

    public class ClassificatorAnalyzer
    {
        private IClassificator _classificator;
        private List<ClassifiedObject> _classifiedObjects;

        private int _falseNegative = 0;
        public int FalseNegative
        {
            get
            {
                if(_falseNegative == 0)
                    _falseNegative = _classifiedObjects.Count(s => s.ActualClass == false && s.ClassifiedValue);
                return _falseNegative;
            }
        }

        private int _truePositive = 0;
        public int TruePositive
        {
            get
            {
                if (_truePositive == 0)
                    _truePositive = _classifiedObjects.Count(s => s.ActualClass && s.ClassifiedValue);
                return _truePositive;
            }
        }

        private int _trueNegative = 0;
        public int TrueNegative
        {
            get
            {
                if (_trueNegative == 0)
                    _trueNegative = _classifiedObjects.Count(s => s.ActualClass == false && s.ClassifiedValue == false);
                return _trueNegative;
            }
        }

        private int _falsePositive = 0;
        public int FalsePositive
        {
            get
            {
                if (_falsePositive == 0)
                    _falsePositive = _classifiedObjects.Count(s => s.ActualClass == false && s.ClassifiedValue);
                return _falsePositive;
            }
        }

        private int _negativeNumber = 0;
        public int NegativeNumber
        {
            get
            {
                if (_negativeNumber == 0)
                    _negativeNumber = _classifiedObjects.Count(s => s.ActualClass == false);
                return _negativeNumber;
            }
        }

        private int _positiveNumber = 0;
        public int PositiveNumber
        {
            get
            {
                if (_positiveNumber == 0)
                    _positiveNumber = _classifiedObjects.Count(s => s.ActualClass == true);
                return _positiveNumber;
            }
        }

        private double _normalizedFalseNegative = 0;

        public double NormalizedFalseNegative
        {
            get
            {
                if(_normalizedFalseNegative == 0)
                    _normalizedFalseNegative = (double)FalseNegative / NegativeNumber;
                return _normalizedFalseNegative;
            }
        }

        private double _normalizedFalsePositive = 0;

        public double NormalizedFalsePositive
        {
            get
            {
                if (_normalizedFalsePositive == 0)
                    _normalizedFalsePositive = (double)FalsePositive / PositiveNumber;
                return _normalizedFalsePositive;
            }
        }

        private double _precision = 0;
        public double Precision
        {
            get
            {
                if (_precision == 0)
                {
                    _precision = (double) TruePositive / (TruePositive + FalsePositive);
                }
                return _precision;
            }
        }

        private double _recall = 0;
        public double Recall
        {
            get
            {
                if (_recall == 0)
                {
                    _recall = (double) TruePositive / (TruePositive + FalseNegative);
                }
                return _recall;
            }
        }

        private double _fMeasure = 0;

        public double FMeasure
        {
            get
            {
                if (_fMeasure == 0)
                {
                    _fMeasure = (2 * Precision * Recall) / (Precision + Recall);
                }
                return _fMeasure;
            }
        }

        public ClassificatorAnalyzer(IClassificator classificator, Dictionary<double[], bool> testVectors)
        {
            _classificator = classificator;
            _classifiedObjects = new List<ClassifiedObject>();
            foreach (var testVector in testVectors)
            {
                _classifiedObjects.Add(new ClassifiedObject()
                {
                    VectorData = testVector.Key,
                    ActualClass = testVector.Value,
                    ClassifiedValue = _classificator.Classificate(testVector.Key)
                });
            }
        }

        public static IEnumerable<double[]> GenerateTestData(int dimension, int objectsNumber)
        {
            var testData = new List<double[]>();
            var randomGenerator = new Random();
            for (int i = 0; i < objectsNumber; i++)
            {
                var testObject = new double[dimension];
                for (int j = 0; j < dimension; j++)
                {
                    testObject[j] = randomGenerator.NextDouble() / double.MaxValue;
                }
                testData.Add(testObject);
            }
            return testData;
        }

        public IEnumerable<Point> GetRocCurvePositive()
        {
            var result = new List<Point>();
            var currentPosition = new Point(0, 0);
            foreach (var classifiedObject in _classifiedObjects.Where(s=>s.ClassifiedValue == true))
            {
                var x = currentPosition.X;
                var y = currentPosition.Y;
                if (classifiedObject.ActualClass)
                {
                    y++;
                }
                else
                {
                    x++;
                }
                currentPosition = new Point(x, y);
                result.Add(currentPosition);
            }
            return result;
        }

        public IEnumerable<Point> GetRocCurveNegative()
        {
            var result = new List<Point>();
            var currentPosition = new Point(0, 0);
            foreach (var classifiedObject in _classifiedObjects.Where(s=>s.ClassifiedValue == false))
            {
                var x = currentPosition.X;
                var y = currentPosition.Y;
                if (classifiedObject.ActualClass == false)
                {
                    y++;
                }
                else
                {
                    x++;
                }
                currentPosition = new Point(x, y);
                result.Add(currentPosition);
            }
            return result;
        }

    }
}
