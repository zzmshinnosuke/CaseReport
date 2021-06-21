using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseReport.MyClass
{
    public class CaseFormat
    {
        public CaseFormat()
        {
            ColumnName = new List<string>();
        }
        public string name; //生成excel的文件名
        public List<string> ColumnName; //生成excel中每一列的名称，并用做图片生成文字后处理的关键字
    }
}
