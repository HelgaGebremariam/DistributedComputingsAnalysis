using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using DataAnalysis.Classificators;
using DataAnalysis.DataLoading;
using Microsoft.Win32;
using System.Drawing;
using System.Globalization;
using System.Windows.Controls.DataVisualization.Charting;

namespace DataAnalysis.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IEnumerable<double[]> _sourceObjects = new List<double[]>();
		private readonly IDataLoader _dataLoader;

		public MainWindow()
		{
			InitializeComponent();
			_dataLoader = new TextFileDataLoader();

		}

		private void buttonOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
			{
				LoadDataFromFile(openFileDialog.FileName);
				FillDataGrid();
			}
		}

		private void LoadDataFromFile(string filename)
		{
			var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
			_sourceObjects = _dataLoader.GetSourceObjectsFromStream(stream);
		}

		private void FillDataGrid()
		{
			var dimensions = _sourceObjects.First().Length;
			for (int i = 0; i < dimensions; i++)
			{
				var column = new DataGridTextColumn {Binding = new Binding($"[{i}]")};
				DataGridSourceVectors.Columns.Add(column);
			}
			foreach (var sourceObject in _sourceObjects)
			{
				DataGridSourceVectors.Items.Add(sourceObject);
			}
		}

	    private void DrawRockCurvePositive(IEnumerable<System.Drawing.Point> points)
	    {
	        var dataPoints = new PointCollection();
	        foreach (var point in points)
	        {
	            dataPoints.Add(new System.Windows.Point(point.X, point.Y));
	        }
            ChartRockCurvePositive.DataContext = dataPoints;
	    }

	    private void DrawRockCurveNegative(IEnumerable<System.Drawing.Point> points)
	    {
	        var dataPoints = new PointCollection();
	        foreach (var point in points)
	        {
	            dataPoints.Add(new System.Windows.Point(point.X, point.Y));
	        }
	        ChartRockCurveNegative.DataContext = dataPoints;
	    }

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            var classificator = new KNearestNeighborsClassificator();
            var testData = DataGenerator.GenerateTeachingData(3, 100);
            classificator.Teach(testData);
            var analyzer = new ClassificatorAnalyzer(classificator, testData);
            DrawRockCurveNegative(analyzer.GetRocCurveNegative());
            DrawRockCurvePositive(analyzer.GetRocCurvePositive());
            TextBoxFn.Text = analyzer.FalseNegative.ToString();
            TextBoxFp.Text = analyzer.FalsePositive.ToString();
            TextBoxPrecision.Text = analyzer.Precision.ToString(CultureInfo.InvariantCulture);
            TextBoxRecall.Text = analyzer.Recall.ToString(CultureInfo.InvariantCulture);
            TextBoxFMeasure.Text = analyzer.FMeasure.ToString(CultureInfo.InvariantCulture);
        }
    }
}
