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
    /// ThanksPage.xaml 的互動邏輯
    /// </summary>
    public partial class ThanksPage : UserControl
    {
        MainWindow mMainWindow;
        public ThanksPage(MainWindow mw)
        {
            InitializeComponent();
            mMainWindow = mw;
        }

        private void amOKBtn_Click(object sender, RoutedEventArgs e)
        {
            mMainWindow.OnThanksClose();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mMainWindow.centralize(this, 586, 405);
        }
    }
}
