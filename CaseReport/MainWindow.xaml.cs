using CaseReport.EventPool;
using CaseReport.Helper;
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
using System.Windows.Threading;

namespace CaseReport
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /*public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(delegate(object f)
                {
                    ((DispatcherFrame)f).Continue = false;

                    return null;
                }
                    ), frame);
            Dispatcher.PushFrame(frame);
        }*/

        public MainWindow()
        {
            InitializeComponent();
            MainWindowEvent.GetInstance().MainResponseEvent += MainPageResponseEvent;
            //XmlHelper.AddCaseName("红烧豆腐");
            //XmlHelper.AddColumnName("红烧豆腐", "日本豆腐","int");
            //XmlHelper.ChangeCaseName("红烧豆腐","豆腐");
           // XmlHelper.DeleteCaseName("豆腐");
           // XmlHelper.ChangeColumnName("红烧豆腐", "日本豆腐", "dd", "int", "double");
           // test();
            loadCase();
        }

        void MainPageResponseEvent(MainMessageType msgType)
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                switch (msgType)
                {
                    case MainMessageType.UpdateCase:
                        updatePage();
                        break;
                }
            }));
        }

        private void loadCase()
        {
            List<CaseFormat> CFList = XmlHelper.getAllCaseFormat();
            foreach (CaseFormat cf in CFList)
            {
                CasePage casePage = new CasePage(cf);
                Frame frame = new Frame();
                frame.Content = casePage;
                this.caseListBox1.Items.Add(frame);
            }
        }

        private void test()
        {
            BaiduApiHelper bah = new BaiduApiHelper(PublicData.API_KEY, PublicData.SECRET_KEY);
            bah.Ocr(Base64Helper.ImageToBase64("D://aa.jpg"));
            XmlHelper.getAllCaseFormat();
        }

        private void updatePage()
        {
            this.caseListBox1.Items.Clear();
            loadCase();
        }

        private void btn_addCase_Click(object sender, RoutedEventArgs e)
        {          
            AddFormatWindow win = new AddFormatWindow();
            win.ShowDialog();
            updatePage();
        }
    }
}
