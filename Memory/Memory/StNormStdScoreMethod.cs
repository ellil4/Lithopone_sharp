using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    //class to define the method of making standard score
    public class StNormStdScoreMethod
    {
        //the methods are to be used to make the standard score
        public List<char> Method;
        //the values are to be used to make the standard score
        public List<double> Values;

        public StNormStdScoreMethod()
        {
            Method = new List<char>();
            Values = new List<double>();
        }
    }
}
