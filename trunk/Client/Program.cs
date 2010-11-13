using System;

namespace thing
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Simulation g = new Simulation();
            g.main();
        }
    }
}
