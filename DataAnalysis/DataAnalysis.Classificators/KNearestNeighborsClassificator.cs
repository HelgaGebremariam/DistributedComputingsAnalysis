using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.Classificators
{
	public class KNearestNeighborsClassificator : IClassificator
	{
	    private Dictionary<bool, double[]> _standards;

	    private static double GetEuclideanDistance(double[] vector1, double[] vector2)
	    {
	        if (vector1 == null || vector2 == null)
	            throw new ArgumentNullException();
	        if (vector1.Length != vector2.Length)
	            throw new ArgumentException();

	        var dimension = vector1.Length;
	        double result = 0;

	        for (var i = 0; i < dimension; i++)
	        {
	            result += Math.Pow(vector1[i] - vector2[i], 2);
	        }

	        return Math.Sqrt(result);
	    }

	    public Dictionary<double[], bool> Classificate(IEnumerable<double[]> inputVectors)
	    {
	        if (_standards == null || !_standards.Any())
	            throw new InvalidOperationException();
	        if (inputVectors == null || !inputVectors.Any())
	            throw new ArgumentException();

	        var result = new Dictionary<double[], bool>();
	        foreach (var inputVector in inputVectors)
	        {
	            result.Add(inputVector, Classificate(inputVector));
	        }

	        return result;
	    }

	    public bool Classificate(double[] inputVector)
	    {
	        var minDistance = _standards.Min(d => GetEuclideanDistance(d.Value, inputVector));
	        return _standards.Where(s => GetEuclideanDistance(s.Value, inputVector) <= minDistance).Select(s => s.Key).FirstOrDefault();
        }

	    public void Teach(Dictionary<double[], bool> inputVectors)
	    {
	        _standards = new Dictionary<bool, double[]>();

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
