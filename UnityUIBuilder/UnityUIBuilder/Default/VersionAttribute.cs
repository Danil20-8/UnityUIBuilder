using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityUIBuilder.Default
{
    public class VersionAttribute : Attribute
    {
        public readonly string version;
        public readonly bool useByDefault;
        public VersionAttribute(string version, bool useByDefault)
        {
            this.version = version;
            this.useByDefault = useByDefault;
        }
    }
}
