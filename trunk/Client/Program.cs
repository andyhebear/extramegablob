using System;

namespace thing
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Simulation g = new Simulation();
                g.main();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
