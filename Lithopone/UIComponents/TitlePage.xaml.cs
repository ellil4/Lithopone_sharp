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
    /// TitlePage.xaml 的互動邏輯
    /// </summary>
    public partial class TitlePage : UserControl
    {
        public MainWindow mMainWindow;
        public TitlePage(MainWindow mw, int sizeX, int sizeY)
        {
            InitializeComponent();
            mMainWindow = mw;
            amTitleBlock.Width = sizeX;
            amRichTxt.Width = sizeX;
            amRichTxt.Height = sizeY - 12 - 56 - 45 - 15;
            Canvas.SetTop(button1, sizeY - 56);
            Canvas.SetLeft(button1, (sizeX - 116) / 2);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            mMainWindow.OnTitleClose();
        }
    }
}
