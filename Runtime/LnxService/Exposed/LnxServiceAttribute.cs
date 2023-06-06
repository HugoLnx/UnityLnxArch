using System;
using System.Collections;
using System.Linq;

namespace LnxArch
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class LnxServiceAttribute : Attribute
    {
        public bool Persistent { get; set; }
        public bool AutoAdd { get; set; } = true;
    }
}
