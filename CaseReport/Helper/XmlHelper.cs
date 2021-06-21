using CaseReport.MyClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CaseReport.Helper
{
    public class XmlHelper
    {
        static String dir = AppDomain.CurrentDomain.BaseDirectory;
        static String filename = "CaseFormat.xml";
        static String filePath =dir+filename;
        /// <summary>
        /// 创建保存生成excel文件格式的xml文件
        /// </summary>
        /// <returns></returns>
        private static bool CreateXml()
        {
            FileInfo fi = new FileInfo(filename);
            if (!Directory.Exists(dir))
            {
                new DirectoryInfo(dir).Create();
            }
            try
            {
                if (!File.Exists(filePath))  //如果不存在
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNode xmlnode = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                    xmlDoc.AppendChild(xmlnode);
                    //创建根节点  
                    XmlNode root = xmlDoc.CreateElement("CaseName");
                    xmlDoc.AppendChild(root);
                    try
                    {
                        xmlDoc.Save(filePath);
                    }
                    catch (Exception e)
                    {
                        //显示错误信息  
                        Console.WriteLine(e.Message);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取所有的excel文件格式
        /// </summary>
        /// <returns>病理报告格式list</returns>
        public static  List<CaseFormat> getAllCaseFormat()
        {
            List<CaseFormat> cfList = new List<CaseFormat>();
            try
            {
                if (!File.Exists(filePath))  //如果不存在
                {
                    CreateXml();
                    return null;
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNodeList nodelist = doc.SelectSingleNode("CaseName").ChildNodes;
                foreach (XmlNode xnode in nodelist)
                {
                    if (xnode.Name == "Case")
                    {
                        CaseFormat cf = new CaseFormat();
                        cf.name = xnode.Attributes["name"].Value.ToString();

                        //XmlNodeList columnList = xnode.ChildNodes;
                        //foreach (XmlNode clnode in columnList)
                        //{
                        //    string columnName=clnode
                        //}
                        foreach (XmlElement e in ((XmlElement)xnode).GetElementsByTagName("ColumnName"))
                        {
                            cf.ColumnName.Add(e.GetAttribute("name"));
                        }

                        cfList.Add(cf);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return cfList;
        }

        /// <summary>
        /// 添加excel文件名，也就是病理名称
        /// </summary>
        /// <param name="caseName"></param>
        public static void AddCaseName(string caseName)
        {
            try
            {
                if (!File.Exists(filePath))  //如果不存在
                {
                    CreateXml();
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNodeList nodelist = doc.SelectSingleNode("CaseName").ChildNodes;
                foreach (XmlNode xnode in nodelist)
                {
                    if (xnode.Name == "Case" && xnode.Attributes["name"].Value.ToString() == caseName)
                    {
                        return;
                    }
                }
                XmlElement RootElement = (XmlElement)doc.SelectSingleNode("CaseName");
                XmlElement CaseNameElement=doc.CreateElement("Case");
                CaseNameElement.SetAttribute("name",caseName);
                RootElement.AppendChild(CaseNameElement);
                doc.Save(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("AddCaseName Exception:  "+ e.ToString());
                return;
            }
        }

        /// <summary>
        /// 修改病理名称
        /// </summary>
        /// <param name="oldCaseName"></param>
        /// <param name="newCaseName"></param>
        public static void ChangeCaseName(string oldCaseName,string newCaseName)
        {
            try
            {
                if (!File.Exists(filePath))  //如果不存在
                {
                    CreateXml();
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNodeList nodelist = doc.SelectSingleNode("CaseName").ChildNodes;
                foreach (XmlNode xnode in nodelist)
                {
                    if (xnode.Name == "Case" && xnode.Attributes["name"].Value.ToString() == oldCaseName)
                    {
                        ((XmlElement)xnode).SetAttribute("name",newCaseName);
                        doc.Save(filePath);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ChangeCaseName Exception:  " + e.ToString());
                return;
            }
        }

        /// <summary>
        /// 删除病理节点
        /// </summary>
        /// <param name="caseName"></param>
        public static void DeleteCaseName(string caseName)
        {
            try
            {
                if (!File.Exists(filePath))  //如果不存在
                {
                    CreateXml();
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNodeList nodelist = doc.SelectSingleNode("CaseName").ChildNodes;
                foreach (XmlNode xnode in nodelist)
                {
                    if (xnode.Name == "Case" && xnode.Attributes["name"].Value.ToString() == caseName)
                    {
                        XmlElement RootElement = (XmlElement)doc.SelectSingleNode("CaseName");
                        RootElement.RemoveChild((XmlElement)xnode);
                        doc.Save(filePath);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DeleteCaseName Exception:  " + e.ToString());
                return;
            }
        }

        /// <summary>
        /// 添加excel文件中，每一列的名称
        /// </summary>
        /// <param name="caseName">病理名</param>
        /// <param name="columnName">列名</param>
        public static void AddColumnName(string caseName, string columnName,string type)
        {
            if (!File.Exists(filePath))  //如果不存在
            {
                CreateXml();
                throw new Exception("no xml");
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNodeList nodelist = doc.SelectSingleNode("CaseName").ChildNodes;
                foreach (XmlNode xnode in nodelist)
                {
                    if (xnode.Name == "Case" && xnode.Attributes["name"].Value.ToString() == caseName)
                    {
                        foreach (XmlElement e in ((XmlElement)xnode).ChildNodes)
                        {
                            if (e.GetAttribute("name") == columnName) return;
                        }
                        XmlElement colName = doc.CreateElement("ColumnName");
                        colName.SetAttribute("name", columnName);
                        colName.SetAttribute("type", type);
                        xnode.AppendChild(colName);
                        doc.Save(filePath);
                        return;
                    }
                }
                AddCaseName(caseName);
                AddColumnName(caseName,columnName,type);
            }
            catch (Exception e)
            {
                Console.WriteLine("AddColumnName Exception:  " + e.ToString());
                return;
            }
        }

        /// <summary>
        /// 修改列名
        /// </summary>
        /// <param name="caseName"></param>
        /// <param name="oldColumnName"></param>
        /// <param name="newColumnName"></param>
        /// <param name="oldType"></param>
        /// <param name="newType"></param>
        public static void ChangeColumnName(string caseName, string oldColumnName, string newColumnName, string oldType, string newType)
        {
            if (!File.Exists(filePath))  //如果不存在
            {
                CreateXml();
                throw new Exception("no xml");
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNodeList nodelist = doc.SelectSingleNode("CaseName").ChildNodes;
                foreach (XmlNode xnode in nodelist)
                {
                    if (xnode.Name == "Case" && xnode.Attributes["name"].Value.ToString() == caseName)
                    {
                        foreach (XmlElement e in ((XmlElement)xnode).ChildNodes)
                        {
                            if (e.GetAttribute("name") == oldColumnName)
                            {
                                e.SetAttribute("name", newColumnName);
                                e.SetAttribute("type", newType);
                                doc.Save(filePath);
                                return;
                            }    
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ChangeColumnName Exception:  " + e.ToString());
                return;
            }
        }

        /// <summary>
        /// 删除列名
        /// </summary>
        /// <param name="caseName"></param>
        /// <param name="columnName"></param>
        public static void DeleteColumnName(string caseName, string columnName)
        {
            if (!File.Exists(filePath))  //如果不存在
            {
                CreateXml();
                throw new Exception("no xml");
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNodeList nodelist = doc.SelectSingleNode("CaseName").ChildNodes;
                foreach (XmlNode xnode in nodelist)
                {
                    if (xnode.Name == "Case" && xnode.Attributes["name"].Value.ToString() == caseName)
                    {
                        foreach (XmlElement e in ((XmlElement)xnode).ChildNodes)
                        {
                            if (e.GetAttribute("name") == columnName)
                            {
                                xnode.RemoveChild(e);
                                doc.Save(filePath);
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DeleteColumnName Exception:  " + e.ToString());
                return;
            }
        }

    }
}
