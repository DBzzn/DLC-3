using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup.Localizer;
using System.Xml;
using System.Xml.Serialization;

namespace DLC_3.NET
{

    public class xmlS
    {
        public string dConfig = AppDomain.CurrentDomain.BaseDirectory;
        private string[] cfg = { "Username",
                                 "192.168.15.133", //cfg  padrao
                                 "9773" };
        public string xmlpath = $"{AppDomain.CurrentDomain.BaseDirectory}\\fConfig.xml";

        public bool chkA(string b)
        {
            if (string.IsNullOrEmpty(b.ToString()))
            {
                return false;

            }
            else if (string.IsNullOrWhiteSpace(b.ToString()))
            {
                return false;
                               
            }
            else
            {
                return true;

            }
        } // algo escrito = true

        public void save(string[] c) //cria o arquivo cfg ou excreve nele
        {
            xmlpath = Array.Find(Directory.GetFiles(dConfig), arc => arc.EndsWith(".xml"));
            if (chkA(xmlpath))
            {
                writeXML(c);
            } 
            else
            {
                sWrite(c);                
            }
        }

        internal void sWrite(string[] a)
        {
            string[] aConf;
            if (chkA(a.ToString()))
            {
                aConf = a;
            }
            else
            {
                aConf = cfg;
            }

            XmlWriterSettings set = new XmlWriterSettings();
            set.Indent = true;
            set.IndentChars = (" ");
            set.CloseOutput = true;
            set.OmitXmlDeclaration = true;
            using (XmlWriter wrt = XmlWriter.Create($"{dConfig}\\fConfig.xml", set))
            {
                wrt.WriteStartDocument();
                wrt.WriteStartElement("root"); // inicio de ROOT              
                wrt.WriteStartElement("cfg"); // conteudo - lista;
                wrt.WriteAttributeString("username", $"{aConf[0]}");
                wrt.WriteAttributeString("servidor", $"{aConf[1]}");
                wrt.WriteAttributeString("porta", $"{aConf[2]}");
                wrt.WriteEndElement();
                wrt.WriteEndElement();
                wrt.WriteEndDocument(); //finaliza o elemento ROOT                        
                wrt.Close();
                wrt.Flush();
            }
        } // cria arc e   p/a valido escreve a[] | p/a invalido escreve cfg[]



        public void writeXML(string[] cf) //escreve no arquivo
        {
            if (chkA(cf.ToString()))
            {
                this.cfg = cf;
            }

            XmlDocument fConfig = new XmlDocument();
            fConfig.Load(xmlpath);
            XmlNodeList els = fConfig.GetElementsByTagName("cfg");

            foreach (XmlNode el in els)
            {
                foreach (XmlAttribute att in el.Attributes.OfType<XmlAttribute>())
                {
                    var attName = att.Name.ToString();
                    switch (attName)
                    {
                        case "username":
                            Console.WriteLine($"USERNAME {att.Value}\n");
                            att.Value = this.cfg[0];
                            break;

                        case "servidor":
                            Console.WriteLine($"SERVIDOR {att.Value}\n");
                            att.Value = this.cfg[1];
                            break;

                        case "porta":
                            Console.WriteLine($"PORTA {att.Value}\n");
                            att.Value = this.cfg[2];
                            break;
                    }

                }
            }
            fConfig.Save(xmlpath);
        }

        public string[] readXML()
        {
            XmlDocument fConfig = new XmlDocument();
            string[] readCfg = new string[3];
            try
            {
                fConfig.Load(xmlpath);
                XmlNodeList els = fConfig.GetElementsByTagName("cfg");

                foreach (XmlNode el in els)
                {

                    foreach ( XmlAttribute att in el.Attributes.OfType<XmlAttribute>())
                    {
                        var attName = att.Name;
                        switch (attName)
                        {
                            case "username":
                                Console.WriteLine($"USERNAME {att.Value}\n");
                                readCfg[0] = att.Value;
                                break;

                            case "servidor":
                                Console.WriteLine($"SERVIDOR {att.Value}\n");
                                readCfg[1] = att.Value;
                                break;

                            case "porta":
                                Console.WriteLine($"PORTA {att.Value}\n");
                                readCfg[2] = att.Value;
                                break;
                        }

                    }
                }
                cfg = readCfg;
                return readCfg;
            }
            catch
            {
                save(cfg);
                cfg = readXML();// se deu erro = sem XML
                return cfg; 
            }         

        } // p/ xml valido, retorna e define o lido como padrao, se nao retorna o padrao

    }
}
