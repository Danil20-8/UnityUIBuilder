using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder
{
    public interface IAddAttributeHandler<TAppData, TModuleData, TElementData>
    {
        bool AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModuleData, TElementData> element);
    }
}
