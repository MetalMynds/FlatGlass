using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Xml.Linq;

namespace MetalMynds.Utilities
{
    public static class XmlHelper
    {
        public static Boolean IsWellFormedXml(String Xml)
        {
            try
            {
                XmlDocument document = new XmlDocument();

                document.LoadXml(Xml);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static String Escape(String Input)
        {
            return System.Security.SecurityElement.Escape(Input);
        }

        public static String GetFullyQualifiedXPath(XmlNode Node)
        {
            StringBuilder builder = new StringBuilder();
            while (Node != null)
            {
                switch (Node.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, "/@" + Node.Name);
                        Node = ((XmlAttribute)Node).OwnerElement;
                        break;
                    case XmlNodeType.Element:
                        int index = FindElementIndex((XmlElement)Node);
                        builder.Insert(0, "/" + Node.Name + "[" + index + "]");
                        Node = Node.ParentNode;
                        break;
                    case XmlNodeType.Document:
                        return builder.ToString();
                    default:
                        throw new ArgumentException("Only elements and attributes are supported");
                }
            }
            throw new ArgumentException("Node was not in a document");
        }

        private static int FindElementIndex(XmlElement element)
        {
            XmlNode parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return 1;
            }
            XmlElement parent = (XmlElement)parentNode;
            int index = 1;
            foreach (XmlNode candidate in parent.ChildNodes)
            {
                if (candidate is XmlElement && candidate.Name == element.Name)
                {
                    if (candidate == element)
                    {
                        return index;
                    }
                    index++;
                }
            }
            throw new ArgumentException("Couldn't find element within parent");
        }

        public static String StripBOM(String Xml)
        {
            int index = Xml.IndexOf('<');

            if (index > 0)
            {
                Xml = Xml.Substring(index, Xml.Length - index);
            }

            return Xml;
        }

        //public static String SanitiseXml(String Xml, Encoding Encoding)
        //{
        //    byte[] encodedString = Encoding.GetBytes(Xml);

        //    // Put the byte array into a stream and rewind it to the beginning
        //    MemoryStream ms = new MemoryStream(encodedString);
        //    ms.Flush();
        //    ms.Position = 0;

        //    return Encoding.GetString(encodedString);
        //}

        public static XmlDocument LoadXml(Encoding Encoding, String Xml, Boolean StripBOM)
        {
            // Build the XmlDocument from the MemorySteam of UTF-8 encoded bytes
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(LoadXml(Xml, Encoding, StripBOM));

            return xmlDoc;
        }

        public static MemoryStream LoadXml(String Xml, Encoding Encoding, Boolean StripBOM)
        {
            String xml;

            if (StripBOM)
            {
                xml = XmlHelper.StripBOM(Xml);
            }
            else
            {
                xml = Xml;
            }

            byte[] encodedString = Encoding.GetBytes(xml);

            // Put the byte array into a stream and rewind it to the beginning
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;

            return ms;
        }

        public static String PrettyPrint(String Xml, Encoding Encoding)
        {
            //String path = RelectionHelper.ApplicationDirectory; // Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            MemoryStream inputStream;

            inputStream = XmlHelper.LoadXml(Xml, Encoding, true);

            XmlTextReader inputReader = new XmlTextReader(inputStream);

            MemoryStream outputMemoryStream = new MemoryStream();

            XmlTextWriter outputWriter = new XmlTextWriter(outputMemoryStream, Encoding);

            XslCompiledTransform transform = new XslCompiledTransform();

            MemoryStream stylesheetMemoryStream;

            stylesheetMemoryStream = XmlHelper.LoadXml(MetalMynds.Utilities.Properties.Resources.PrettyPrint, Encoding, true); //XmlHelper.LoadXmlAsEncodedMemoryStream(MetalMynds.Utilities.Properties.Resources.PrettyPrint, Encoding);

            XmlTextReader xsltReader = new XmlTextReader(stylesheetMemoryStream);

            transform.Load(xsltReader);

            transform.Transform(inputReader, outputWriter);

            outputMemoryStream.Flush();

            outputMemoryStream.Seek(0, SeekOrigin.Begin);

            return Encoding.GetString(outputMemoryStream.ToArray());
        }

