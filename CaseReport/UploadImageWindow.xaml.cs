using System;
using CaseReport.MyClass;
using CaseReport.Helper;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Data.Sql;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
namespace CaseReport
{
    /// <summary>
    /// UploadImageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UploadImageWindow : Window
    {
        CaseFormat currentcase;
        DataTable dt = new DataTable("病历");
        public UploadImageWindow(CaseFormat cf)
        {
            InitializeComponent();
            currentcase = cf;
            //Dictionary<String, String> start = new Dictionary<string,string>();
            //dt.Columns.Add("追加", typeof(Button));
            foreach (String columnName in currentcase.ColumnName)
            {
                //start.Add(columnName, "");
                dt.Columns.Add(columnName, typeof(string));
            }
            this.data.ItemsSource = dt.DefaultView;
        }    

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            //创建＂打开文件＂对话框
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            //设置文件类型过滤
            dlg.Filter = "图片|*.jpg;*.png;*.gif;*.bmp;*.jpeg";

            // 调用ShowDialog方法显示＂打开文件＂对话框
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                //获取所选文件名并在FileNameTextBox中显示完整路径
                string filename = dlg.FileName;
                FileNameTextBox.Text = filename;

                //在image1中预览所选图片
                BitmapImage image = new BitmapImage(new Uri(filename));
                image1.Source = image;
                image1.Width = scroll.Width-20;
                image1.Height = image.Height*scroll.Width/image.Width;
            }
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
                UploadIMG();                
        }

        private void UploadIMG()
        {
            var add = this.data.SelectedItem;
            var selectedRow = add as DataRowView;
            string fileName = System.IO.Path.GetFileNameWithoutExtension(FileNameTextBox.Text);
            DataProcessHelper dph = new DataProcessHelper();
            String words = dph.Convert(FileNameTextBox.Text);  //从图片中识别出的文字
            try
            {
                dt = dph.getDataTable(words, currentcase.ColumnName, dt, selectedRow);
            }
            catch
            {
                MessageBox.Show("该图片识别有误，请重新上传");
            }
            this.data.ItemsSource = dt.DefaultView;
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ExcelHelper eh = new ExcelHelper();
            eh.ExportExcel(dt);
            FileNameTextBox.Text = string.Empty;
        }
     }
}
