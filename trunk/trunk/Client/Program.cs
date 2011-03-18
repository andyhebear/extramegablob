using System;
using System.IO;

namespace ExtraMegaBlob.Client
{
    static class Program
    {
        private static String AssemblyCopyright
        {
            get
            {
                object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((System.Reflection.AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
        private static DateTime DateCompiled()
        {
            System.Version v = version;
            DateTime d = new DateTime(
                v.Build * TimeSpan.TicksPerDay +
                v.Revision * TimeSpan.TicksPerSecond * 2
                ).AddYears(1999).AddHours(1);
            return d.Subtract(new TimeSpan(24, 0, 0));
        }
        private static String title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        private static Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public static string header
        {
            get
            {
                return title + " v" + Program.version + " Compiled " + Program.DateCompiled().ToString();
            }
        }
        [STAThread]
        static void Main()
        {
            try
            {
                if (File.Exists("log2.txt")) File.Delete("log2.txt");
                Simulation g = new Simulation();
                g.main();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
