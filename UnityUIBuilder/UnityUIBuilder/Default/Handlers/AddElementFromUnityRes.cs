using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;

namespace UnityUIBuilder.Default.Handlers
{
    /// <summary>
    /// Loads, instantiates and adds prefab as element
    /// </summary>
    /// <typeparam name="TModelData"></typeparam>
    public class AddElementFromUnityRes<TAppData, TModelData> : IAddElementHandler<TAppData, TModelData> where TModelData: IResFoldersData
    {
        public IXMLElement AddElement(string name, Transform parent, MonoBehaviour controller, XMLModule<TAppData, TModelData>.External provider)
        {
            UnityEngine.Object prefab = null;
            foreach (var folder in provider.data.GetResFolders())
            {
                prefab = Resources.Load(string.Join("/", new string[] { folder, name }));
                if (prefab != null)
                    return provider.AddElement(name,
                        GameObject.Instantiate((prefab as GameObject)),
                        parent,
                        controller
                        );
            }
            return null;
        }
    }
}
