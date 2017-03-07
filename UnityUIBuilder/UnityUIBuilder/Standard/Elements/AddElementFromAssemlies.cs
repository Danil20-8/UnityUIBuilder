using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRLib.Parsing.XML;
using System.Reflection;
using UnityEngine;
using DRLib.Algoriphms;
namespace UnityUIBuilder.Standard.Elements
{
    public class AddElementFromAssemblies<TAppData, TModuleData, TElementData> : VElementHandler<TAppData, TModuleData, TElementData>
        where TModuleData : INamespaceData, IModuleVersionData
        where TElementData : ICreateChildData<TElementData>
    {
        [Version(typeof(std_1_0))]
        new public IXMLElement AddElement(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
        {
            var namespaces = previewElement.module.data.GetNamespaces();

            var componentType = ComponentGetter.GetFromAssemblies(name, namespaces);
            if(componentType != null)
            {
                GameObject go = new GameObject(name);
                go.AddComponent(componentType);
                return previewElement.CreateElement(name, previewElement.data.CreateChild(go));
            }

            return new BombElement(name, previewElement.name);
        }
    }
}
