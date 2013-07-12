using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    public class StDemogLine
    {
        public String PreText;
        public String PostText;
        public LINETYPE Type;
        public Dictionary<int, String> SubItems;//only for comboBoxes and radio buttons
        public String VarName;
        public int Width = 0;
        public bool ForcedChoice = false;

        public StDemogLine()
        {
            SubItems = new Dictionary<int, String>();
        }
    }
}
