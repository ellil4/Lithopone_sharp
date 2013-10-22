using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    //class to check if the test users` result is legal
    public class StNormValidity
    {
        public List<int> ItemIndex;//validity verification item list
        public ValidityPrincipal Principal;//how to verify the validity
        public int Tolerance;//how many times` breaking validity pricipal is allowed

        public StNormValidity()//constructor
        {
            ItemIndex = new List<int>();
            Principal = ValidityPrincipal.None;
            Tolerance = -1;
        }
    }

    public enum ValidityPrincipal
    {
        None, ValueEqual, IndexEqual
    }
}
