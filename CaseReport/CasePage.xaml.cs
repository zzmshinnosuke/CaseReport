using CaseReport.MyClass;
using System;
using System.Collections.Generic;
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

namespace CaseReport
{
    /// <summary>
    /// CasePage.xaml 的交互逻辑
    /// </summary>
    public partial class CasePage : Page
    {
        CaseFormat currentCase;
        public CasePage(CaseFormat cf)
        {
            InitializeComponent();
            this.lb_name.Content = cf.name;
            currentCase = cf;
        }

        private void btn_change_Click(object sender, RoutedEventArgs e)
        {
            ChangeFormatWindow win = new ChangeFormatWindow(currentCase);
            win.ShowDialog();
        }

        private void btn_upload_Click(object sender, RoutedEventArgs e)
        {
            UploadImageWindow win = new UploadImageWindow(currentCase);
            win.ShowDialog();
        }
    }
}
