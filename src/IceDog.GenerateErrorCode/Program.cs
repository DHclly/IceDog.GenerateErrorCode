using IceDog.GenerateErrorCode.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace IceDog.GenerateErrorCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //因为是从xx.xml文件里面读取，所以要确保要读取的类所在项目
            //的属性-生成-xml文档文件勾选上了并添加依赖到此项目，重新生成当前项目
            var xml = GetXmlDoc(Environment.CurrentDirectory + @"\IceDog.GenerateErrorCode.xml");
            var noteDict = GetNoteDict(xml);
            var modelList = GetHttpStatusCodeModelList(noteDict);
            GenerateMarkDownDoc(modelList,Environment.CurrentDirectory+@"\http-status-code.md");
        }
        /// <summary>
        /// 获取xmldoc
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static XmlDocument GetXmlDoc(string path)
        {
            
            XmlDocument xml = new XmlDocument();
            //加载xml文件
            xml.Load(path);
            return xml;
        }
        /// <summary>
        /// 获取注释的字典集合
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        static IDictionary<string, string> GetNoteDict(XmlDocument xml)
        {
            //用于存储解析出来的xml注释
            var dictNote = new Dictionary<string, string>();
            //可以查看xml文档格式然后可以通过如下XPath表达式获取相关节点内容
            var memebers = xml.SelectNodes("/doc/members/member");
            foreach (object m in memebers)
            {
                //判断是否转换类型成功
                if (m is XmlNode node)
                {
                    //获取member节点的属性-名称
                    XmlAttribute propName = node.Attributes["name"];
                    string propNameValue = propName.Value;
                    //里面还有一层summary节点，因为我们解析的是常量节点，
                    //不会包含其他节点，所以不用进一步读取子节点
                    var value = node.InnerText.Trim();
                    //用于匹配的key
                    var matchKey = "F:IceDog.GenerateErrorCode.Codes.HttpStatusCode";
                    //通过name值进行解析，目前发现的前缀有 F:field,M:method,T;type,P:property
                    if (propNameValue.IndexOf(matchKey, StringComparison.Ordinal) > -1)
                    {
                        //去掉前缀和冒号，然后赋值
                        //如：IceDog.GenerateErrorCode.Codes.HttpStatusCode.Ok
                        dictNote[propNameValue.Substring(2)] = value;
                    }
                }
            }
            return dictNote;
        }
        /// <summary>
        /// 获取解析成功后的模型列表
        /// </summary>
        /// <param name="noteDict"></param>
        /// <returns></returns>
        static IList<HttpStatusCodeModel> GetHttpStatusCodeModelList(IDictionary<string, string> noteDict)
        {
            //解析常量对象

            //存储解析的内容
            var modelList = new List<HttpStatusCodeModel>();
            var constants = new ArrayList();
            Type type = typeof(Codes.HttpStatusCode);

            //从规定的约束内搜索字段
            //约束有是静态成员，是公共成员，和返回父级的公共静态成员，
            FieldInfo[] infoList = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            foreach (FieldInfo info in infoList)
            {
                //按照要解析的字段的特性来判断,
                //常量是字面量，不可以在构造函数中初始化
                if (info.IsLiteral && !info.IsInitOnly)
                {
                    constants.Add(info);
                }
            }
            //常量信息列表
            var constantInfoList = (FieldInfo[])constants.ToArray(typeof(FieldInfo));
            foreach (FieldInfo info in constantInfoList)
            {
                var hscm = new HttpStatusCodeModel
                {
                    Value = (int)info.GetRawConstantValue(),
                    Name = info.Name,
                    FullName = info.DeclaringType.FullName + "." + info.Name
                };
                hscm.Note = noteDict[hscm.FullName];
                modelList.Add(hscm);
            }
            //通过值进行升序排序
            modelList.Sort((m1, m2) => m1.Value - m2.Value);
            return modelList;
        }
        /// <summary>
        /// 生成MarkDown文档
        /// </summary>
        /// <param name="modelList"></param>
        private static void GenerateMarkDownDoc(IList<HttpStatusCodeModel> modelList,string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Http Status Code Description Doc");
            sb.AppendLine();
            sb.AppendLine("|FullName|Name|Value|Note|");
            sb.AppendLine("|-|-|-|-|");
            foreach (var m in modelList)
            {
                sb.AppendLine($"|{m.FullName}|{m.Name}|{m.Value}|{m.Note}|");
            }
            var sw = File.CreateText(path);
            sw.WriteAsync(sb.ToString()).Wait();
            sw.FlushAsync().Wait();
            sw.Close();
            Console.WriteLine("generate success !!!");
        }
    }
}
