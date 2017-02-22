﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Default.Attributes
{
    public class AttributesList<TAppData, TModelData> : IAddAttributeHandler<TAppData, TModelData>
    {
        IAddAttributeHandler<TAppData, TModelData>[] handlers;

        public AttributesList(params IAddAttributeHandler<TAppData, TModelData>[] handlers )
        {
            this.handlers = handlers;
        }

        public bool AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModelData> element)
        {
            foreach (var h in handlers)
                if (h.AddAttribute(attributeName, attributeValue, element))
                    return true;
            return false;
        }
    }
}