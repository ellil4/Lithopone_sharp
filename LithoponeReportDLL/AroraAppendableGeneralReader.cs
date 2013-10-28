using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibTabCharter;
using Lithopone.Memory;

namespace Arora
{
    public class AroraAppendableGeneralReader
    {
        public int mDemogLen;
        public int mTestCount;
        public StNormAuto mNorm;

        private TabFetcher mFetch;
        private List<String> mLine;

        public AroraAppendableGeneralReader(int DemogLen, int TestCount, StNormAuto Norm)
        {
            mFetch = new TabFetcher(AroraCore.OUT_PATH, "\\t");

            mDemogLen = DemogLen;
            mTestCount = TestCount;
            mNorm = Norm;
        }

        public void Fetch(int index)
        {
            mFetch.Open();
            if (index != -1)
            {
                mLine = mFetch.GetLineAt(index);
            }
            else
            {
                mLine = mFetch.GetLineAt(mFetch.GetLineCount() - 2);
            }
            mFetch.Close();
        }

        public int GetRatingDimCount()
        {
            return mNorm.Dims.Count;
        }

        private int getBaseOffset()
        {
            return mDemogLen + 1 + mTestCount * 2;
            //demog + time + itemCount * 2(indexSel, value)
        }

        public double GetItemSelected(int Index)
        {
            return double.Parse(mLine[mDemogLen + 1 + Index * 2]);
        }

        public double GetItemSelectedValue(int Index)
        {
            return double.Parse(mLine[mDemogLen + 1 + Index * 2 + 1]);
        }

        public double GetDimScore(int Index)
        {
            return double.Parse(mLine[getBaseOffset() + Index * 3]);
        }

        public double GetDimPercentile(int Index)
        {
            return double.Parse(mLine[getBaseOffset() + Index * 3 + 1]);
        }

        public double GetDimStdScore(int Index)
        {
            return double.Parse(mLine[getBaseOffset() + Index * 3 + 2]);
        }

        public bool IsValid()
        {
            return bool.Parse(mLine[getBaseOffset() + mNorm.Dims.Count * 3 + 1]);
        }
    }
}
