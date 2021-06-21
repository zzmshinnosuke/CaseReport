using CaseReport.EventPool;
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
    /// ChangeFormatWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeFormatWindow : Window
    {
        CaseFormat currentCase; //当前病理信息

        public ChangeFormatWindow(CaseFormat cf)
        {
            InitializeComponent();
            this.casename_label.Text =cf.name;
            String allnames = "";
            if (cf.ColumnName != null)
            {
                foreach (var name in cf.ColumnName)
                {
                    allnames = allnames + name + " ";
                }
            }
            allnames = allnames.Substring(0, allnames.Length - 1);
            this.columnname_label.Text = allnames;
            currentCase = cf;
        }



        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        //确定按钮
        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            //XmlHelper.DeleteCaseName(currentCase.name);
            String casename = this.casename_label.Text;
            XmlHelper.ChangeCaseName(currentCase.name,casename);
            String content = this.columnname_label.Text;
            String[] s = content.Split(new char[] { ' ' });
            foreach (String columnname in currentCase.ColumnName)
            {
                XmlHelper.DeleteColumnName(casename, columnname);
            }
            foreach (string columnname in s)
            {

                XmlHelper.AddColumnName(casename,columnname,"int"); 
            }
            MainWindowEvent.GetInstance().MainResponse(MainMessageType.UpdateCase);
            this.DialogResult = true; 
            
        }

        //取消按钮
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true; 
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            XmlHelper.DeleteCaseName(currentCase.name);
            MainWindowEvent.GetInstance().MainResponse(MainMessageType.UpdateCase);
            this.DialogResult = true;
        }
    }
}
