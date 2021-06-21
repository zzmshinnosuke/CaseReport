using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using CaseReport.Helper;
using CaseReport.MyClass;
namespace CaseReport.Helper
{
    class DataProcessHelper
    {
        BaiduApiHelper bah = new BaiduApiHelper(PublicData.API_KEY, PublicData.SECRET_KEY);
        /// <summary>
        /// 识别指定路径图片中的文字并返回
        /// </summary>
        /// <param name="filePath">本地文件路径</param>
        /// <returns>文字识别结果字符串</returns>
        public String Convert(String filePath)
        {
            
            dynamic result = bah.Ocr(Base64Helper.ImageToBase64(filePath));
            int num = (int)result.words_result_num;
            String words = null;
            for (int i = 0; i < num; i++)
            {
                words = words + result.words_result[i].words;
            }
            return words;
        }
        /// <summary>
        /// 将识别出的文字信息提取出并转化为datatable格式供datagrid显示
        /// </summary>
        /// <param name="words">图片识别结果</param>
        /// /// <param name="columnNames">该病理类型所包含的列名</param>
        /// /// <param name="dt">datatable类型的数据表</param>
        /// /// <param name="selectedRow">鼠标选中要继续添加数据的行</param>
        /// <returns>datatable数据表</returns>
        public DataTable getDataTable(String words, List<string> columnNames, DataTable dt,DataRowView selectedRow)
        {
            bool flag = false;//判断此行的数据是否有效
            DataRow dr = dt.NewRow();
            if (selectedRow != null)
            {
                dr = selectedRow.Row;
                //selectedRow.Delete();//删除
            }
            //循环对每个列明所要提取的数据进行检索
            foreach (string columnName in columnNames)
            {
                Regex reg = new Regex("");
                MatchCollection matches = reg.Matches(words);
                String value = "";
                String[] values = {""};
                String columnName2 = "";
                try
                {
                    switch (columnName)
                    {
                        case "病理号":
                            reg = new Regex("(?<=(" + columnName + ":))[0-9]+");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[0].Value;
                            break;
                        case "病案号":
                            reg = new Regex("(?<=(" + columnName + ":))[0-9]+");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[0].Value;
                            break;
                        case "姓名":
                            reg = new Regex(@"姓名:(.+)性别");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value;
                            break;
                        case "性别":
                            reg = new Regex(@"性别:(.+)年龄");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value;
                            break;
                        case "年龄":
                            reg = new Regex(@"年龄:(.+)科室");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value;
                            break;
                        case "科室":
                            reg = new Regex(@"科室:(.+)科别");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value;
                            break;
                        case "科别":
                            reg = new Regex(@"科别:(.+)送检");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value;
                            break;
                        case "标本类型":
                            reg = new Regex(@"材料:(.+)床号");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value;
                            break;
                        case "床号":
                            reg = new Regex(@"床号:(.+)收到");
                            value = matches[0].Groups[1].Value;
                            break;
                        case "收到日期":
                            reg = new Regex(@"(?<=收到日期:)([^\u4e00-\u9fa5]+)");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[0].Value;
                            value = Regex.Replace(value, @"[^0-9]+", "");//更改日期格式
                            value = Regex.Replace(value, @"(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})", "$1-$2-$3 $4:$5");
                            break;
                        case "诊断日期":
                            reg = new Regex(@"(?<=诊断日期:)([^\u4e00-\u9fa5]+)");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[0].Value;
                            value = Regex.Replace(value, @"[^0-9]+", "");//更改日期格式
                            value = Regex.Replace(value, @"(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})", "$1-$2-$3 $4:$5");
                            break;
                        case "肿瘤大小1":
                            reg = new Regex(@"大小([^cm]*?)cm");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value;
                            values = value.Split(new char[] { '×' });
                            value = values[0];
                            dr[columnName] = values[0];
                            columnName2 = columnName.Substring(0, columnName.Length - 1)+"2";
                            dr[columnName2] = values[1];
                            columnName2 = columnName2.Substring(0, columnName.Length - 1)+"3";
                            dr[columnName2] = values[2];
                            String test = dr[columnName2].ToString();
                            break;
                        case "网膜大小1":
                            reg = new Regex(@"大小([^cm]*?)cm");
                            matches = reg.Matches(words);
                            value = matches[1].Groups[1].Value;
                            values = value.Split(new char[] { '×' });
                            value = values[0];
                            dr[columnName] = values[0];
                            columnName2 = columnName.Substring(0, columnName.Length - 1)+"2";
                            dr[columnName2] = values[1];
                            columnName2 = columnName2.Substring(0, columnName.Length - 1)+"3";
                            dr[columnName2] = values[2];
                            break;
                        case "肿瘤最大径":
                            reg = new Regex(@"最大径([^cm]*?)cm");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value + "cm";
                            break;
                        case "核分裂":
                            reg = new Regex(@"核分裂([^H]*?)HPF");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value + "HPF";
                            break;
                        case "危险度分级":
                            reg = new Regex(@"危险度分级:([^。]*?)。");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value;
                            break;
                        case "肿瘤位置":
                            reg = new Regex(@"结果符合([^,]*?),");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value;
                            break;
                        default:
                            if (!dr.IsNull(columnName) && dr[columnName].ToString() != "") break;
                            reg = new Regex(@"(?<=" + columnName + ")\\((.*?)\\)");
                            matches = reg.Matches(words);
                            value = matches[0].Groups[1].Value;
                            break;
                    }
                }
                catch { }
                //Regex reg = new Regex(@"(?<="+columnName+")\\((.*?)\\)"); //识别免疫组化结果显示 Groups[1]
                //Regex reg = new Regex(@"大小([^cm]*?)cm");  //识别大小 用到Matches来区分识别到的第一第二个结果，考虑后边是否加cm
                //Regex reg = new Regex("(?<=(" + columnName + ":))[0-9]+"); //识别数字 Groups[0](.*?)
                //Regex reg = new Regex(@"(?<=收到日期:)([^\u4e00-\u9fa5]+)");//\d{4}-\d{2}-\d{4}:\d{2}
                //MatchCollection matches = reg.Matches(words);
                //string value = matches[0].Groups[0].Value;
                //value = Regex.Replace(value, @"[^0-9]+", "");//更改日期格式
                //value = Regex.Replace(value, @"(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})", "$1-$2-$3 $4:$5");//更改日期格式
                if (!dr.IsNull(columnName) && dr[columnName].ToString() != "") continue;
                if (value != null && value != "") flag = true; //有一列数据不为空则改行数据有效
                dr[columnName] = value;
            }
            //if(selectedRow!=null) selectedRow.Delete();
            if(selectedRow==null&&flag == true) dt.Rows.Add(dr);
            if (flag == false) MessageBox.Show("识别不到有效内容，请更换图片！");

            return dt;
        }
        
    }
}
