using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.Classificators
{
	public interface IClassificator
	{
		Dictionary<double[], bool> Classificate(IEnumerable<double[]> inputVectors);
	    bool Classificate(double[] inputVector);
        void Teach(Dictionary<double[], bool> inputVectors);
    }
}
