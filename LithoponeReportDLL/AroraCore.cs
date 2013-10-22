using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lithopone.Memory;
using System.IO;
using LibTabCharter;
using NormDistributionLib;

namespace Arora
{
    //calculating and saving the scores
    public class AroraCore
    {
        public static String OUT_PATH = 
            AppDomain.CurrentDomain.BaseDirectory + "AroraAppendableGeneral";

        protected Dictionary<int, StItem> mItems;
        protected List<StAnswer> mAnswers;
        protected Dictionary<String, String> mDemogInfo;
        protected List<StNorm> mNorm;
        
        protected List<float> mDimzScores;
        protected List<double> mDimzPercentile;


        //must override
        //abstract public void DoReport();
        
        //abstract protected void ReadResultForm();

        public AroraCore()
        {
            mDimzPercentile = new List<double>();
            mDimzScores = new List<float>();
        }

        public void SetData(Dictionary<int, StItem> items, List<StAnswer> answers,
            Dictionary<String, String> demogInfo, List<StNorm> norm)
        {
            mItems = items;
            mAnswers = answers;
            mDemogInfo = demogInfo;
            mNorm = norm;
        }

        private List<String> genHeader()
        {
            List<String> header = new List<string>();
            for (int i = 0; i < mDemogInfo.Count; i++)
                header.Add(mDemogInfo.ElementAt(i).Key);

            header.Add("TimeStamp");

            for (int i = 0; i < mAnswers.Count; i++)
            {
                header.Add( i + "selected");
                //header.Add(i + "RT");
                header.Add(i + "value");
            }

            for (int i = 0; i < mNorm.Count; i++)
            {
                header.Add(mNorm[i].KEY + "_Socre");
                header.Add(mNorm[i].KEY + "_Percentile");
            }

            header.Add("Total_C_Score");
            header.Add("Validity");

            return header;
        }

        protected bool TestValid()
        {
            int TotalDiff = 0;
            //validity
            if (GetSingleItemScore(mItems, mAnswers, 3) != GetSingleItemScore(mItems, mAnswers, 59))
                TotalDiff++;
            if (GetSingleItemScore(mItems, mAnswers, 40) != GetSingleItemScore(mItems, mAnswers, 6))
                TotalDiff++;
            if (GetSingleItemScore(mItems, mAnswers, 55) != GetSingleItemScore(mItems, mAnswers, 33))
                TotalDiff++;

            if (TotalDiff > 1)
                return false;
            else
                return true;
        }

        virtual protected void WriteFile(String path)
        {
            TabCharter ltc = new TabCharter(path);

            if (!File.Exists(path))
            {
                ltc.Create(genHeader());
            }

            //demog info
            List<String> line = new List<string>();
            for (int i = 0; i < mDemogInfo.Count; i++)
            {
                line.Add(mDemogInfo.ElementAt(i).Value);
            }

            DateTime dt = DateTime.Now;
            line.Add(dt.Year + "-" + dt.Month + "-" + dt.Day + " " + 
                dt.Hour + ":" + dt.Minute + ":" + dt.Second);

            //items` info
            for (int i = 0; i < mAnswers.Count; i++)
            {
                line.Add(mAnswers[i].Selected.ToString());
                //line.Add(mAnswers[i].RT.ToString());
                line.Add(GetSingleItemScore(mItems, mAnswers, i).ToString());
            }

            //norm info
            for (int i = 0; i < mNorm.Count; i++ )
            {
                line.Add(mDimzScores[i].ToString());
                line.Add(mDimzPercentile[i].ToString());
            }

            int TotalStandardScore = (int)Math.Round((mDimzScores[5] - 194.583) / 6.617 * 100 + 500);
            line.Add(TotalStandardScore.ToString());

            line.Add(TestValid().ToString());

            ltc.Append(line);
        }

        protected void calcNormRelatedResult()
        {
            float singleNormScore = 0;
            for (int i = 0; i < mNorm.Count; i++)
            {
                singleNormScore = GetSingleDimensionzScore(mItems, mAnswers, mNorm[i]);
                mDimzScores.Add(singleNormScore);
                mDimzPercentile.Add(DifferenceIntegrate.GetSpecificAreaSize(
                    1000000, mNorm[i].Mean, mNorm[i].SD, mNorm[i].Mean - 4 * mNorm[i].SD, 
                    singleNormScore));
            }
        }

        virtual public void Sta_Save()
        {
            calcNormRelatedResult();
            WriteFile(OUT_PATH);
        }

        public float GetSingleDimensionzScore(
            Dictionary<int, StItem> item, List<StAnswer> answer, StNorm norm)
        {
            float retval = 0;
            int itemNumBuf = 0;

            for (int i = 0; i < norm.ItemIDs.Length; i++)
            {
                //get index in norm
                itemNumBuf = norm.ItemIDs[i];
                //get certain index`s selection value added to return value
                retval += item[itemNumBuf].Selections[answer[itemNumBuf].Selected].Value;
            }

            return retval;
        }

        public float GetSingleItemScore(
            Dictionary<int, StItem> item, List<StAnswer> answer, int index)
        {
            if (answer[index].Selected != -1)
            {
                return item[index].Selections[answer[index].Selected].Value;
            }
            else
            {
                return 0;
            }
        }
    }
}
