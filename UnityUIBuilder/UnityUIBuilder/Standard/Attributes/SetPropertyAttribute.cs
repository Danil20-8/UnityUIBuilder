using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using DRLib.Algoriphms;

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
        [Version(typeof(std_1_0))]
        new public AddResult AddAttribute(string attributeName, string value, XMLElementUI<TAppData, TModuleData, TElementData> element)
        {
            foreach (var go in element.data.GetGameObject().GetComponentsInChildren<Component>())
            {
                var p = go.GetType().GetProperty(attributeName, BindingFlags.Instance | BindingFlags.Public);
                if (p == null)
                    continue;

                try {
                    if (PropertySetter.SetValue(p, go, value, PropertySetter.Data.Create(element.data, element.module.data)))
                        return AddResult.State.OK;
                }
                catch(Exception e)
                {
                    return new AddResult(AddResult.State.Error) { message = e.Message };
                }

            }
            return AddResult.State.Ignored;
        }
    }
}
