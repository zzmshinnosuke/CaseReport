using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;

namespace CaseReport.Helper
{
    class HttpResponseHelper
    {
        /*
        返回结果结构说明：
        obj.data_res.code描述接口访问结果，200表示成功，其他代码自行通过接口测试工具来获取结果
        */
        public static dynamic GetResponseObj(HttpWebRequest request)
        {
            HttpWebResponse response;
            dynamic obj = null;
            try
            {

                response = (HttpWebResponse)request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                var json = reader.ReadToEnd();
                obj = JsonConvert.DeserializeObject<ExpandoObject>(json);
            }
            catch
            {
                //MessageBox.Show("网络异常");
            }
            return obj;
        }


        public static string GetResponseObjByte(HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

            string rlt = reader.ReadToEnd();
            return rlt;
        }
    }
}
