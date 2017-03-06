using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;

namespace UnityUIBuilder.Standard.Elements
{
    /// <summary>
    /// Loads, instantiates and adds prefab as element
    /// </summary>
    /// <typeparam name="TModuleData"></typeparam>
    public class AddElementFromUnityRes<TAppData, TModuleData, TElementData> : VElementHandler<TAppData, TModuleData, TElementData>
        where TModuleData: IResFoldersData, IModuleVersionData
        where TElementData : ICreateChildData<TElementData>
    {
        [Version(typeof(std_1_0))]
        new public IXMLElement AddElement(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
        {
            UnityEngine.Object prefab = null;
            foreach (var folder in previewElement.module.data.GetResFolders())
            {
                prefab = Resources.Load(string.Join("/", new string[] { folder, name }));
                if (prefab != null)
                    return previewElement.CreateElement(name,
                        previewElement.data.CreateChild(GameObject.Instantiate((prefab as GameObject)))
                        );
            }

            return new BombElement(name, previewElement.name);
        }
    }
}
