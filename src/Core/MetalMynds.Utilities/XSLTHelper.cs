using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;

using Saxon.Api;

namespace MetalMynds.Utilities
{
    public class XSLTHelper
    {

        public static void Transform(String StylesheetFilename, String SourceFilename, String OutputFilename)
        {

            if (StylesheetFilename.StartsWith(".\\"))
            {
                StylesheetFilename = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + StylesheetFilename;
            }

            Processor processor = new Processor();

            XdmNode input = processor.NewDocumentBuilder().Build(new Uri(SourceFilename));

            processor.SetProperty("http://saxon.sf.net/feature/preferJaxpParser", "true");

            XsltCompiler compiler = processor.NewXsltCompiler();

            XsltExecutable executable = compiler.Compile(new Uri(StylesheetFilename));

            XsltTransformer transformer = executable.Load();

            transformer.InitialContextNode = input;

            Serializer serializer = new Serializer();

            System.IO.StreamWriter stream = new StreamWriter(OutputFilename);

            serializer.SetOutputWriter(stream);

            transformer.Run(serializer);

            stream.Close();

        }

        public static void Transform(Uri StyleSheet, String Input,Encoding Encoding,out String Output) {

            Processor processor = new Processor();

            XdmNode input = processor.NewDocumentBuilder().Build(new XmlTextReader(new StringReader(Input)));

            processor.SetProperty("http://saxon.sf.net/feature/preferJaxpParser", "true");

            XsltCompiler compiler = processor.NewXsltCompiler();

            XsltExecutable executable = compiler.Compile(StyleSheet);

            XsltTransformer transformer = executable.Load();

            transformer.InitialContextNode = input;

            Serializer serializer = new Serializer();

            MemoryStream stream = new MemoryStream();

            System.IO.StreamWriter writer = new StreamWriter(stream);

            serializer.SetOutputWriter(writer);

            transformer.Run(serializer);

            Output = Encoding.GetString(stream.ToArray());

            writer.Close();

            stream.Close();

        }

    }
}