        public static XmlWriter CreateWriter(StringBuilder Builder, System.Text.Encoding Encoding, Boolean Indented)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = Indented;
            settings.Encoding = Encoding;
            return XmlWriter.Create(Builder, settings);
        }

        internal static List<String> _validationErrors = new List<String>();

        public static bool IsValidXml(String Xml, XmlSchemaSet Schemas, out List<String> Errors)
        {
            _validationErrors.Clear();

            XmlReaderSettings settings = new XmlReaderSettings();

            settings.Schemas.Add(Schemas);
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            settings.ValidationType = ValidationType.Schema;

            //Create the schema validating reader.
            using (XmlReader vreader = XmlReader.Create(new XmlTextReader(new StringReader(Xml)), settings))
            {
                while (vreader.Read()) { }

                //Close the reader.
                vreader.Close();
            }

            settings.ValidationEventHandler -= ValidationCallBack;

            Errors = new List<String>(_validationErrors);

            return Errors.Count == 0;
        }

        internal static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            _validationErrors.Add(String.Format("Validation {0} {1} {2}", args.Severity, args.Message, args.Exception));
        }

        public static XmlReader CreateReader(String Filename, Encoding Encoding)
        {
            String xml = File.ReadAllText(Filename, Encoding);

            return CreateReader(xml);
        }

        public static XmlReader CreateReader(String Xml)
        {
            StringReader stringReader = new StringReader(Xml);

            return new XmlTextReader(stringReader);
        }

        public static XmlWriter CreateWriter(Stream Stream, System.Text.Encoding Encoding, Boolean Indented)
        {
            return CreateWriter(Stream, Encoding, Indented, false);
        }

        public static XmlWriter CreateWriter(Stream Stream, System.Text.Encoding Encoding, Boolean Indented, Boolean SupressDeclaration)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = Indented;
            settings.Encoding = Encoding;
            settings.OmitXmlDeclaration = SupressDeclaration;
            return XmlWriter.Create(Stream, settings);
        }

        public static void WriteStyleSheetDeclaration(XmlWriter Writer, String StyleSheetName)
        {
            Writer.WriteProcessingInstruction("xml-stylesheet", String.Format("type='text/xsl' href='{0}'", StyleSheetName));
        }

        public static void WriteHtmlDeclaration(XmlWriter Writer, String PublicIdentifier = "-//W3C//DTD XHTML 1.0 Transitional//EN", String SystemIdentifier = "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd")
        {
            Writer.WriteDocType("html", PublicIdentifier, SystemIdentifier, null);            
        }

        public static void WriteStartDocument(XmlWriter Writer, String Name)
        {
            Writer.WriteStartDocument(true);

            WriteStartNode(Writer, Name);
        }

        public static void WriteStartDocument(XmlWriter Writer, String Name, String StyleSheet)
        {
            Writer.WriteStartDocument(true);

            WriteStyleSheetDeclaration(Writer, StyleSheet);

            WriteStartNode(Writer, Name);
        }

        public static void WriteRaw(XmlWriter Writer, String RawText)
        {
            Writer.WriteRaw(RawText);
        }

        public static void WriteStartDocument(XmlWriter Writer, String Root, NameValueCollection Attributes, String StyleSheet)
        {
            Writer.WriteStartDocument(true);

            WriteStyleSheetDeclaration(Writer, StyleSheet);

            Writer.WriteStartElement(Root);

            foreach (String attributeName in Attributes.Keys)
            {
                String value = Attributes[attributeName];

                if (value == null)
                {
                    value = String.Empty;
                }

                Writer.WriteAttributeString(attributeName, value);
            }

            //Writer.WriteEndElement();
        }

        public static void WriteStartDocument(XmlWriter Writer, String Root, NameValueCollection Attributes)
        {
            Writer.WriteStartDocument(true);

            Writer.WriteStartElement(Root);

            foreach (String attributeName in Attributes.Keys)
            {
                String value = Attributes[attributeName];

                if (value == null)
                {
                    value = String.Empty;
                }

                Writer.WriteAttributeString(attributeName, value);
            }

            //Writer.WriteEndElement();
        }

        public static void WriteEndDocument(XmlWriter Writer, String Root)
        {
            //WriteEndNode(Writer, Name);

            Writer.WriteEndDocument();
        }

        public static void WriteCompleteNode(XmlWriter Writer, String Name, String Value)
        {
            Writer.WriteElementString(Name, Value);
        }

        public static void WriteCompleteCDataNode(XmlWriter Writer, String Name, String Value)
        {
            Writer.WriteStartElement(Name);

            Writer.WriteCData(Value);

            Writer.WriteEndElement();
        }

        public static void WriteCompleteCDataNode(XmlWriter Writer, String Name, String Value, NameValueCollection Attributes)
        {
            Writer.WriteStartElement(Name);

            if (Attributes != null)
            {
                foreach (String attributeName in Attributes.Keys)
                {
                    String value = Attributes[attributeName];

                    if (value == null)
                    {
                        value = String.Empty;
                    }

                    Writer.WriteAttributeString(attributeName, value);
                }
            }

            Writer.WriteCData(Value);

            Writer.WriteEndElement();
        }

        public static void WriteCompleteNode(XmlWriter Writer, String Name, String Value, NameValueCollection Attributes)
        {
            Writer.WriteStartElement(Name);

            if (Attributes != null)
            {
                foreach (String attributeName in Attributes.Keys)
                {
                    String value = Attributes[attributeName];

                    if (value == null)
                    {
                        value = String.Empty;
                    }

                    Writer.WriteAttributeString(attributeName, value);
                }
            }

            //Writer.WriteString(">");

            Writer.WriteValue(Value);

            Writer.WriteEndElement();
        }

        public static void WriteCompleteNodeRaw(XmlWriter Writer, String Name, String Value, NameValueCollection Attributes)
        {
            Writer.WriteStartElement(Name);

            if (Attributes != null)
            {
                foreach (String attributeName in Attributes.Keys)
                {
                    String value = Attributes[attributeName];

                    if (value == null)
                    {
                        value = String.Empty;
                    }

                    Writer.WriteAttributeString(attributeName, value);
                }
            }

            //Writer.WriteString(">");

            Writer.WriteRaw(Value);

            Writer.WriteEndElement();
        }

        public static void WriteCompleteNode(XmlWriter Writer, String Name, NameValueCollection Attributes)
        {
            Writer.WriteStartElement(Name);

            if (Attributes != null)
            {
                foreach (String attributeName in Attributes.Keys)
                {
                    String value = Attributes[attributeName];

                    if (value == null)
                    {
                        value = String.Empty;
                    }

                    Writer.WriteAttributeString(attributeName, value);
                }
            }

            Writer.WriteEndElement();
        }

        public static void WriteStartNode(XmlWriter Writer, String Name)
        {
            Writer.WriteStartElement(Name);
        }

        public static void WriteStartNode(XmlWriter Writer, String Name, NameValueCollection Attributes)
        {
            Writer.WriteStartElement(Name);

            if (Attributes != null)
            {
                foreach (String attributeName in Attributes.Keys)
                {
                    String value = Attributes[attributeName];

                    if (value == null)
                    {
                        value = String.Empty;
                    }

                    Writer.WriteAttributeString(attributeName, value);
                }
            }
        }

        public static void WriteEndNode(XmlWriter Writer, String Name)
        {
            Writer.WriteEndElement();
        }

        public static XmlDocument Merge(XmlDocument DocumentA, XmlDocument DocumentB, List<String> IgnoreList)
        {
            XmlMerge.ClearIgnoreList();

            XmlMerge.AddXPathIgnoreRules(IgnoreList);

            return XmlMerge.MergeXml(DocumentA, DocumentB);
        }

        public static string GetPath(this XElement node)
        {
            string path = node.Name.ToString();
            XElement currentNode = node;
            while (currentNode.Parent != null)
            {
                currentNode = currentNode.Parent;
                path = currentNode.Name.ToString() + @"\" + path;
            }
            return path;
        }

    }
}