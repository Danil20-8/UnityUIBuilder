using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using System.Reflection;
using UnityEngine;
using MyLib.Algoriphms;
namespace UnityUIBuilder.Default.Handlers
{
    public class AddElementFromAssemlies<TAppData, TModelData> : IAddElementHandler<TAppData, TModelData> where TModelData : INamespaceData
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();


        public IXMLElement AddElement(string name, Transform parent, MonoBehaviour controller, XMLModule<TAppData, TModelData>.External provider)
        {
            var namespaces = provider.data.GetNamespaces();

            foreach(var a in assemblies)
            {
                foreach(var n in namespaces)
                {
                    foreach(var t in a.GetTypes())
                        if(t.Name == name && t.Namespace == n)
                            if(t.GetGeneration(typeof(Component)) != -1)
                            {
                                var go = new GameObject(name);
                                go.AddComponent(t);
                                return provider.AddElement(name, go, parent, controller);
                            }
                }
            }
            return null;
        }
    }
}
