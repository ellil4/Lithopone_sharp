using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    public class StNormAuto
    {
        List<StNormDim> Dims;
        StNormValidity Validity;

        public StNormAuto()
        {
            Dims = new List<StNormDim>();
            Validity = null;
        }
    }
}
