using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lithopone.Memory;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

//PTSD Double Test
namespace Arora
{
    public class AroraReport
    {
        protected static String OUT_PATH_SPECIFIC =
            AppDomain.CurrentDomain.BaseDirectory + "\\AroraAppendableSpecific";

        public ReportUI mUI;
        private int mCurPage = 0;
        private int mTotalPages;

        private AroraAppendableGeneralReader mReader;
        public StNormAuto mNorm;

        public AroraReport(int DemogLen, int TestCount, StNormAuto Norm, int UserIndex = -1)
        {
            mUI = new ReportUI(this);
            mReader = new AroraAppendableGeneralReader(DemogLen, TestCount, Norm);//norm for offset count
            mReader.Fetch(UserIndex);
            mTotalPages = mReader.mNorm.Dims.Count;
            mNorm = Norm;
        }

        /*public override void Run()
        {
            //calc
            calcNormRelatedResult();
            DoReport();
        }*/

        public void SetReportPageContent(int index)
        {
            //chart
            Color aboveColor = Color.FromArgb(255, 100, 100);
            Color belowColor = Color.FromArgb(200, 200, 255);
            NormDistriBarviewGen.SetChart(mUI.chart1, (int)(Math.Round(mReader.GetDimPercentile(index) * 100.0)),
                aboveColor, belowColor);
            //richText
            mUI.richTextBox1.Text = mReader.mNorm.Dims[index].Name + ": 高于%" +
                ((int)(Math.Round(mReader.GetDimPercentile(index)* 100.0))).ToString() + "的人群(红色部分)";
            //label
            mUI.label1.Text = (index + 1).ToString() + "/" + mTotalPages;
            //comment
            attachComment(index);
        }

        private void attachComment(int dimIndex)
        {
            //validity comment
            if (!mReader.IsValid())
            {
                mUI.richTextBox1.Text += "\r\n题目前后回答不一致";
            }
            //level comment
            if (mNorm.Dims[dimIndex].Levels.Count != 0)
            {
                string textShow = "";
                //get target value first
                double targetval = -1;
                if (mNorm.Dims[dimIndex].TargetValue == NormTargetValue.Percentile)
                {
                    targetval = mReader.GetDimPercentile(dimIndex) * 100.0;
                }
                else if (mNorm.Dims[dimIndex].TargetValue == NormTargetValue.RawScore)
                {
                    targetval = mReader.GetDimScore(dimIndex);
                }
                else if (mNorm.Dims[dimIndex].TargetValue == NormTargetValue.StdScore)//total score only
                {
                    targetval = mReader.GetDimStdScore(dimIndex);
                }

                textShow += "\r\n得分： " + targetval;
                textShow += "\r\n\r\n" + mNorm.Dims[dimIndex].Name + "评价: ";

                int levelAt = -1;

                for (int i = 0; i < mNorm.Dims[dimIndex].Levels.Count; i++)
                {
                    if (targetval >= mNorm.Dims[dimIndex].Levels[i].FloorWith &&
                        targetval < mNorm.Dims[dimIndex].Levels[i].CeilingWithout)
                    {
                        levelAt = i;
                        break;
                    }
                }

                textShow += "\r\n" + mNorm.Dims[dimIndex].Levels[levelAt].Description;
                mUI.richTextBox1.Text += textShow;
            }

        }

        public void DoReport()
        {
            //ReadResultForm();
            SetReportPageContent(0);
            mUI.Show();
        }

        public void NextPage()
        {
            if (mCurPage < mTotalPages - 1)
            {
                mCurPage++;
                SetReportPageContent(mCurPage);
            }

            if (mCurPage == mTotalPages - 1)
            {
                mUI.button2.Text = "打印";
            }
        }

        public void PrevPage()
        {
            if (mCurPage > 0)
            {
                mCurPage--;
                SetReportPageContent(mCurPage);
                mUI.button2.Text = "下一页";
            }
        }

        public void Close()
        {

        }
    }
}
