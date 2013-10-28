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
        protected StNormAuto mNorm;
        
        protected List<float> mDimzScores;
        protected List<double> mDimzPercentile;
        protected List<double> mDimzStdScore;


        //must override
        //abstract public void DoReport();
        
        //abstract protected void ReadResultForm();

        public AroraCore()
        {
            mDimzPercentile = new List<double>();
            mDimzScores = new List<float>();
            mDimzStdScore = new List<double>();
        }

        public void SetData(Dictionary<int, StItem> items, List<StAnswer> answers,
            Dictionary<String, String> demogInfo, StNormAuto norm)
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

            for (int i = 0; i < mNorm.Dims.Count; i++)
            {
                header.Add(mNorm.Dims[i].Name + "_Socre");
                header.Add(mNorm.Dims[i].Name + "_Percentile");
                header.Add(mNorm.Dims[i].Name + "_StdScore");
            }

            header.Add("Validity");

            return header;
        }

        protected bool TestValid()
        {
            int TotalDiff = 0;

            //validity with principal
            for (int i = 0; i < mNorm.Validity.ItemIndex.Count / 2; i += 2)
            {
                if (mNorm.Validity.Principal == ValidityPrincipal.IndexEqual)
                {
                    if (mAnswers[mNorm.Validity.ItemIndex[i]] !=
                        mAnswers[mNorm.Validity.ItemIndex[i + 1]])
                    {
                        TotalDiff++;
                    }
                }
                else if (mNorm.Validity.Principal == ValidityPrincipal.ValueEqual)
                {
                    if (GetSingleItemScore(mItems, mAnswers, mNorm.Validity.ItemIndex[i]) !=
                        GetSingleItemScore(mItems, mAnswers, mNorm.Validity.ItemIndex[i + 1]))
                    {
                        TotalDiff++;
                    }
                }
            }

            if (TotalDiff >= mNorm.Validity.Tolerance)
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
            for (int i = 0; i < mNorm.Dims.Count; i++ )
            {
                line.Add(mDimzScores[i].ToString());
                line.Add(mDimzPercentile[i].ToString());
                line.Add(mDimzStdScore[i].ToString());
            }

            line.Add(TestValid().ToString());

            ltc.Append(line);
        }

        protected void calculateStdScore()
        {
            for (int index = 0; index < mDimzScores.Count; index++)
            {
                double retval = mDimzScores[index];

                for (int i = 0; i < mNorm.Dims[index].Method.Method.Count; i++)
                {
                    if (mNorm.Dims[index].Method.Method[i] == '+')
                    {
                        retval += mNorm.Dims[index].Method.Values[i];
                    }
                    else if (mNorm.Dims[index].Method.Method[i] == '-')
                    {
                        retval -= mNorm.Dims[index].Method.Values[i];
                    }
                    else if (mNorm.Dims[index].Method.Method[i] == '*')
                    {
                        retval *= mNorm.Dims[index].Method.Values[i];
                    }
                    else if (mNorm.Dims[index].Method.Method[i] == '/')
                    {
                        retval /= mNorm.Dims[index].Method.Values[i];
                    }
                }

                mDimzStdScore.Add(retval);
            }
        }

        //raw score and percentile
        protected void calcNormRelatedResult()
        {
            float singleNormScore = 0;
            for (int i = 0; i < mNorm.Dims.Count; i++)
            {
                singleNormScore = GetSingleDimensionzScore(mItems, mAnswers, mNorm.Dims[i].ItemIndexList);
                mDimzScores.Add(singleNormScore);
                mDimzPercentile.Add(DifferenceIntegrate.GetSpecificAreaSize(
                    1000000, mNorm.Dims[i].Mean, mNorm.Dims[i].SD, mNorm.Dims[i].Mean - 4 * mNorm.Dims[i].SD, 
                    singleNormScore));
            }
        }

        virtual public void Sta_Save()
        {
            calcNormRelatedResult();
            calculateStdScore();
            WriteFile(OUT_PATH);
        }

        public float GetSingleDimensionzScore(
            Dictionary<int, StItem> item, List<StAnswer> answer, List<int> itemIndexList)
        {
            float retval = 0;
            int itemNumBuf = 0;

            for (int i = 0; i < itemIndexList.Count; i++)
            {
                //get index in norm
                itemNumBuf = itemIndexList[i];
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
