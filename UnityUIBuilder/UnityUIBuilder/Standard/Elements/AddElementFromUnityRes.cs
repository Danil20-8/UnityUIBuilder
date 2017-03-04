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
    /// <typeparam name="TModelData"></typeparam>
    public class AddElementFromUnityRes<TAppData, TModelData, TElementData> : VElementHandler<TAppData, TModelData, TElementData>
        where TModelData: IResFoldersData, IModuleVersionData
        where TElementData : ICreateChildData<TElementData>
    {
        [Version(typeof(std_1_0))]
        new public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModelData, TElementData>.External provider)
        {
            UnityEngine.Object prefab = null;
            foreach (var folder in provider.data.GetResFolders())
            {
                prefab = Resources.Load(string.Join("/", new string[] { folder, name }));
                if (prefab != null)
                    return provider.AddElement(name,
                        previewData.CreateChild(GameObject.Instantiate((prefab as GameObject)))
                        );
            }
            return null;
        }
    }
}
