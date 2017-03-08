using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace UnityUIBuilder.Standard.Attributes
{
    public abstract class VAttributeHandler<TAppData, TModuleData, TElementData> : IAddAttributeHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
    {

        public delegate AddResult AddAttributeDelegate(string attributeName, string attributeValue, XMLElementUI<TAppData, TModuleData, TElementData> element);

        public readonly string libName;
        List<Version> versions = new List<Version>();

        public VAttributeHandler(string libName = STD.lib_name)
        {
            this.libName = libName;
            foreach (var m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                foreach(var a in m.GetCustomAttributes(typeof(VersionAttribute), false).Cast<VersionAttribute>())
                {
                    var f = Delegate.CreateDelegate(typeof(AddAttributeDelegate), this, m) as AddAttributeDelegate;
                    versions.Add(new Version(a.name, a.iteration, f));
                }
            versions.Sort();
        }

        public AddResult AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModuleData, TElementData> element)
        {
            var m = GetMethod(element.module.data.GetVersion());
            if (m != null)
                return m(attributeName, attributeValue, element);

            throw new VersionException(this, element.module.data.GetVersion());
        }

        public AddAttributeDelegate GetMethod(string versionName)
        {
            var t = versions.Find(v => v.name == versionName);
            if (t == null)
            {
                try {
                    var i = VersionAttribute.GetIteration(libName, versionName);
                    foreach (var v in versions)
                        if (v.iteration <= i)
                            t = v;
                        else
                            break;
                }
                catch
                {
                    return null;
                }
            }
            return t != null ? t.addAttribute : null;
        }

        class Version : IComparable<Version>
        {
            public readonly string name;
            public readonly int iteration;
            public AddAttributeDelegate addAttribute;

            public Version(string name, int iteration, AddAttributeDelegate addAttribute)
            {
                this.name = name;
                this.iteration = iteration;
                this.addAttribute = addAttribute;
            }

            public int CompareTo(Version other)
            {
                return iteration < other.iteration ? -1 : (iteration > other.iteration ? 1 : 0);
            }
        }
    }
}
