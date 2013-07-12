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
    /// UserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class CompButton : UserControl
    {
        //private TestRunner mRunner;
        public delegate void PressedFunc();
        public static int HEIGHT = 35;
        public static int WIDTH = 100;

        private PressedFunc mfPressed;

        public CompButton(String text)
        {
            InitializeComponent();

            amButton.Content = text;
        }

        public void SetBehavior(PressedFunc func)
        {
            mfPressed = func;
        }

        private void amButton_Click(object sender, RoutedEventArgs e)
        {
            mfPressed();
        }
    }
}
