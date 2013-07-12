using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    public class StItem
    {
        public int ResID;
        public string Casual;
        public Dictionary<int, StSelection> Selections;

        public StItem(int ri, string cas, ref Dictionary<int, StSelection> sel)
        {
            ResID = ri;
            Casual = cas;
            Selections = sel;
        }
    }
}
