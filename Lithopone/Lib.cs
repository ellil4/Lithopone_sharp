using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using Lithopone.Memory;

namespace Lithopone
{
    public class Lib
    {
        public const String TestFileName = "tsource";
        public const String DemogFileName = "dsource";

        public static LINETYPE Str2LineType(String typeStr)
        {
            LINETYPE retval = LINETYPE.UNKNOW;
            if (typeStr.Equals("RADIO"))
            {
                retval = LINETYPE.RADIO;
            }
            else if (typeStr.Equals("COMBO"))
            {
                retval = LINETYPE.COMBO;
            }
            else if (typeStr.Equals("TXTFIELD"))
            {
                retval = LINETYPE.TXTFIELD;
            }

            return retval;
        }

        public static GRAPH_SIZE Str2Size(string src)
        {
            GRAPH_SIZE ret = GRAPH_SIZE.UNKNOWN;

            if (src.Equals("SMALL"))
                ret =  GRAPH_SIZE.SMALL;
            else if (src.Equals("MEDIUM"))
                ret =  GRAPH_SIZE.MEDIUM;
            else if (src.Equals("LARGE"))
                ret = GRAPH_SIZE.LARGE;

            return ret;
        }

        public static int Size2Number(GRAPH_SIZE size)
        {
            int retval = -1;

            switch (size)
            {
                case GRAPH_SIZE.SMALL:
                    retval = 50;
                    break;
                case GRAPH_SIZE.MEDIUM:
                    retval = 100;
                    break;
                case GRAPH_SIZE.LARGE:
                    retval = 200;
                    break;
            }

            return retval;
        }
    }
}
