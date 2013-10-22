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
using Lithopone.Kernel;

namespace Lithopone.UIComponents
{
    /// <summary>
    /// CompInstructionPage.xaml 的互動邏輯
    /// </summary>
    public partial class CompInstructionPage : UserControl
    {
        public TestRunner mTestRunner;
        public CompInstructionPage(TestRunner tr, int sizeX, int sizeY)
        {
            InitializeComponent();
            mTestRunner = tr;

            this.Width = sizeX;
            this.Height = sizeY;

            amTitleLabel.Width = sizeX;
            amInstructionText.Width = sizeX;
            amInstructionText.Height = sizeY - 52 - 2 * 10;
            Canvas.SetTop(amOKBtn, sizeY - 52 - 20);
            Canvas.SetLeft(amOKBtn, (sizeX - 122) / 2);
        }

        private void amOKBtn_Click(object sender, RoutedEventArgs e)
        {
            mTestRunner.StartTest();
        }
    }
}
