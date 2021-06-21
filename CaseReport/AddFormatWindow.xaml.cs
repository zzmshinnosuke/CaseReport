using CaseReport.Helper;
using CaseReport.MyClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CaseReport
{
    /// <summary>
    /// AddFormatWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddFormatWindow : Window
    {

        public AddFormatWindow()
        {
            InitializeComponent();
        }

        //确定按钮
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //XmlHelper.DeleteCaseName(currentCase.name);
            String casename = this.casename_label2.Text;
            String content = this.columnname_label2.Text;
            XmlHelper.AddCaseName(casename);
            String[] s = content.Split(new char[] { ' ' });
            foreach (string columnname in s)
            {
                XmlHelper.AddColumnName(casename,columnname,"int"); 
            }
            this.DialogResult = true;         
        }

        //取消按钮
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true; 
        }
    }
}
