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
            chart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.None;
            chart.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 0;
            chart.ChartAreas["ChartArea1"].Area3DStyle.Perspective = 10;
            chart.ChartAreas["ChartArea1"].BackColor = Color.FromArgb(255, 255, 255);
            chart.ChartAreas["ChartArea1"].AxisX.Enabled = AxisEnabled.False;
            chart.ChartAreas["ChartArea1"].AxisY.Enabled = AxisEnabled.False;
            chart.ChartAreas["ChartArea1"].Area3DStyle.WallWidth = 0;
            chart.ChartAreas["ChartArea1"].Area3DStyle.PointDepth = 1000;

            Series serbelow = new Series("below");
            serbelow.IsVisibleInLegend = false;
            serbelow.ChartType = SeriesChartType.Area;
            serbelow.BorderWidth = 0;
            serbelow.ShadowOffset = 3;
            serbelow.Color = belowcolor;

            Series serover = new Series("over");
            serover.IsVisibleInLegend = false;
            serover.ChartType = SeriesChartType.Area;
            serover.BorderWidth = 0;
            serover.ShadowOffset = 3;
            serover.Color = overcolor;

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
            chart.Series.Add(serbelow);
            chart.Series.Add(serover);
        }
    }
}