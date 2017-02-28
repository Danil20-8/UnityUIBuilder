using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using MyLib.Algoriphms;

namespace UnityUIBuilder.Standard
{
    public static class ComponentGetter
    {
        public static Type GetFromAssemblies(string name, IEnumerable<string> namespaces)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var a in assemblies)
            {
                foreach (var n in namespaces)
                {
                    foreach (var t in a.GetTypes())
                        if (t.Name == name && t.Namespace == n)
                            if (t.GetGeneration(typeof(Component)) != -1)
                                return t;
                }
            }
            return null;
        }
        public static Component GetFromAssemblies(string name, IEnumerable<string> namespaces, GameObject gameObject)
        {
            var type = GetFromAssemblies(name, namespaces);
            if (type != null)
                return gameObject.AddComponent(type);
            else
                return null;
        }
        
    }
}
