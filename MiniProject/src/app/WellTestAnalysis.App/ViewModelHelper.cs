using Microsoft.Win32;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using WellTestAnalysis.Models;
using OxyPlot.Legends;
using System.CodeDom.Compiler;

namespace WellTestAnalysis.App
{
    public static class ViewModelHelper
    {    

        public static PlotModel PlotModel(IEnumerable<PressureTimeData> pressureTimes, string title)
        {
            var model = new PlotModel();

           
            model.Title = "Pressure vs. Time";
            model.Subtitle = title;
            model.PlotType = PlotType.XY;

            var xAxis = new LinearAxis
            {
                Title = "Pressure (psi)",
                MinorGridlineColor = OxyColor.FromRgb(211, 211, 211),
                MajorGridlineColor = OxyColor.FromRgb(211, 211, 211),
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineStyle = LineStyle.Solid,
                IsZoomEnabled = true,
                Position = AxisPosition.Left,
            };
            var yAxis = new LinearAxis
            {
                Title = "Time (hr)",
                MinorGridlineColor = OxyColor.FromRgb(211, 211, 211),
                MajorGridlineColor = OxyColor.FromRgb(211, 211, 211),
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineStyle = LineStyle.Solid,
                IsZoomEnabled = true,
                Position = AxisPosition.Bottom,
            };

            model.Axes.Add(xAxis);
            model.Axes.Add(yAxis);

            var series1 = new LineSeries { MarkerType = MarkerType.Square };
            foreach (var x in pressureTimes)
            {
                if (x.Time != null && x.Pressure != null)
                    series1.Points.Add(new DataPoint(Convert.ToDouble(x.Time), Convert.ToDouble(x.Pressure)));

            }


            model.Series.Add(series1);
            return model;
        }

        public static PlotModel PlotModel(IEnumerable<PressureTimeTrend> pressureTimes, string title)
        {
            var model = new PlotModel();

            var legends = new ElementCollection<LegendBase>(model)
            {
                new Legend()
                {
                    IsLegendVisible = true,
                    LegendPosition = LegendPosition.RightMiddle,
                    LegendPlacement = LegendPlacement.Inside
                }
            };
            model.Title = "Pressure vs. Time";
            model.Subtitle = title;
            model.PlotType = PlotType.XY;
            model.IsLegendVisible = true;
            model.Legends = legends;

            var xAxis = new LinearAxis
            {
                Title = "Pressure (psi)",
                MinorGridlineColor = OxyColor.FromRgb(211, 211, 211),
                MajorGridlineColor = OxyColor.FromRgb(211, 211, 211),
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineStyle = LineStyle.Solid,
                IsZoomEnabled = true,
                Position = AxisPosition.Left,
            };
            var yAxis = new LinearAxis
            {
                Title = "Time (hr)",
                MinorGridlineColor = OxyColor.FromRgb(211, 211, 211),
                MajorGridlineColor = OxyColor.FromRgb(211, 211, 211),
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineStyle = LineStyle.Solid,
                IsZoomEnabled = true,
                Position = AxisPosition.Bottom,
            };

            model.Axes.Add(xAxis);
            model.Axes.Add(yAxis);

            var series1 = new LineSeries { MarkerType = MarkerType.Circle, Title = "Trended Pressure" };
            foreach (var x in pressureTimes)
            {
                if (x.Time != null && x.Pressure != null)
                    series1.Points.Add(new DataPoint(Convert.ToDouble(x.Time), Convert.ToDouble(x.Pressure)));

            }
            var series2 = new LineSeries { MarkerType = MarkerType.Square, Title = "Detrended Pressure" };
            foreach (var x in pressureTimes)
            {
                if (x.Time != null && x.Pressure != null)
                    series2.Points.Add(new DataPoint(Convert.ToDouble(x.Time), Convert.ToDouble(x.DetrendedPressure)));

            }


            model.Series.Add(series1);
            model.Series.Add(series2);

            return model;
        }
    }
}
