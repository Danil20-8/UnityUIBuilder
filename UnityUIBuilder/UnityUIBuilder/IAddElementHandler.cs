using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;

namespace UnityUIBuilder
{
    public interface IAddElementHandler<TAppData, TModuleData, TElementData>
    {
        IXMLElement AddElement(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement);
    }
}
