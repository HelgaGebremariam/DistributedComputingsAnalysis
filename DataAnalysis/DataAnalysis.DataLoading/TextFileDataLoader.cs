using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.DataLoading
{
    public class TextFileDataLoader : IDataLoader
    {
	    private double[] StringToVector(string strData)
	    {
		    return strData.Split(',').Select(Convert.ToDouble).ToArray();
	    }

	    private double[] StringToClassificatedVector(string strData, out int vectorClass)
	    {
		    var vector = StringToVector(strData);
		    vectorClass = Convert.ToInt32(vector.First());
		    return vector.Skip(0).ToArray();

	    }

		public IEnumerable<double[]> GetSourceObjectsFromStream(Stream stream)
	    {
		    var resultObjectsArray = new List<double[]>();
		    using (var reader = new StreamReader(stream))
		    {
			    var dimension = 0;
			    while (!reader.EndOfStream)
			    {
				    var tmpVector = StringToVector(reader.ReadLine());
				    if (dimension == 0)
				    {
					    dimension = tmpVector.Length;
				    }
				    if (tmpVector.Length != dimension)
				    {
					    throw new ArgumentException("Objects dimensions differ");
				    }
				    resultObjectsArray.Add(tmpVector);
				}
		    }
		    return resultObjectsArray;
	    }


	    public Dictionary<double[], int> GetClassificatedObjectsFromStream(Stream stream)
	    {
		    var resultObjectsArray = new Dictionary<double[], int>();
		    using (var reader = new StreamReader(stream))
		    {
			    var dimension = 0;
			    while (!reader.EndOfStream)
			    {
				    int vectorClass;
				    var tmpVector = StringToClassificatedVector(reader.ReadLine(), out vectorClass);
				    if (dimension == 0)
				    {
					    dimension = tmpVector.Length;
				    }
				    if (tmpVector.Length != dimension)
				    {
					    throw new ArgumentException("Objects dimensions differ");
				    }
				    resultObjectsArray.Add(tmpVector, vectorClass);
			    }
		    }
		    return resultObjectsArray;
		}
    }
}
