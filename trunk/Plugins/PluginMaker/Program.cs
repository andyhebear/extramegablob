using System;
using System.Collections;
using System.IO;
namespace PluginMaker
{
    class Program
    {
        static string options = string.Empty;
        static string bigLog = "";
        static void log(string s)
        {
            log(s, false);
        }
        static void logv(string s)
        {
            if (options.Contains("v"))
            {
                log(s, false);
            }
        }
        static void log(string s, bool ignoreOpt)
        {
            bigLog += s + Environment.NewLine;
            if (!options.Contains("s"))
                Console.WriteLine(s);
        }
        static void pressAnyKey()
        {
            if (options.Contains("w"))
            {
                log("Press any key to continue...");
                Console.ReadKey(true);
            }
        }
        static void errorQuit(int code)
        {
            log(@"Error: " + Constants.errors[code] + @"
Code: " + code.ToString(), true);
            pressAnyKey();
            Environment.Exit(code);
        }
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    tryrun(args[0]);
                    break;
                case 2:
                    options = args[0];
                    tryrun(args[1]);
                    break;
                default:
                    log(Constants.header, true);
                    log(Constants.help, true);
                    errorQuit(1);
                    break;
            }
            log(Constants.header);
            pressAnyKey();
        }
        private static void tryrun(string pluginName)
        {
            try
            {
                Run(pluginName);
            }
            catch (Exception ex)
            {
                log(ex.Message);
                errorQuit(2);
            }
        }
        public static void Run(string pluginName)
        {
            string path_outdir = Path.GetFullPath(".");
            string name = normalize_filename(pluginName);
            logv("pluginName:         " + pluginName);
            logv("OutputDirectory:  " + path_outdir);
            if (path_outdir.Length + (name.Length + 7) > 247)
                errorQuit(4);
            logv(Constants.hr);
            string file_cs = name + ".cs";
            string file_projfile = name + ".csproj";
            string path_cs = path_outdir + "\\" + file_cs;
            string path_projfile = path_outdir + "\\" + file_projfile;
            file_write(path_projfile, Constants.template_proj_cs
                .Replace("<{TOKEN1}>", pluginName)
                .Replace("<{TOKEN2}>", getNewGUID()));
            file_write(path_cs, Constants.template_code_cs
                .Replace("<{TOKEN3}>", pluginName));
        }
        private static string getNewGUID()
        {
            return "{" + System.Guid.NewGuid().ToString().ToUpper() + "}";
        }
        private static void file_write(string path, string data)
        {
            logv("Writing data to " + path);
            if (File.Exists(path))
            {
                if (options.Contains("o"))
                    File.Delete(path);
                else
                {
                    errorQuit(3);
                }
            }
            FileInfo t = new FileInfo(path);
            StreamWriter sw = t.CreateText();
            sw.Write(data);
            sw.Write(Environment.NewLine);
            sw.Close();
        }
        public const String validchars_filename = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789^&'@{}[],$=!-#()%.+~_ ";
        public static String normalize_filename(String String)
        {
            String RetVal = String.Empty;
            foreach (char c in String)
            {
                if (validchars_filename.IndexOf(c) > -1)
                    RetVal += c;
                else
                    RetVal += " ";
            }
            return RetVal;
        }
        private static void touch(string path_db, string path_outdir)
        {
            if (!Directory.Exists(path_outdir))
            {
                try
                {
                    Directory.CreateDirectory(path_outdir);
                    log("Created Directory: " + path_outdir);
                }
                catch
                {
                    errorQuit(6);
                }
            }
            if (!File.Exists(path_db))
            {
                try
                {
                    FileStream fs = File.Create(path_db);
                    fs.Close();
                    log("Created Database File: " + path_db);
                }
                catch
                {
                    errorQuit(6);
                }
            }
        }
    }
}
