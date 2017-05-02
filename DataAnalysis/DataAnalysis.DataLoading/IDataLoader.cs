using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataAnalysis.DataLoading
{
	public interface IDataLoader
	{
		IEnumerable<double[]> GetSourceObjectsFromStream(Stream stream);
		Dictionary<double[], int> GetClassificatedObjectsFromStream(Stream stream);
	}
}
