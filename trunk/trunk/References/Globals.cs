using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ExtraMegaBlob.References
{
    public class Globals
    {
        public Users Users = new Users();
        public Hashtable Data = new Hashtable();
        public static Globals Instance
        {
            get
            {
                return Nested.instance;
            }
        }
        public Globals()
        {
        }
        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly Globals instance = new Globals();
        }
    }
}


