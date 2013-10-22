using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Lithopone.Memory;
using System.IO;
using System.Windows.Forms;

namespace Lithopone.FileReader
{
    class DemogXmlReader
    {
        private XmlDocument mDoc;
        private FileStream mFs;

        public Dictionary<int, StDemogLine> GetDemogItems(String filename)
        {
            Dictionary<int, StDemogLine> retval = null;
            try
            {
                mDoc = new XmlDocument();
                mFs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + filename,
                        FileMode.Open, FileAccess.Read);
                mDoc.Load(mFs);

                retval = new Dictionary<int, StDemogLine>();

                XmlNodeList nodeList = mDoc.SelectNodes("/DemographicSheet/Line");
                XmlAttributeCollection attrCollection = null;

                //line
                for (int i = 0; i < nodeList.Count; i++)
                {
                    attrCollection = nodeList[i].Attributes;
                    //attributes in line
                    StDemogLine line = new StDemogLine();
                    int lineID = -1;

                    for (int j = 0; j < attrCollection.Count; j++)
                    {
                        String attrName = attrCollection[j].Name;

                        if (attrName.Equals("id"))
                        {
                            lineID = Int32.Parse(attrCollection[j].Value);
                        }
                        else if (attrName.Equals("var_name"))
                        {
                            line.VarName = attrCollection[j].Value;
                        }
                        else if (attrName.Equals("line_type"))
                        {
                            line.Type = Lib.Str2LineType(attrCollection[j].Value);
                        }
                        else if (attrName.Equals("pre_text"))
                        {
                            line.PreText = attrCollection[j].Value;
                        }
                        else if (attrName.Equals("post_text"))
                        {
                            line.PostText = attrCollection[j].Value;
                        }
                        else if (attrName.Equals("width"))
                        {
                            line.Width = Int32.Parse(attrCollection[j].Value);
                        }
                        else if (attrName.Equals("forced_choice"))
                        {
                            line.ForcedChoice = Boolean.Parse(attrCollection[j].Value);
                        }
                    }

                    XmlNodeList selections = nodeList[i].ChildNodes;
                    for (int k = 0; k < selections.Count; k++)
                    {
                        int SelectionID = -1;
                        String SelectionText = null;
                        XmlAttributeCollection attrOfSelection = selections[k].Attributes;
                        for (int l = 0; l < attrOfSelection.Count; l++)
                        {
                            if (attrOfSelection[l].Name.Equals("id"))
                            {
                                SelectionID = Int32.Parse(attrOfSelection[l].Value);
                            }
                            else if (attrOfSelection[l].Name.Equals("text"))
                            {
                                SelectionText = attrOfSelection[l].Value;
                            }
                        }

                        line.SubItems.Add(SelectionID, SelectionText);
                    }

                    retval.Add(lineID, line);
                }
            }
            catch (Exception)
            {
                if (MessageBox.Show("人口学数据读取出错") == DialogResult.OK)
                {
                    if (mFs != null)
                        mFs.Close();
                    System.Environment.Exit(0);
                }
            }

            if(mFs != null)
                mFs.Close();

            return retval;
        }
    }
}
