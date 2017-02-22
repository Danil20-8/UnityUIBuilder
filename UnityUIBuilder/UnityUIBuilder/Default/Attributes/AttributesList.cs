using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Default.Attributes
{
    public class AttributesList<TAppData, TModelData, TElementData> : IAddAttributeHandler<TAppData, TModelData, TElementData>
    {
        IAddAttributeHandler<TAppData, TModelData, TElementData>[] handlers;

        public AttributesList(params IAddAttributeHandler<TAppData, TModelData, TElementData>[] handlers )
        {
            this.handlers = handlers;
        }

        public bool AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModelData, TElementData> element)
        {
            foreach (var h in handlers)
                if (h.AddAttribute(attributeName, attributeValue, element))
                    return true;
            return false;
        }
    }
}
