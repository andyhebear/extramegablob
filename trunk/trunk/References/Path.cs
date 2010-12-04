using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace ExtraMegaBlob.References
{
    public static class ThingPath
    {
        public static string path_cache
        {
            get
            {
                string path = path_base + Path.DirectorySeparatorChar + "cache";
                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch { }
                }
                return path;
            }
        }
        public static string path_servercache
        {
            get
            {
                string path = path_base + Path.DirectorySeparatorChar + "servercache";
                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch { }
                }
                return path;
            }
        }
        public static string path_base
        {
            get
            {
                System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
                return System.IO.Path.GetDirectoryName(a.Location);
            }
        }
        public static string path_config
        {
            get
            {
                return path_base + Path.DirectorySeparatorChar + "thingconfig.cfg";
            }
        }
        public static string path_log
        {
            get
            {
                return path_base + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_") + "thinglog.log";
            }
        }
        public static string path_debugfile
        {
            get
            {
                return path_base + Path.DirectorySeparatorChar + "thingdebuglog.log";
            }
        }
        public static string path_db
        {
            get
            {
                return path_base + Path.DirectorySeparatorChar + "thingstatedatabase.db3";
            }
        }
    }
}
