using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using MyLib.Algoriphms;

namespace UnityUIBuilder.Standard.Attributes
{
    /// <summary>
    /// Sets built-in property types
    /// </summary>
    /// <typeparam name="TModuleData"></typeparam>
    public class SetPropertyAttribute<TAppData, TModuleData, TElementData> : VAttributeHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData, IResFoldersData, IIDData
        where TElementData : IGameObjectData, IControllerData
    {
        [Version(Versions.std_v_1_0, true)]
        new public bool AddAttribute(string attributeName, string value, XMLElementUI<TAppData, TModuleData, TElementData> element)
        {
            foreach (var go in element.data.GetGameObject().GetComponentsInChildren<Component>())
            {
                var p = go.GetType().GetProperty(attributeName, BindingFlags.Instance | BindingFlags.Public);
                if (p == null)
                    continue;


                if (PropertySetter.SetValue(p, go, value, PropertySetter.Data.Create(element.data, element.module.data))) return true;

            }
            return false;
        }
    }
}
