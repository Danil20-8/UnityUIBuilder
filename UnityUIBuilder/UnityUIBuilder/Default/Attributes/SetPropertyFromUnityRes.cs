using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using MyLib.Algoriphms;

namespace UnityUIBuilder.Default.Attributes
{
    /// <summary>
    /// Loads prefabs and sets to property
    /// </summary>
    /// <typeparam name="TModelData"></typeparam>
    public class SetPropertyFromUnityRes<TAppData, TModelData, TElementData> : IAddAttributeHandler<TAppData, TModelData, TElementData>
        where TModelData : IResFoldersData
        where TElementData : IGameObjectData
    {
        public bool AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModelData, TElementData> element)
        {
            foreach (var com in element.data.GetGameObject().GetComponentsInChildren<Component>())
            {
                var p = com.GetType().GetProperty(attributeName, BindingFlags.Instance | BindingFlags.Public);
                if (p == null)
                    continue;
                if (p.PropertyType.GetGeneration(typeof(UnityEngine.Object)) != -1)
                {
                    UnityEngine.Object res = Resources.Load(attributeValue, p.PropertyType);
                    if (res == null)
                    {
                        string[] mj = new string[2];
                        mj[1] = attributeValue;
                        foreach (var f in element.module.data.GetResFolders())
                        {
                            mj[0] = f;
                            res = Resources.Load(string.Join("/", mj), p.PropertyType);
                            if (res != null)
                                break;
                        }
                    }
                    if (res != null)
                        p.SetValue(com, res, null);
                    return true;
                }
            }
            return false;
        }
    }
}
