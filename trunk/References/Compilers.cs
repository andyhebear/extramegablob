using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
namespace ExtraMegaBlob.References
{
    public class ServerPluginCompiler
    {
        private CodeDomProvider GetCurrentProvider(string fileExtension)
        {
            CodeDomProvider provider;
            switch (fileExtension)
            {
                case ".cs":
                    provider = CodeDomProvider.CreateProvider("CSharp");
                    break;
                case ".vb":
                    provider = CodeDomProvider.CreateProvider("VisualBasic");
                    break;
                case ".js":
                    provider = CodeDomProvider.CreateProvider("JScript");
                    break;
                default:
                    provider = CodeDomProvider.CreateProvider("CSharp");
                    break;
            }
            return provider;
        }
        public ServerPlugin CompileCode(String sourceFile)
        {
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateInMemory = true;
            cp.TreatWarningsAsErrors = false;
            cp.GenerateExecutable = false;
            //cp.CompilerOptions = "/optimize";
            cp.IncludeDebugInformation = true;
            cp.ReferencedAssemblies.Add("System.dll");
            //cp.ReferencedAssemblies.Add("Mogre.dll");
            cp.ReferencedAssemblies.Add("ExtraMegaBlob.References.dll");
            //cp.ReferencedAssemblies.Add("MogreFramework.dll");
            //cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            //cp.ReferencedAssemblies.Add("MOIS_d.dll");
            //cp.ReferencedAssemblies.Add("MOIS.dll");

            //cp.OutputAssembly = Path.GetDirectoryName(sourceFile) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(sourceFile) + ".dll";
            string srcFilExt = System.IO.Path.GetExtension(sourceFile);
            string srcFilNam_minuspath = System.IO.Path.GetFileName(sourceFile);
            string prefix = "[" + srcFilNam_minuspath + "] ";
            CodeDomProvider provider = this.GetCurrentProvider(srcFilExt);
            DateTime dt_before = DateTime.Now;
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
            DateTime dt_after = DateTime.Now;
            TimeSpan ts_compiletime = new TimeSpan(dt_after.Ticks - dt_before.Ticks);
            log(prefix + "compiled in: " + ts_compiletime.ToString());
            log(prefix + "HasErrors: " + cr.Errors.HasErrors.ToString() + " HasWarnings: " + cr.Errors.HasWarnings.ToString());
            foreach (CompilerError ce in cr.Errors)
            {
                string g = ((ce.IsWarning) ? "[Warning]" : "[Error]");
                log(prefix + g + " Line: " + ce.Line + ", Number: " + ce.ErrorNumber + ", Text: " + ce.ErrorText);
            }
            if (cr.Errors.HasErrors) return null;
            Assembly ClientClass = cr.CompiledAssembly;
            Type ClientClassType = ClientClass.GetType("ExtraMegaBlob.plugin");
            if (!object.Equals(null, ClientClassType))
            {
                BindingFlags bflags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                ServerPlugin o;
                try
                {
                    o = (ServerPlugin)ClientClassType.InvokeMember("ExtraMegaBlob.plugin", bflags | BindingFlags.CreateInstance, null, null, null);
                }
                catch (Exception ex)
                {
                    log(prefix + ex.Message);
                    return null;
                }
                return o;
            }
            else return null;
        }
        public event LogDelegate onLog;
        private void log(string msg)
        {
            if (!object.Equals(null, this.onLog))
            {
                onLog(msg);
            }
        }
    }
    public class ClientPluginCompiler
    {
        private CodeDomProvider GetCurrentProvider(string fileExtension)
        {
            CodeDomProvider provider;
            switch (fileExtension)
            {
                case ".cs":
                    provider = CodeDomProvider.CreateProvider("CSharp");
                    break;
                case ".vb":
                    provider = CodeDomProvider.CreateProvider("VisualBasic");
                    break;
                case ".js":
                    provider = CodeDomProvider.CreateProvider("JScript");
                    break;
                default:
                    provider = CodeDomProvider.CreateProvider("CSharp");
                    break;
            }
            return provider;
        }
        public ClientPlugin CompileCode(String sourceFile)
        {
            CompilerParameters cp = new CompilerParameters();
            cp.CompilerOptions = "-unsafe";
            //cp.CompilerOptions.Insert(cp.CompilerOptions.Length, "-unsafe");

            //CompilerParameters.Unsafe
            cp.GenerateInMemory = true;
            cp.TreatWarningsAsErrors = false;
            cp.GenerateExecutable = false;
            //cp.CompilerOptions = "/optimize";
            cp.IncludeDebugInformation = true;
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("Mogre.dll");
            cp.ReferencedAssemblies.Add("ExtraMegaBlob.References.dll");
            cp.ReferencedAssemblies.Add("MogreFramework.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("System.Drawing.dll");
            //cp.ReferencedAssemblies.Add("MOIS_d.dll");
            cp.ReferencedAssemblies.Add("MOIS.dll");
            //cp.OutputAssembly = Path.GetDirectoryName(sourceFile) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(sourceFile) + ".dll";
            string srcFilExt = System.IO.Path.GetExtension(sourceFile);
            string srcFilNam_minuspath = System.IO.Path.GetFileName(sourceFile);
            CodeDomProvider provider = this.GetCurrentProvider(srcFilExt);
            DateTime dt_before = DateTime.Now;
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
            DateTime dt_after = DateTime.Now;
            TimeSpan ts_compiletime = new TimeSpan(dt_after.Ticks - dt_before.Ticks);
            log("compiled in: " + ts_compiletime.ToString() + ", File: " + srcFilNam_minuspath);
            foreach (CompilerError ce in cr.Errors)
            {
                string prefix = " [" + srcFilNam_minuspath + "] " + ((ce.IsWarning) ? "[Warning]" : "[Error]");
                log(prefix + " Line: " + ce.Line + ", Number: " + ce.ErrorNumber + ", Text: " + ce.ErrorText);
            }
            if (cr.Errors.HasErrors) return null;
            Assembly ClientClass = cr.CompiledAssembly;
            Type ClientClassType = ClientClass.GetType("ExtraMegaBlob.plugin");
            if (!object.Equals(null, ClientClassType))
            {
                BindingFlags bflags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                ClientPlugin o = (ClientPlugin)ClientClassType.InvokeMember("ExtraMegaBlob.plugin", bflags | BindingFlags.CreateInstance, null, null, null);
                return o;
            }
            else
            {
                log("Compiled Plugin failed to initialize properly, source: \"" + sourceFile + "\"");
                return null;
            }
        }
        public ClientPlugin CompileCode2(String sourceFile)
        {
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateInMemory = true;
            cp.TreatWarningsAsErrors = true;
            cp.GenerateExecutable = false;
            //cp.CompilerOptions = "/optimize";
            cp.IncludeDebugInformation = true;
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("Mogre.dll");
            cp.ReferencedAssemblies.Add("ExtraMegaBlob.References.dll");
            cp.ReferencedAssemblies.Add("MogreFramework.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("MOIS_d.dll");
            //cp.IncludeDebugInformation = false;
            string srcFilExt = System.IO.Path.GetExtension(sourceFile);
            string srcFilNam_minuspath = System.IO.Path.GetFileName(sourceFile);
            CodeDomProvider provider = this.GetCurrentProvider(srcFilExt);
            DateTime dt_before = DateTime.Now;
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
            DateTime dt_after = DateTime.Now;
            TimeSpan ts_compiletime = new TimeSpan(dt_after.Ticks - dt_before.Ticks);
            log("compiled in: " + ts_compiletime.ToString() + ", File: " + srcFilNam_minuspath);
            foreach (CompilerError ce in cr.Errors)
            {
                string prefix = (ce.IsWarning) ? "[Warning]" : "[Error]";
                log(prefix + " Line: " + ce.Line + ", Number: " + ce.ErrorNumber + ", Text: " + ce.ErrorText);
            }
            if (cr.Errors.HasErrors) return null;
            Assembly ClientClass = cr.CompiledAssembly;
            Type ClientClassType = ClientClass.GetType("thing.plugin");
            BindingFlags bflags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            ClientPlugin o = (ClientPlugin)ClientClassType.InvokeMember("thing.plugin", bflags | BindingFlags.CreateInstance, null, null, null);
            return o;
        }
        public event LogDelegate onLog;
        private void log(string msg)
        {
            if (!object.Equals(null, this.onLog))
            {
                onLog(msg);
            }
        }
    }
}



