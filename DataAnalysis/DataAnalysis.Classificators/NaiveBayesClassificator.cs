using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.Classificators
{
    public class NaiveBayesClassificator : IClassificator
    {


        public Dictionary<double[], bool> Classificate(IEnumerable<double[]> inputVectors)
        {
            throw new NotImplementedException();
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
