using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRLib.Parsing.XML;

namespace UnityUIBuilder.Standard.Elements
{
    public class AddElementCase<TAppData, TModuleData, TElementData> : IAddElementHandler<TAppData, TModuleData, TElementData>
    {
        IAddElementHandler<TAppData, TModuleData, TElementData> rootHandler;
        IAddElementHandler<TAppData, TModuleData, TElementData> elementHandler;

        public AddElementCase(IAddElementHandler<TAppData, TModuleData, TElementData> rootHandler, IAddElementHandler<TAppData, TModuleData, TElementData> elementHandler)
        {
            this.rootHandler = rootHandler;
            this.elementHandler = elementHandler;
        }

        public IXMLElement AddElement(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
        {
            if (previewElement.isRoot)
                return rootHandler.AddElement(name, previewElement);
            else
                return elementHandler.AddElement(name, previewElement);
        }
    }
}
