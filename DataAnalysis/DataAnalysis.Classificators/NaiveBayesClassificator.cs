using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.Classificators
{
    public class NaiveBayesClassificator : IClassificator
    {
        private Dictionary<int, double[]> _standards;

        private double GetEuclideanDistance(double[] vector1, double[] vector2)
        {
            if(vector1 == null || vector2 == null)
                throw new ArgumentNullException();
            if(vector1.Length != vector2.Length)
                throw new ArgumentException();
            var dimension = vector1.Length;
            double result = 0;
            for (var i = 0; i < dimension; i++)
            {
                result += Math.Pow(vector1[i] - vector2[i], 2);
            }
            return Math.Sqrt(result);
        }

        public Dictionary<double[], int> Classificate(IEnumerable<double[]> inputVectors)
	    {
            if(_standards == null || !_standards.Any())
                throw new InvalidOperationException();
            if(inputVectors == null || !inputVectors.Any())
                throw new ArgumentException();

	        var result = new Dictionary<double[], int>();
	        foreach (var inputVector in inputVectors)
	        {
	            var minDistance = _standards.Min(d => GetEuclideanDistance(d.Value, inputVector));
	            var vectorClass = _standards.Where(s => GetEuclideanDistance(s.Value, inputVector) == minDistance).Select(s=>s.Key).FirstOrDefault();
                result.Add(inputVector, vectorClass);
	        }
	        return result;
	    }

        public void Teach(Dictionary<double[], int> inputVectors)
        {
            _standards = new Dictionary<int, double[]>();
            var groups = inputVectors.GroupBy(g => g.Value);
            var dimension = inputVectors.First().Key.Length;
            foreach (var group in groups)
            {
                var key = group.Key;
                var everageValues = new double[dimension];
                for (int i = 0; i < dimension; i++)
                {
                    everageValues[i] = group.Average(g => g.Key[i]);
                }
                _standards.Add(key, everageValues);
            }
        }
    }
}
