using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    public class StGeneralInfo
    {
        public String Name;
        public String Value;

        public StGeneralInfo(String value, String name)
        {
            Name = name;
            Value = value;
        }
    }
}
