using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    public class StAnswer
    {
        public long RT = -1;
        public int Selected = -1;

        public StAnswer()
        { }

        public StAnswer(long rt, int selected)
        {
            RT = rt;
            Selected = selected;
        }
    }
}
