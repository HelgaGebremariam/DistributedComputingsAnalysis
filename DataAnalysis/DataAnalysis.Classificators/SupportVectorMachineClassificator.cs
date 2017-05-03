using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.Classificators
{
    public class SupportVectorMachineClassificator : IClassificator
    {


        public Dictionary<double[], bool> Classificate(IEnumerable<double[]> inputVectors)
        {
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
            throw new NotImplementedException();
        }

        public void Teach(Dictionary<double[], bool> inputVectors)
        {

        }
    }
}
