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

    abstract public class AroraCoreBase
    {
        protected static String OUT_PATH = 
            AppDomain.CurrentDomain.BaseDirectory + "\\AroraAppendableGeneral";

        protected ReportForm mRF;
        protected Dictionary<int, StItem> mItems;
        protected List<StAnswer> mAnswers;
        protected Dictionary<String, String> mDemogInfo;
        protected List<StNorm> mNorm;
        
        protected List<float> mDimzScores;
        protected List<double> mDimzPercentile;
        protected String mOtherInfo;

        public AroraCoreBase(ReportForm rf)
        {
            mRF = rf;
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

            for (int i = 0; i < mAnswers.Count; i++)
            {
                header.Add( i + "selected");
                header.Add(i + "RT");
                header.Add(i + "value");
            }

            for (int i = 0; i < mNorm.Count; i++)
            {
                header.Add(mNorm[i].KEY + "_Socre");
                header.Add(mNorm[i].KEY + "_Percentile");
            }

            header.Add("Total_C+Score");

            return header;
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

            //items` info
            for (int i = 0; i < mAnswers.Count; i++)
            {
                line.Add(mAnswers[i].Selected.ToString());
                line.Add(mAnswers[i].RT.ToString());
                line.Add(GetSingleItemScore(mItems, mAnswers, i).ToString());
            }

            //norm info
            for (int i = 0; i < mNorm.Count; i++ )
            {
                line.Add(mDimzScores[i].ToString());
                line.Add(mDimzPercentile[i].ToString());
            }

            line.Add(mOtherInfo);

            ltc.Append(line);
        }

        //must override
        abstract protected void ShowReport();

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

        virtual public void Run()
        {
            calcNormRelatedResult();
            WriteFile(OUT_PATH);
            ShowReport();
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
