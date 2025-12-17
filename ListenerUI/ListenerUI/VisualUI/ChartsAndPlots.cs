using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ScottPlot.WinForms;
using ScottPlot;

namespace ListenerUI.VisualUI
{
    public class ChartsAndPlots
    {
        public FormsPlot CreateLineChart(List<double> values, string title, string xAxisValue = "Index")
        {
            var formsPlot = new FormsPlot
            {
                Dock = DockStyle.Fill
            };

            double[] xs = System.Linq.Enumerable.Range(0, values.Count).Select(i => (double)i).ToArray();
            double[] ys = values.ToArray();

            var plot = formsPlot.Plot;
            plot.Add.Scatter(xs, ys);
            plot.Title(title);
            plot.XLabel(xAxisValue);
            plot.YLabel("Value");

            return formsPlot;
        }

        /// <summary>
        /// Creates a bar chart using ScottPlot v5.
        /// </summary>
        public FormsPlot CreateBarChart(Dictionary<string, double> data, string title)
        {
            var formsPlot = new FormsPlot { Dock = DockStyle.Fill };

            var plot = formsPlot.Plot;
            var bars = plot.Add.Bars(data.Values.ToArray());

            // Add X-axis ticks manually
            plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
                Enumerable.Range(0, data.Count).Select(i => (double)i).ToArray(),
                data.Keys.ToArray()
            );

            plot.Title(title);
            return formsPlot;
        }

        /// <summary>
        /// Creates a pie chart using ScottPlot v5.
        /// </summary>
        public FormsPlot CreatePieChart(Dictionary<string, double> data, string title)
        {
            var formsPlot = new FormsPlot { Dock = DockStyle.Fill };
            var plot = formsPlot.Plot;

            var values = data.Values.ToArray();
            var labels = data.Keys.ToArray();

            // Create Pie chart
            var pie = plot.Add.Pie(values);

            // Assign labels to slices and enable values+percentages
            for (int i = 0; i < pie.Slices.Count; i++)
            {
                double percent = (values[i] / values.Sum()) * 100;
                pie.Slices[i].Label = $"{labels[i]} ({values[i]} | {percent:0.##}%)";

                // Style (optional)
                pie.Slices[i].LabelStyle.Bold = true;
                pie.Slices[i].LabelStyle.FontSize = 14;
            }

            plot.Title(title);
            plot.ShowLegend();   // show legend for labels

            return formsPlot;
        }
    }
}