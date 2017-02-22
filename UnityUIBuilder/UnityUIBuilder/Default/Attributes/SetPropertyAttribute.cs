using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using MyLib.Algoriphms;

namespace UnityUIBuilder.Default.Attributes
{
    /// <summary>
    /// Sets built-in property types
    /// </summary>
    /// <typeparam name="TModelData"></typeparam>
    public class SetPropertyAttribute<TAppData, TModelData, TElementData> : IAddAttributeHandler<TAppData, TModelData, TElementData>
        where TElementData : IGameObjectData, IControllerData
    {
        public bool AddAttribute(string attributeName, string value, XMLElementUI<TAppData, TModelData, TElementData> element)
        {
            foreach (var go in element.data.GetGameObject().GetComponentsInChildren<Component>())
            {
                var p = go.GetType().GetProperty(attributeName, BindingFlags.Instance | BindingFlags.Public);
                if (p == null)
                    continue;

                try
                {
                    if (ValueSetter.SetValue(p, go, value, element.data)) return true;
                }
                catch (Exception e)
                {
                    element.module.app.PushError(e.Message);
                    return true;
                }
            }
            return false;
        }
    }
}
