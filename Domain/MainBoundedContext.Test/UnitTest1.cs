// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.MainBoundedContext.Test/UnitTest1.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Dev.Comm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MainBoundedContext.Test
{
    [TestClass]
    public class UnitTest1
    {
        #region Instance Methods

        [TestMethod]
        public void MyTestMethod()
        {
            var c = new UnitTest1.c { I = 1, S = "mys" };
            var htmlAttributes = new { a = 1, b = 2, c = 3, style = "display:none;", c.I, c.S };
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(htmlAttributes))
            {
                Console.WriteLine(propertyDescriptor.Name + "==>" + propertyDescriptor.GetValue(htmlAttributes));
            }
        }


        //解密密码

        [TestMethod]
        public void MyTestMethodMethed()
        {
            Func(1, 2, "aaa");
        }

        [TestMethod]
        public void MyTestMethodReadXML()
        {
            var xml = @"<cas:serviceResponse xmlns:cas=""http://www.yale.edu/tp/cas"">
  <cas:authenticationSuccess>
    <cas:user>zbw911@qq.com</cas:user>
    <cas:ext>
      <cas:Uid>1</cas:Uid>
      <cas:Sex>1</cas:Sex>
      <cas:NickName>zbw911</cas:NickName>
      <cas:City>170300</cas:City>
      <cas:Province>170000</cas:Province>
      <cas:UserName>zbw911@qq.com</cas:UserName>
    </cas:ext>
  </cas:authenticationSuccess>
</cas:serviceResponse>
";


            var xmlh = new XmlHelper();
            xmlh.LoadXML(xml, XmlHelper.LoadType.FromString);

            if (xmlh.RootNode.FirstChild.LocalName == "authenticationSuccess")
            {
                var value = xmlh.GetChildElementValue(xmlh.RootNode.FirstChild, "cas:user");
                Console.WriteLine(value);

                var exts = xmlh.GetFirstChildXmlNode(xmlh.RootNode.FirstChild, "cas:ext");


                var dic = new Dictionary<string, string>();


                for (var i = 0; i < exts.ChildNodes.Count; i++)
                {
                    var ext = exts.ChildNodes.Item(i);

                    dic.Add(ext.LocalName, ext.InnerText);
                }
            }
        }

        [TestMethod]
        public void MyTestMethodXML()
        {
            var writer = new StringWriter();
            var xmlwriter = new XmlTextWriter(writer);

            xmlwriter.WriteStartElement("cas:serviceResponse");
            xmlwriter.WriteAttributeString("xmlns:cas", "http://www.yale.edu/tp/cas");

            xmlwriter.WriteStartElement("cas:authenticationSuccess");
            xmlwriter.WriteStartElement("cas:user");
            xmlwriter.WriteString("zbw911");
            xmlwriter.WriteEndElement();

            var extobj = new { a = 1, b = 2, c = 3 };

            xmlwriter.WriteStartElement("cas:ext");

            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(extobj))
            {
                //Console.WriteLine(propertyDescriptor.Name + "==>" + propertyDescriptor.GetValue(extobj));

                xmlwriter.WriteStartElement("cas:" + propertyDescriptor.Name);
                var value = propertyDescriptor.GetValue(extobj);
                xmlwriter.WriteString(value == null ? "" : value.ToString());
                xmlwriter.WriteEndElement();
            }

            xmlwriter.WriteEndElement();


            xmlwriter.WriteEndElement();

            xmlwriter.Close();
            var str = writer.ToString();

            Console.WriteLine(str);
        }

        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine(
                "6680E8CE1DCECB2D6D69C04C6E1D4874291884CF293E59F19E810F4B5F885400206E0E382A97055D4EF4ACB630B069CFD52F49D9D7125976");
        }

        #endregion

        #region Class Methods



        private static void Func(int i, decimal d, string s)
        {
            //我想知道:
            //1.Func的被调用参数的数量
            //这个没看懂，被调用？参数的数量不就是3个么？
            var frame = new StackFrame(0);
            var m = frame.GetMethod(); //当前方法，反射获得

            var parameters = m.GetParameters(); //反射参数列表
            Console.WriteLine(parameters.Length); //3
            //2.Func此时的参数类型
            foreach (var p in parameters)
            {
                Console.WriteLine(p.ParameterType.Name + " " + p.Name + "=>" + p); //输出3次
                //Int32 i
                //Decimal d
                //String s
            }
            //3.Func此时的某个参数的具体值
            //第三个不知道如何实现
        }

        #endregion

        #region Nested type: c

        private class c
        {
            #region Instance Properties

            public int I { get; set; }

            public string S { get; set; }

            #endregion
        }

        #endregion



        [TestMethod]
        public void Add100()
        {
            var date = System.DateTime.Parse("2013-3-22").AddDays(100);
            Console.WriteLine(date);
        }









    }
}