using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DRLib.Algoriphms;

namespace UnityUIBuilder.Standard
{
    public class VersionException : Exception
    {
        public VersionException(object errorObject, string versionName)
            :base(errorObject.GetType().Name + " doesn't supports the " + versionName + " version" )
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class VersionAttribute : Attribute
    {
        public readonly string name;
        public readonly string lib;
        public readonly string fullName;
        public readonly int iteration;

        public VersionAttribute(Type versionType)
            :this(
                (string)versionType.GetField("name").GetValue(null),
                (string)versionType.GetField("lib").GetValue(null),
                (int)versionType.GetField("iteration").GetValue(null)
        )
        {
        }

        public VersionAttribute(string name, string lib, int iteration)
        {
            this.name = name;
            this.lib = lib;
            fullName = lib + name;
            this.iteration = iteration;
            AddVersion(this);
        }

        void AddVersion(VersionAttribute va)
        {
            int iteration;
            if(versions.TryGetValue(va.fullName, out iteration))
            {
                if (iteration != va.iteration)
                    throw new Exception("Not equal versions. Use a same iteration for " + va.fullName + " version");
                return;
            }

            versions.Add(va.fullName, va.iteration);
        }

        static Dictionary<string, int> versions = new Dictionary<string, int>();

        public static int GetIteration(string fullName)
        {
            return versions[fullName];
        }

        public static int GetIteration(string lib, string name)
        {
            return versions[lib + name];
        }

        public static string GetLastVersionName(string lib)
        {
            return versions.Where(p => p.Key.IndexOf(lib) == 0).WithMax(p => p.Value).Key.Remove(0, lib.Length);
        }

    }
}
