using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    //class to define the relationship of the value and description
    public class StNormLevel
    {
        public int ID;//level ID
        public double FloorWith;//start value (included)
        public double CeilingWithout;//end value (not included)
        public string Description;//description
    }
}
