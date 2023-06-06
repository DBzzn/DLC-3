using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DLC_3.NET
{
    public class xml
    {
        public string dConfig = $"{AppDomain.CurrentDomain.BaseDirectory}";
        // private string[] cfgs = { "username", "192.168.15.133", "9773" }; array de CFG

        public void save(string[] cf)
        {
            string found = Array.Find(Directory.GetFiles(dConfig), arc => arc.EndsWith(".xml"));
            if (found != null)
            {
                XmlWriterSettings set = new XmlWriterSettings();
                set.Indent = true;
                set.IndentChars = (" ");
                set.CloseOutput = true;
                set.OmitXmlDeclaration = true;
                using (XmlWriter wrt = XmlWriter.Create($"{dConfig}\\fConfig.xml", set))
                {
                    wrt.WriteStartDocument();
                    wrt.WriteStartElement("root"); // inicio  de ROOT              
                    wrt.WriteStartElement("cfg"); // conteudo - lista;
                    wrt.WriteAttributeString("username", $"{cf[0]}");
                    wrt.WriteAttributeString("servidor", $"{cf[1]}");
                    wrt.WriteAttributeString("porta", $"{cf[2]}");
                    wrt.WriteEndElement();
                    wrt.WriteEndElement();
                    wrt.WriteEndDocument(); //finaliza o elemento ROOT                        
                    wrt.Close();
                    wrt.Flush();
                }

            }
            else
            {

                XmlDocument fConfig = new XmlDocument();
                fConfig.Load($"{dConfig}\\fConfig.xml");
                XmlNodeList els = fConfig.GetElementsByTagName("cfg");

                foreach (XmlNode el in els)
                {
                    foreach (XmlElementAttribute att in el)
                    {
                        if (att.ElementName.ToString() == "username")
                        {
                            Console.WriteLine($"USERNAME {att.ElementName.ToString()}");
                        }
                        else
                        {
                            Console.WriteLine(att.ElementName.ToString());
                            //continuar a escrever o load de XML
                        }
                    }
                }




            }


        }


    }
}
