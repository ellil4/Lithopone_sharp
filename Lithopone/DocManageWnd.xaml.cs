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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Arora;
using Lithopone.FileReader;
using LibTabCharter;
using System.IO;

namespace Lithopone
{
    /// <summary>
    /// DocManageWnd.xaml 的互動邏輯
    /// </summary>
    public partial class DocManageWnd : Window
    {
        public ConfigCollection mConfigs;
        public DocManageWnd(ConfigCollection configs)
        {
            InitializeComponent();
            mConfigs = configs;
        }

        private ObservableCollection<Memory.StDocManageBriefInfo> readRecBriefList()
        {
            ObservableCollection<Memory.StDocManageBriefInfo> list =
                new ObservableCollection<Memory.StDocManageBriefInfo>();
            if (System.IO.File.Exists(Arora.AroraCore.OUT_PATH))
            {
                LibTabCharter.TabFetcher fetcher =
                    new LibTabCharter.TabFetcher(Arora.AroraCore.OUT_PATH, "\\t");

                List<String> lineBuf = null;

                fetcher.Open();
                fetcher.GetLineBy();//skip first line

                while ((lineBuf = fetcher.GetLineBy()).Count != 0)
                {
                    Memory.StDocManageBriefInfo lineSt = new Memory.StDocManageBriefInfo();
                    lineSt.Name = lineBuf[0];
                    lineSt.Number = lineBuf[6];
                    lineSt.Stamp = lineBuf[7];
                    list.Add(lineSt);
                }

                fetcher.Close();
            }

            return list;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            amDataGrid.DataContext = readRecBriefList();
            amDataGrid.Items.Refresh();
        }

        private void amBtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void amBtnDel_Click(object sender, RoutedEventArgs e)
        {
            int index = amDataGrid.SelectedIndex;
            
            if (index != -1)
            {
                //read
                List<List<String>> table = new List<List<string>>();
                TabFetcher fet = new TabFetcher(AroraCore.OUT_PATH, "\\t");
                fet.Open();
                List<string> lineBuf = null;
                while ((lineBuf = fet.GetLineBy()).Count != 0)
                {
                    table.Add(lineBuf);
                }
                fet.Close();
                //remove
                table.RemoveAt(index + 1);
                //write
                File.Delete(AroraCore.OUT_PATH);
                TabCharter charter = new TabCharter(AroraCore.OUT_PATH);
                charter.Create(table[0]);
                for (int i = 1; i < table.Count; i++)
                {
                    charter.Append(table[i]);
                }

                amDataGrid.DataContext = readRecBriefList();
            }
        }

        private void amBtnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (amDataGrid.SelectedIndex != -1)
            {
                AroraReport rep = new AroraReport(mConfigs.mDemogLines.Count, mConfigs.mTestInfo.ItemCount,
                            mConfigs.mNorms, amDataGrid.SelectedIndex + 1);

                rep.DoReport();
            }
        }
    }
}
