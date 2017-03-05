using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Standard.Elements
{
    public abstract class VElementHandler<TAppData, TModuleData, TElementData> : IAddElementHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
    {
        public delegate IXMLElement AddElementDelegate(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement);

        string libName;
        List<Version> versions = new List<Version>();

        public VElementHandler(string libName = STD.lib_name)
        {
            this.libName = libName;
            foreach (var m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                foreach (var a in m.GetCustomAttributes(typeof(VersionAttribute), false).Cast<VersionAttribute>())
                {
                    var f = Delegate.CreateDelegate(typeof(AddElementDelegate), this, m) as AddElementDelegate;
                    if (f != null)
                        versions.Add(new Version(a.name, a.iteration, f));
                    else
                        throw new Exception(m.Name + " is not valid.");
                }
            versions.Sort();
        }
        public IXMLElement AddElement(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
        {
            var m = GetMethod(previewElement.module.data.GetVersion());
            if (m != null)
                return m(name, previewElement);

            throw new VersionException(this, previewElement.module.data.GetVersion());
        }

        public AddElementDelegate GetMethod(string versionName)
        {
            var t = versions.Find(v => v.name == versionName);
            if (t == null)
            {
                try
                {
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
            public AddElementDelegate addAttribute;

            public Version(string name, int iteration, AddElementDelegate addAttribute)
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
