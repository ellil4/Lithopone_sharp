using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    public class StNormDim
    {
        public string Name;//dimension name
        public List<int> ItemIndexList;//dimension`s items
        public double Mean;//mean score
        public double SD;//standard deviation of the socre
        public NormTargetValue TargetValue;//target value type

        public List<StNormLevel> Levels;//levels of the dimension
        public StNormStdScoreMethod Method;//method to make this dimension`s standard score

        public StNormDim()
        {
            Name = "";
            ItemIndexList = new List<int>();
            Mean = -1;
            SD = -1;
            TargetValue = NormTargetValue.None;
            Levels = new List<StNormLevel>();
            Method = null;
        }

        public double GetStandardScore()
        {
            return -1;
        }
    }

    public enum NormTargetValue
    {
        None, RawScore, StdScore, Percentile
    }
}
