using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CSharp;
using System.Reflection;
using System.CodeDom.Compiler;


namespace MetalMynds.Utilities
{
    public class Compiler
    {

        private Assembly BuildAssembly(String Code)
        {
            Microsoft.CSharp.CSharpCodeProvider provider = new CSharpCodeProvider();
            ICodeCompiler compiler = provider.CreateCompiler();
            CompilerParameters compilerparams = new CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;

            String assembliesPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            assembliesPath = assembliesPath.Replace("file:\\","");

            Listing listing = new FileListing();

            listing.Search(assembliesPath, "*.dll", true);

            foreach (String assembly in listing.Results)
            {

                compilerparams.ReferencedAssemblies.Add(assembly);

            }

            compilerparams.ReferencedAssemblies.Add("C:\\WINDOWS\\assembly\\GAC\\System.Windows.Forms\\1.0.5000.0__b77a5c561934e089\\System.Windows.Forms.dll");

            CompilerResults results =
               compiler.CompileAssemblyFromSource(compilerparams, Code);
            if (results.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder("Compiler Errors :\r\n");
                foreach (CompilerError error in results.Errors)
                {
                    errors.AppendFormat("Line {0},{1}\t: {2}\n",
                           error.Line, error.Column, error.ErrorText);
                }
                throw new Exception(errors.ToString());
            }
            else
            {
                return results.CompiledAssembly;
            }
        }

        public Boolean ExecuteCode(string Code, string NamespaceName, string ClassName, string FunctionName, bool IsStatic, out String Error,Dictionary<String,String> Properties, params object[] Args)
        {
            object returnval = null;
            Assembly asm = BuildAssembly(Code);
            object instance = null;
            Type type = null;
            if (IsStatic)
            {
                type = asm.GetType(NamespaceName + "." + ClassName);
            }
            else
            {
                instance = asm.CreateInstance(NamespaceName + "." + ClassName);
                type = instance.GetType();
            }

            PropertyInfo property = type.GetProperty("Properties");
            MethodInfo method = property.GetSetMethod();

            Dictionary<String, String> properties = new Dictionary<string,string>();

            foreach (String propertyName in Properties.Keys)
            {
                properties.Add(propertyName, Properties[propertyName]);
            }

            Object[] parameters = new Object[1];

            parameters[0] = properties;

            returnval = method.Invoke(instance, parameters);
            
            MethodInfo runMethod = type.GetMethod(FunctionName);
            returnval = runMethod.Invoke(instance, Args);

            FieldInfo errorField = type.GetField("Error");
            Error = (String)errorField.GetValue(instance);

            return (Boolean)returnval;
        }

    }
}
