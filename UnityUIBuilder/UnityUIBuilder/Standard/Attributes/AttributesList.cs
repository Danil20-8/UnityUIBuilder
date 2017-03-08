using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Standard.Attributes
{
    public class AttributesList<TAppData, TModuleData, TElementData> : VAttributeHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
    {
        IAddAttributeHandler<TAppData, TModuleData, TElementData>[] handlers;

        public AttributesList(params IAddAttributeHandler<TAppData, TModuleData, TElementData>[] handlers )
        {
            this.handlers = handlers;
        }

        [Version(typeof(std_1_0))]
        new public AddResult AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModuleData, TElementData> element)
        {
            AddResult r = AddResult.State.Ignored;
            foreach (var h in handlers)
            {
                r |= h.AddAttribute(attributeName, attributeValue, element);
                if (!r.ignored) return r;
            }
            return r;
        }
    }
}
