using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

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
                    _falseNegative = _classifiedObjects.Count(s => s.ActualClass && s.ClassifiedValue == false);
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

            CrossValidationTeachAndClassificate(testVectors, 5);
        }

        private void CrossValidationTeachAndClassificate(Dictionary<double[], bool> testVectors, int groupsNumber)
        {
            int vectorsInGroup = (int)((double)testVectors.Count / groupsNumber);
            for (int i = 0; i < groupsNumber; i++)
            {
                var testVectorsFrom = i * vectorsInGroup;
                var testVectorsTo = (i * vectorsInGroup) + vectorsInGroup;

                var teachingVectors = testVectors.Skip(testVectorsFrom).Take(vectorsInGroup).ToDictionary(x => x.Key, x => x.Value);

                var checkVectors = testVectors.Take(testVectorsFrom).ToDictionary(x => x.Key, x => x.Value);
                foreach (var element in testVectors.Skip(testVectorsTo))
                {
                    checkVectors.Add(element.Key, element.Value);
                }
                _classificator.Teach(teachingVectors);
                Classificate(checkVectors);

            }
        }

        private void Classificate(Dictionary<double[], bool> testVectors)
        {
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

        public IEnumerable<Point> GetRocCurvePositive()
        {
            return GetRocCurve(true);
        }

        public IEnumerable<Point> GetRocCurveNegative()
        {
            return GetRocCurve(false);
        }

        private IEnumerable<Point> GetRocCurve(bool isPositive)
        {
            var result = new List<Point>();
            var currentPosition = new Point(0, 0);
            result.Add(currentPosition);
            var pointsNumber = _classifiedObjects.Count(s => s.ClassifiedValue == isPositive);
            foreach (var classifiedObject in _classifiedObjects.Where(s => s.ClassifiedValue == isPositive))
            {
                var x = currentPosition.X;
                var y = currentPosition.Y;
                if (classifiedObject.ActualClass == isPositive)
                {
                    x++;
                }
                else
                {
                    y++;
                }
                currentPosition = new Point(x, y);
                result.Add(currentPosition);
            }
            currentPosition = new Point(pointsNumber, pointsNumber);
            result.Add(currentPosition);
            return result;
        }
    }
}
