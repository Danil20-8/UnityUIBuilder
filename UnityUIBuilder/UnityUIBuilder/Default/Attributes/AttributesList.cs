using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Default.Attributes
{
    public class AttributesList<TAppData, TModuleData, TElementData> : VAttributeHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
    {
        IAddAttributeHandler<TAppData, TModuleData, TElementData>[] handlers;

        public AttributesList(params IAddAttributeHandler<TAppData, TModuleData, TElementData>[] handlers )
        {
            this.handlers = handlers;
        }

        [Version(Versions.std_v_1_0, true)]
        new public bool AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModuleData, TElementData> element)
        {
            foreach (var h in handlers)
                if (h.AddAttribute(attributeName, attributeValue, element))
                    return true;
            return false;
        }
    }
}
