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
    public class AddElementFromAssemblies<TAppData, TModelData, TElementData> : VElementHandler<TAppData, TModelData, TElementData>
        where TModelData : INamespaceData, IModuleVersionData
        where TElementData : ICreateChildData<TElementData>
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        [Version(Versions.std_v_1_0, true)]
        new public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModelData, TElementData>.External provider)
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
                                return provider.AddElement(name, previewData.CreateChild(go), previewData);
                            }
                }
            }
            return null;
        }
    }
}
