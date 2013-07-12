using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    public class StSelection
    {
        public int ResID;
        public float Value;
        public string Casual;

        public StSelection(int resId, float value, string casual)
        {
            ResID = resId;
            Value = value;
            Casual = casual;
        }
    }
}
