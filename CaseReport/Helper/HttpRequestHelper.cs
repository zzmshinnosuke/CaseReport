using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Controls;

namespace CaseReport.Helper
{
    class HttpRequestHelper
    {

        /// <summary>
        /// 根据control的name和对应value生成查询语句string，支持TextBox,PasswordBox
        /// </summary>
        /// <param name="controls"></param>
        /// <returns></returns>
        public static string GetParametersString()
        {
            string str = "";
            return str;
        }

        public static void AddParameter(ref string source, string name, string value)
        {
            source += "&" + name + "=" + value;
        }

        public static void AddParameter(ref string source, string name, int value)
        {
            source += "&" + name + "=" + value;
        }

        public static void AddParameter(ref string source, string value)
        {
            source += "&" + value;
        }

        /// <summary>
        /// 默认Request：Method=post，Accpet=json，charset=utf-8
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpWebRequest DefaultHttpRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "post";
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            return request;
        }

        /// <summary>
        /// 默认Request：Method=post，Accpet=json，charset=utf-8
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpWebRequest DefaultHttpRequest2(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "post";
            request.Accept = "application/json";
            request.ContentType = "multipart/form-data; charset=UTF-8";

            return request;
        }

        /// <summary>
        /// 写入Data
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="request"></param>
        public static void WritePostData(byte[] bytes, ref HttpWebRequest request)
        {
            try
            {

                request.ContentLength = bytes.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }
            catch
            {
                //MessageBox.Show("网络异常");

            }
        }

        public static void WritePostData_File(byte[] ss, byte[] file, ref HttpWebRequest request)
        {
            request.ContentLength = file.Length + ss.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(ss, 0, ss.Length);
            newStream.Write(file, 0, file.Length);
            newStream.Close();
        }
    }
}
