using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace MetalMynds.Utilities
{
    public class PropertiesHelper
    {

        public static void Set(String Filename, NameValueCollection Values)
        {
            XmlDocument propertiesDocument = new XmlDocument();
            Boolean newFile = false;
            //NameValueCollection existingValues = null;

            if (File.Exists(Filename)) {
                //existingValues = Get(Filename);
                FileInfo propertiiesInfo = new FileInfo(Filename);
                if (propertiiesInfo.Length > 0)
                {
                    newFile = false;
                }
                else
                {
                    newFile = true;
                }
            } else {
                //existingValues = new NameValueCollection();
                newFile = true;
            }

            if (newFile)
            {

                XmlNode rootNode = propertiesDocument.CreateElement("configuration");

                rootNode = propertiesDocument.AppendChild(rootNode);

                XmlNode propertiesNode = propertiesDocument.CreateElement("properties");

                rootNode.AppendChild(propertiesNode);

                foreach (String name in Values.Keys)
                {

                    XmlNode propertyNode = propertiesDocument.CreateElement("property");

                    propertyNode.Attributes.Append(propertiesDocument.CreateAttribute("name"));

                    propertyNode.Attributes["name"].InnerText = name;

                    propertyNode.InnerText = Values[name];

                    propertiesNode.AppendChild(propertyNode);

                }

                propertiesDocument.Save(Filename);

            }
            else
            {

                propertiesDocument.Load(Filename);

                XmlNode configurationNode = propertiesDocument.SelectSingleNode("//configuration");

                if (configurationNode != null)
                {

                    XmlNode propertiesNode = configurationNode.SelectSingleNode("./properties");

                    if (propertiesNode != null)
                    {

                        foreach (String key in Values.Keys)
                        {
                            XmlNode existingNode = propertiesNode.SelectSingleNode("./property[@name=\"" + key + "\"]");

                            if (existingNode != null)
                            {
                                existingNode.InnerText = Values[key];
                            }
                            else
                            {
                                XmlNode newPropertyNode = propertiesDocument.CreateElement("property");

                                newPropertyNode.Attributes.Append(propertiesDocument.CreateAttribute("name"));

                                newPropertyNode.Attributes["name"].InnerText = key;

                                newPropertyNode.InnerText = Values[key];

                                propertiesNode.AppendChild(newPropertyNode);

                            }
                        }

                        propertiesDocument.Save(Filename);

                    }
                }
                else
                {
                    throw new SetPropertiesFailedException(Filename, "Unable to Find Root Node [configuration]!");
                }

            }

        }

        public static NameValueCollection Get(String Filename)
        {

            NameValueCollection properties = new NameValueCollection();

            try
            {

                XmlDocument propertiesDocument = new XmlDocument();

                IOHelper.WaitForFile(Filename, 10);

                propertiesDocument.Load(Filename);

                XmlNode configurationNode = propertiesDocument.SelectSingleNode("//configuration");

                if (configurationNode != null)
                {
                    XmlNode propertiesNode  = configurationNode.SelectSingleNode("./properties");

                    if (propertiesNode != null)
                    {

                        XmlNodeList propertyNodeList = propertiesNode.SelectNodes("./property");

                        foreach (XmlNode propertyNode in propertyNodeList)
                        {
                            String name = propertyNode.Attributes["name"].InnerText;

                            String value = propertyNode.InnerText;

                            if (propertyNode.Attributes["value"] != null) // Short cut for hand created files, allows value to be set in property (Writer i.e. Set will not hounor this format)
                            {
                                value = propertyNode.Attributes["value"].InnerText;
                            }

                            properties.Add(name, value);

                        }
                    }
                    else
                    {
                        throw new GetPropertiesFailedException(Filename, "Unable to Find Child Node [properties]!");
                    }
                }
                else
                {
                    throw new GetPropertiesFailedException(Filename, "Unable to Find Root Node [configuration]!");
                }
            }
            catch (Exception ex)
            {
                throw new GetPropertiesFailedException(Filename, ex);
            }

            return properties;

        }

        public static bool IsProperties(String Filename)
        {
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(Filename);

            XmlNode propertiesNode = xmlDocument.SelectSingleNode("//configuration/properties");

            return (propertiesNode != null);

        }

    }

    public class PropertiesException : Exception
    {

        protected String BaseFilename;

        public PropertiesException(String Filename,String Message)
            : base(String.Format("PropertiesHelper: {0}", Message))
        {
        }
        
        public PropertiesException(String Filename, String Message,Exception InnerException)
            : base(String.Format("PropertiesHelper: {0}\nInnerException: {1}",Message,InnerException.Message))
        {
        }

    }

    public class SetPropertiesFailedException : PropertiesException 
    {

        public SetPropertiesFailedException(String Filename, Exception InnerException)
            : base(Filename, String.Format("Set Properties Failed!\nFilename: {0}", Filename), InnerException)
        {
        }

        public SetPropertiesFailedException(String Filename, String Message)
            : base (Filename,Message)
        {
        }

    }

    public class GetPropertiesFailedException : PropertiesException
    {

        public GetPropertiesFailedException(String Filename, Exception InnerException)
            : base(Filename, String.Format("Get Properties Failed!\nFilename: {0}",Filename), InnerException)
        {
        }

        public GetPropertiesFailedException(String Filename, String Message)
            : base (Filename,Message)
        {
        }

    }

}
