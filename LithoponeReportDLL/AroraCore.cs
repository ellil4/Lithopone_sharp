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
    public class AroraCore : AroraCoreBase
    {
        protected static String OUT_PATH_SPECIFIC =
            AppDomain.CurrentDomain.BaseDirectory + "\\AroraAppendableSpecific";

        public AroraCore(ReportForm rf) : base(rf)
        {

        }

        public override void Run()
        {
            //calc
            calcNormRelatedResult();

            ShowReport();

        }

        protected override void ShowReport()
        {
            NormDistriBarviewGen.SetChart(mRF.amChart1, (int)(Math.Round(mDimzPercentile[5] * 100.0)), 
                Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 255));
            mRF.label1.Text = ("总得分高于%" +
                ((int)(Math.Round(mDimzPercentile[5] * 100.0))).ToString() + "的人群(红色部分)");

            NormDistriBarviewGen.SetChart(mRF.amChart2, (int)(Math.Round(mDimzPercentile[0] * 100.0)),
                Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 255));
            mRF.label2.Text = ("社会得分高于%" + 
                ((int)(Math.Round(mDimzPercentile[0] * 100.0))).ToString() + "的人群(红色部分)");

            NormDistriBarviewGen.SetChart(mRF.amChart3, (int)(Math.Round(mDimzPercentile[1] * 100.0)),
                Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 255));
            mRF.label3.Text = ("认知得分高于%" +
                ((int)(Math.Round(mDimzPercentile[1] * 100.0))).ToString() + "的人群(红色部分)");

            NormDistriBarviewGen.SetChart(mRF.amChart4, (int)(Math.Round(mDimzPercentile[2] * 100.0)),
                Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 255));
            mRF.label4.Text = ("适应得分高于%" +
                ((int)(Math.Round(mDimzPercentile[2] * 100.0))).ToString() + "的人群(红色部分)");

            NormDistriBarviewGen.SetChart(mRF.amChart5, (int)(Math.Round(mDimzPercentile[3] * 100.0)),
                Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 255));
            mRF.label5.Text = ("自我得分高于%" +
                ((int)(Math.Round(mDimzPercentile[3] * 100.0))).ToString() + "的人群(红色部分)");

            NormDistriBarviewGen.SetChart(mRF.amChart6, (int)(Math.Round(mDimzPercentile[4] * 100.0)),
                Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 255));
            mRF.label6.Text = ("情绪得分高于%" +
                ((int)(Math.Round(mDimzPercentile[4] * 100.0))).ToString() + "的人群(红色部分)");

            //validity
            int totalDiff = 0;
            if (GetSingleItemScore(mItems, mAnswers, 3) != GetSingleItemScore(mItems, mAnswers, 59))
                totalDiff++;
            if (GetSingleItemScore(mItems, mAnswers, 40) != GetSingleItemScore(mItems, mAnswers, 6))
                totalDiff++;
            if (GetSingleItemScore(mItems, mAnswers, 55) != GetSingleItemScore(mItems, mAnswers, 33))
                totalDiff++;

            int totalStandardScore = (int)Math.Round((mDimzScores[5] - 194.583) / 6.617 * 100 + 500);
            mOtherInfo = totalStandardScore.ToString();

            if (totalDiff > 1)//invalid
            {
                mRF.richTextBox1.Text = "题目回答一致性不足。";
            }
            else
            {
                String textShow = "总得分：" + totalStandardScore +　"。";
                if (totalStandardScore > 600)
                {
                    textShow += "评价：认知效能较好；情绪积极、稳定，较善于调适；自我评价较恰当；人际关系较和谐，有一定的交往能力，社会支持较好；适应能力较好，能应对应激事件。建议：进一步巩固目前良好心理状态，提高心理健康水平。";
                }
                else if(totalStandardScore < 600 && totalStandardScore > 400)
                {
                    textShow += "评价：认知效能正常；情绪较积极、稳定，有一定的调适能力；自我评价尚恰当；人际关系尚和谐，交往能力和社会支持尚好；有一定的适应能力，能应对一般应激事件。建议：进行心理健康教育，学习心理学知识，促进心理能力的发展。进一步改善目前心理状态，提高心理健康水平。";
                }
                else if(totalStandardScore < 400 && totalStandardScore > 300)
                {
                    textShow += "评价：认知效能较低；情绪较不稳定，调适能力较差；自我评价较不恰当，容易低估自己；人际关系、交往能力和社会支持较差；适应能力也较差，较难应对应激事件，耐挫力和复原力较弱。这些均表明受测者有轻度心理问题，面对压力或挫折时有一定风险。建议：除进行心理健康教育外，还应预防发生心理障碍，必要时进行心理疏导，改善其心理健康状况。";
                }
                else if(totalStandardScore < 300)
                {
                    textShow += "评价：认知效能低下；情绪消极、不稳定，调适能力差；缺乏自知之明，自我评价不恰当，往往低估自己；人际关系不良，交往能力和社会支持差；适应不良，不能应对应激事件，耐挫力和复原力弱。这些综合表明受测者存在明显心理问题。建议： 需尽早就医, 进行临床筛查和诊断，及时提供心理咨询与治疗。";
                }

                mRF.richTextBox1.Text = textShow;

                WriteFile(OUT_PATH);
            }

            mRF.ShowDialog();
        }
    }
}
