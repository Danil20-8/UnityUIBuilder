using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using System.Reflection;
using UnityEngine;
using MyLib.Algoriphms;
namespace UnityUIBuilder.Standard.Handlers
{
    public class AddElementFromAssemblies<TAppData, TModelData, TElementData> : VElementHandler<TAppData, TModelData, TElementData>
        where TModelData : INamespaceData, IModuleVersionData
        where TElementData : ICreateChildData<TElementData>
    {
        [Version(Versions.std_v_1_0, true)]
        new public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModelData, TElementData>.External provider)
        {
            var namespaces = provider.data.GetNamespaces();

            var componentType = ComponentGetter.GetFromAssemblies(name, namespaces);
            if(componentType != null)
            {
                GameObject go = new GameObject(name);
                go.AddComponent(componentType);
                return provider.AddElement(name, previewData.CreateChild(go));
            }

            return null;
        }
    }
}
