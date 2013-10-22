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

        public AroraReport(int DemogLen, int TestCount, List<StNorm> Norm, int UserIndex = -1)
        {
            mUI = new ReportUI(this);
            mReader = new AroraAppendableGeneralReader(DemogLen, TestCount, Norm);
            mReader.Fetch(UserIndex);
            mTotalPages = mReader.mNorm.Count;
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
            mUI.richTextBox1.Text = mReader.mNorm[index].KEY + ": 高于%" +
                ((int)(Math.Round(mReader.GetDimPercentile(index)* 100.0))).ToString() + "的人群(红色部分)";
            //label
            mUI.label1.Text = (index + 1).ToString() + "/" + mTotalPages;

            if (index == 5)
            {
                page5Comment();
            }
            //else if(index == )
        }

        //private void page4Comment()
        //private void page3Comment()
        //private void page2Comment()
        //private void page1Comment()
        //private void page0Comment()

        private void page5Comment()
        {
            if (!mReader.IsValid())//invalid
            {
                mUI.richTextBox1.Text += "\r\n题目回答前后不一致。";
            }

            double totalStandardScore = mReader.GetStandardScore();

            String textShow = "总得分：" + totalStandardScore + "。\r\n\r\n";
            if (totalStandardScore > 600)
            {
                textShow += "评价：认知效能较好；情绪积极、稳定，较善于调适；自我评价较恰当；人际关系较和谐，有一定的交往能力，社会支持较好；适应能力较好，能应对应激事件。\r\n\r\n建议：进一步巩固目前良好心理状态，提高心理健康水平。";
            }
            else if (totalStandardScore < 600 && totalStandardScore > 400)
            {
                textShow += "评价：认知效能正常；情绪较积极、稳定，有一定的调适能力；自我评价尚恰当；人际关系尚和谐，交往能力和社会支持尚好；有一定的适应能力，能应对一般应激事件。\r\n\r\n建议：进行心理健康教育，学习心理学知识，促进心理能力的发展。进一步改善目前心理状态，提高心理健康水平。";
            }
            else if (totalStandardScore < 400 && totalStandardScore > 300)
            {
                textShow += "评价：认知效能较低；情绪较不稳定，调适能力较差；自我评价较不恰当，容易低估自己；人际关系、交往能力和社会支持较差；适应能力也较差，较难应对应激事件，耐挫力和复原力较弱。这些均表明受测者有轻度心理问题，面对压力或挫折时有一定风险。\r\n\r\n建议：除进行心理健康教育外，还应预防发生心理障碍，必要时进行心理疏导，改善其心理健康状况。";
            }
            else if (totalStandardScore < 300)
            {
                textShow += "评价：认知效能低下；情绪消极、不稳定，调适能力差；缺乏自知之明，自我评价不恰当，往往低估自己；人际关系不良，交往能力和社会支持差；适应不良，不能应对应激事件，耐挫力和复原力弱。这些综合表明受测者存在明显心理问题。\r\n\r\n建议： 需尽早就医, 进行临床筛查和诊断，及时提供心理咨询与治疗。";
            }

            mUI.richTextBox1.Text += "\r\n" + textShow;
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
