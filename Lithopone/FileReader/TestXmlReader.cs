using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Lithopone.Memory;
using System.IO;

namespace Lithopone.FileReader
{
    public class TestXmlReader
    {
        private XmlDocument mDoc;
        private FileStream mFs;

        public void Begin(String path)
        {
            mDoc = new XmlDocument();
            mFs = new FileStream(path, FileMode.Open, FileAccess.Read);
            mDoc.Load(mFs);
        }

        public void Finish()
        {
            mFs.Close();
        }

        private Dictionary<int, StSelection> GetSelections(XmlNode itemNode)
        {
            Dictionary<int, StSelection> retval = new Dictionary<int, StSelection>();

            XmlNodeList nl = itemNode.ChildNodes;

            string casual;
            int id = Int32.MaxValue, resId = Int32.MaxValue, value = Int32.MaxValue;

            //selections
            for (int i = 1; i < nl.Count; i++)
            {
                casual = nl[i].FirstChild.InnerText;

                XmlAttributeCollection ac =  nl[i].Attributes;
                //attributes in one selection
                for (int j = 0; j < ac.Count; j++)
                {
                    if(ac[j].Name.Equals("id"))
                    {
                        id = Int32.Parse(ac[j].Value);
                    }
                    else if(ac[j].Name.Equals("res_id"))
                    {
                        resId = Int32.Parse(ac[j].Value);
                    }
                    else if(ac[j].Name.Equals("value"))
                    {
                        value =  Int32.Parse(ac[j].Value);
                    }
                }

                retval.Add(id, new StSelection(resId, value, casual));
            }

            return retval;
        }

        public Dictionary<int, StItem> GetItems()
        {
            Dictionary<int, StItem> ret = new Dictionary<int, StItem>();

            XmlNodeList itemsNl = mDoc.SelectNodes("/test/item");
            XmlAttributeCollection ac;

            int id = Int32.MaxValue;
            int resId = Int32.MaxValue;
            string casual = "";

            //items
            for (int i = 0; i < itemsNl.Count; i++)
            {
                ac = itemsNl[i].Attributes;
                //attributes
                for (int j = 0; j < ac.Count; j++)
                {
                    if (ac[j].Name.Equals("id"))
                    {
                        id = Int32.Parse(ac[j].Value);
                    }
                    else if (ac[j].Name.Equals("res_id"))
                    {
                        resId = Int32.Parse(ac[j].Value);
                    }
                }
                //casual
                casual = itemsNl[i].FirstChild.InnerText;
                
                //get selections
                Dictionary<int, StSelection> sel = GetSelections(itemsNl[i]);

                ret.Add(id, new StItem(resId, casual, ref sel));
            }

            return ret;
        }

        public StTestXmlHeader GetHeader()
        {
            StTestXmlHeader ret = new StTestXmlHeader();

            XmlNode node = mDoc.SelectSingleNode("/test");
            XmlAttributeCollection ac = node.Attributes;

            string nameBuf, valueBuf;

            for (int i = 0; i < ac.Count; i++)
            {
                nameBuf = ac[i].Name;
                valueBuf = ac[i].Value;

                if (nameBuf.Equals("version"))
                {
                    ret.Version = Int32.Parse(valueBuf);
                }
                else if (nameBuf.Equals("text_selection"))
                {
                    ret.TextSelection = Boolean.Parse(valueBuf);
                }
                else if (nameBuf.Equals("text_casual"))
                {
                    ret.TextCausal = Boolean.Parse(valueBuf);
                }
                else if (nameBuf.Equals("selection_size"))
                {
                    ret.SelectionSize = Lib.Str2Size(valueBuf);
                }
                else if (nameBuf.Equals("name"))
                {
                    ret.Name = valueBuf;
                }
                else if (nameBuf.Equals("item_count"))
                {
                    ret.ItemCount = Int32.Parse(valueBuf);
                }
                else if (nameBuf.Equals("id"))
                {
                    ret.ID = Int32.Parse(valueBuf);
                }
                else if (nameBuf.Equals("graphic_selection"))
                {
                    ret.GraphicSelection = Boolean.Parse(valueBuf);
                }
                else if (nameBuf.Equals("graphic_casual"))
                {
                    ret.GraphicCasual = Boolean.Parse(valueBuf);
                }
                else if (nameBuf.Equals("description"))
                {
                    ret.Description = valueBuf;
                }
                else if (nameBuf.Equals("casual_size"))
                {
                    ret.CasualSize = Lib.Str2Size(valueBuf);
                }

            }

            return ret;
        }
    }
}
