using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseReport.Helper
{
    public class Base64Helper
    {
        public static  string ImageToBase64(string filePath)
        {
            //读图片转为Base64String
            string UserPhoto=null;
            System.Drawing.Bitmap bmp1 = new System.Drawing.Bitmap(filePath);
            using (MemoryStream ms1 = new MemoryStream())
            {
                bmp1.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr1 = new byte[ms1.Length];
                ms1.Position = 0;
                ms1.Read(arr1, 0, (int)ms1.Length);
                ms1.Close();
                //UserPhoto = Convert.ToBase64String(arr1);
                //byte[] buffer = Convert.FromBase64String(imagebase64);
                UserPhoto = Convert.ToBase64String(arr1).Replace("+", "%2B");//注意加号（’+‘）的替换处理，否则由于加号经过Url传递后变成空格而得不到合法的Base64字符串
            }
            return UserPhoto;
        }

        public static void Base64ToImage(string UserPhoto, string fileDir, string filename, string fileFormat = "jpg")
        {
            //将Base64String转为图片并保存
            byte[] arr2 = Convert.FromBase64String(UserPhoto);
            using (MemoryStream ms2 = new MemoryStream(arr2))
            {
                System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(ms2);
                switch (fileFormat)
                {
                    case "jpg":
                        bmp2.Save(fileDir + filename + "." + fileFormat, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "bmp":
                        bmp2.Save(fileDir + filename + "." + fileFormat, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "gif":
                        bmp2.Save(fileDir + filename + "." + fileFormat, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "png":
                        bmp2.Save(fileDir + filename + "." + fileFormat, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default :
                        bmp2.Save(fileDir + filename + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                }
            }
        }
    }
}
