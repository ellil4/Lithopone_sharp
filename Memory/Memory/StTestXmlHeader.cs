using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithopone.Memory
{
    public class StTestXmlHeader
    {
        public int Version;
        public int ID;
        public String Name;
        public String Description;

        public bool TextSelection;
        public bool TextCausal;
        public bool GraphicSelection;
        public bool GraphicCasual;
        public GRAPH_SIZE SelectionSize;
        public GRAPH_SIZE CasualSize;

        public int ItemCount;
    }
}
