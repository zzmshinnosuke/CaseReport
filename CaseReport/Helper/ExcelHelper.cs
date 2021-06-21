using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Web;
using System.Windows;
namespace CaseReport.Helper
{
    
    class ExcelHelper
    {
        public static string SaveExcelFileDialog()
        {
            var sfd = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = "xls",
                Filter = "excel files(*.xls)|*.xls|All files(*.*)|*.*",
                FilterIndex = 1
            };

            if (sfd.ShowDialog() != true)
                return null;
            return sfd.FileName;
        }
        /// <summary>
        /// 传入datatable数据表以生成Excel
        /// </summary>
        /// <param name="datatable">从图片中提取出的有效信息</param>
        public void ExportExcel(DataTable dt)
        {
            try
            {
                var filepath = SaveExcelFileDialog();
                //创建一个工作簿
                IWorkbook workbook = new HSSFWorkbook();

                //创建一个 sheet 表
                ISheet sheet = workbook.CreateSheet(dt.TableName);

                //创建一行
                IRow rowH = sheet.CreateRow(0);

                //创建一个单元格
                ICell cell = null;

                //创建单元格样式
                ICellStyle cellStyle = workbook.CreateCellStyle();

                //创建格式
                IDataFormat dataFormat = workbook.CreateDataFormat();

                //设置为文本格式，也可以为 text，即 dataFormat.GetFormat("text");
                cellStyle.DataFormat = dataFormat.GetFormat("@");

                //设置列名
                foreach (DataColumn col in dt.Columns)
                {
                    //创建单元格并设置单元格内容
                    rowH.CreateCell(col.Ordinal).SetCellValue(col.Caption);

                    //设置单元格格式
                    rowH.Cells[col.Ordinal].CellStyle = cellStyle;
                }

                //写入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //跳过第一行，第一行为列名
                    IRow row = sheet.CreateRow(i + 1);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                        cell.CellStyle = cellStyle;
                    }
                }

                //设置导出文件路径
                //string path = HttpContext.Current.Server.MapPath("Export/");
                string path = filepath;
                //设置新建文件路径及名称
                string savePath = path;

                //创建文件
                FileStream file = new FileStream(savePath, FileMode.CreateNew, FileAccess.Write);

                //创建一个 IO 流
                MemoryStream ms = new MemoryStream();

                //写入到流
                workbook.Write(ms);

                //转换为字节数组
                byte[] bytes = ms.ToArray();

                file.Write(bytes, 0, bytes.Length);
                file.Flush();

                //还可以调用下面的方法，把流输出到浏览器下载
                //OutputClient(bytes);

                //释放资源
                bytes = null;

                ms.Close();
                ms.Dispose();

                file.Close();
                file.Dispose();

                workbook.Close();
                sheet = null;
                workbook = null;
                MessageBox.Show("导出成功!");
            }
            catch (Exception)
            {
                MessageBox.Show("导出失败!");
            }
        }
    }
}
