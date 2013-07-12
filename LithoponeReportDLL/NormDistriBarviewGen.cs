using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using NormDistributionLib;

namespace Arora
{
    public class NormDistriBarviewGen
    {
        static public void SetChart(Chart chart, int percentage, Color overcolor, Color belowcolor)
        {
            Series serover = new Series("over");
            serover.IsVisibleInLegend = false;
            serover.ChartType = SeriesChartType.Column;
            serover.BorderWidth = 3;
            serover.ShadowOffset = 2;
            serover.Color = overcolor;

            Series serbelow = new Series("below");
            serbelow.IsVisibleInLegend = false;
            serbelow.ChartType = SeriesChartType.Column;
            serbelow.BorderWidth = 3;
            serbelow.ShadowOffset = 2;
            serbelow.Color = belowcolor;

            int stepCount = 100;

            float sdRange = (float)2.58 * 2;
            float step = sdRange / stepCount;
            float startPoint = (float)-2.58;

            for (int i = 0; i < stepCount; i++)
            {
                float x = startPoint + i * step;
                if (i < percentage)
                {
                    serover.Points.AddXY(x, (float)ValueY.GetValue(0, 1, x));
                }
                else
                {
                    serbelow.Points.AddXY(x, (float)ValueY.GetValue(0, 1, x));
                }
            }

            chart.Series.Clear();
            chart.Series.Add(serover);
            chart.Series.Add(serbelow);
        }
    }
}