using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lithopone.Memory;

namespace Lithopone.FileReader
{
    public class ConfigCollection
    {
        public Dictionary<int, StDemogLine> mDemogLines;
        public StTestXmlHeader mTestInfo;
        public List<StNorm> mNorms;

        public ConfigCollection()
        {
            //demog structure
            Lithopone.FileReader.DemogXmlReader rd = new FileReader.DemogXmlReader();
            mDemogLines = rd.GetDemogItems(Lib.DemogFileName);
            //test structure
            TestXmlReader xmlReader = new TestXmlReader();
            xmlReader.Begin(Lib.TestFileName);
            mTestInfo = xmlReader.GetHeader();
            xmlReader.Finish();
            //norm structure
            Arora.AroraNormFactory nf = new Arora.AroraNormFactory();
            mNorms = nf.GetNorm();
        }
    }
}
