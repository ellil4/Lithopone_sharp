using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    public class StNormAuto
    {
        public List<StNormDim> Dims;
        public StNormValidity Validity;

        public StNormAuto()
        {
            Dims = new List<StNormDim>();
            Validity = new StNormValidity();
        }
    }
}
