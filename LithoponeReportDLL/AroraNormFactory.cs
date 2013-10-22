using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lithopone.Memory;

namespace Arora
{
    public class AroraNormFactory
    {
        public List<StNorm> GetNorm()
        {
            List<StNorm> retval = new List<StNorm>();
            buildNorm(ref retval);
            return retval;
        }

        private void buildNorm(ref List<StNorm> norm)
        {
            int[] socialArr = {0, 7, 12, 14, 15, 18, 20, 32, 49, 65};
            StNorm social = new StNorm("人际", 30.3333, 6.167, socialArr);

            int[] recogArr = { 1, 9, 22, 28, 34, 36, 41, 42, 47, 53 };
            StNorm recog = new StNorm("认知", 26.403, 5.710, recogArr);

            int[] adaptArr = { 2, 4, 6, 13, 16, 23, 25, 26, 29, 30, 31, 33, 37, 40, 44, 51, 55, 61, 63 };
            StNorm adapt = new StNorm("适应", 56.246, 10.256, adaptArr);

            int [] egoArr = {5, 17, 21, 38, 45, 50, 52, 56, 59, 60, 67};
            StNorm ego = new StNorm("自我", 35.588, 5.541, egoArr);

            int[] emoArr = {3, 8, 10, 11, 19, 24, 27, 35, 43, 46, 48, 57, 62, 64, 66};
            StNorm emo = new StNorm("情绪", 45.827, 8.657, emoArr);

            int[] totalArr = new int[68];
            for (int i = 0; i < 68; i++)
            {
                totalArr[i] = i;
            }
            StNorm total = new StNorm("total", 194.583, 30.155, totalArr);

            norm.Add(social);
            norm.Add(recog);
            norm.Add(adapt);
            norm.Add(ego);
            norm.Add(emo);
            norm.Add(total);
        }
    }
}
