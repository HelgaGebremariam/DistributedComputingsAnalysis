using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.Classificators
{
    public static class DataGenerator
    {
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

        public static Dictionary<double[], bool> GenerateTeachingData(int dimension, int objectsNumber, Func<double[], bool> classificationFunction)
        {
            var testData = new Dictionary<double[], bool>();
            var randomGenerator = new Random();
            for (int i = 0; i < objectsNumber; i++)
            {
                var testObject = new double[dimension];
                for (int j = 0; j < dimension; j++)
                {
                    testObject[j] = (double)randomGenerator.Next() / int.MaxValue;
                }
                var testObjectClass = classificationFunction(testObject);
                testData.Add(testObject, testObjectClass);
            }
            return testData;
        }
    }
}
