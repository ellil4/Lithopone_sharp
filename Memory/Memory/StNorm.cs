using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    public class StNorm
    {
        public String KEY;
        public double Mean;
        public double SD;
        public int[] ItemIDs;

        public StNorm(String key, double mean, double sd, int[] itemIDs)
        {
            KEY = key;
            Mean = mean;
            SD = sd;
            ItemIDs = itemIDs;
        }
    }
}
