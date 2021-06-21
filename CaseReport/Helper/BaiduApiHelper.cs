using CaseReport.MyClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseReport.Helper
{
    public class BaiduApiHelper
    {
        private string access_token = "";
        public BaiduApiHelper(string client_id, string client_secret)
        {
            //检查会议是否需要密码接口
            string url = PublicData.Url_token;

            //生成默认请求
            try
            {
                System.Net.HttpWebRequest request = HttpRequestHelper.DefaultHttpRequest(url);
                string postString = HttpRequestHelper.GetParametersString();

                HttpRequestHelper.AddParameter(ref postString, "grant_type", "client_credentials");
                HttpRequestHelper.AddParameter(ref postString, "client_id", client_id);
                HttpRequestHelper.AddParameter(ref postString, "client_secret", client_secret);

                byte[] post = Encoding.UTF8.GetBytes(postString);
                HttpRequestHelper.WritePostData(post, ref request);
                dynamic obj = HttpResponseHelper.GetResponseObj(request);
                access_token = obj.access_token;
            }
            catch (Exception ee)
            {
                System.Console.Out.WriteLine("BaiduApiHelper Exception:"+ee.ToString());
                access_token = "";

            }
        }

        public dynamic Ocr(string imagebase64)
        {
            string url = PublicData.Url_general_base;      
            try
            {
                System.Net.HttpWebRequest request = HttpRequestHelper.DefaultHttpRequest(url);
                string postString = HttpRequestHelper.GetParametersString();

                HttpRequestHelper.AddParameter(ref postString, "access_token", access_token);
                HttpRequestHelper.AddParameter(ref postString, "image", imagebase64);
               
                byte[] post = Encoding.Default.GetBytes(postString);
                //string pos = Encoding.UTF8.GetString(post);
                HttpRequestHelper.WritePostData(post, ref request);
                dynamic obj = HttpResponseHelper.GetResponseObj(request);
                long words_result_num = obj.words_result_num;
                System.Console.Out.WriteLine("words_result_num："+words_result_num);
                return obj;
            }
            catch (Exception ee)
            {
                System.Console.Out.WriteLine("OCR Exception:" + ee.ToString());
                return null;
            }

        }
    }
}
