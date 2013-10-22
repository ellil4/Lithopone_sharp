using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Lithopone.Kernel
{
    public class KernelLib
    {

        public static int LINE_HEIGHT = 35;
        public static int CHARA_WIDTH = 18;
        public static int DEMOG_FONT_SIZE = 18;

        public static int STACKPANE_WIDTH = 760;

        public static int LINE_GAP = 5;

        public static StackPanel GenLinePane()
        {
            StackPanel retval = new StackPanel();
            retval.Height = LINE_HEIGHT;
            retval.Width = STACKPANE_WIDTH;

            retval.Orientation = Orientation.Horizontal;

            return retval;
        }
    }
}
