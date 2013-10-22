using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lithopone.UIComponents
{
    /// <summary>
    /// CompTextCasual.xaml 的互動邏輯
    /// </summary>
    public partial class CompText : UserControl
    {
        public static int HEIGHT = 53;
        public int mWidth;
        public MainWindow mMW;

        public CompText()
        {
            InitializeComponent();
        }

        public CompText(MainWindow mw, int width)
        {
            InitializeComponent();
            BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            mMW = mw;
            mWidth = width;
        }

        public void SetText(String text)
        {
            amLabel.Text = text;
            amLabel.Width = mWidth;
            this.Width = mWidth;
        }
    }
}
