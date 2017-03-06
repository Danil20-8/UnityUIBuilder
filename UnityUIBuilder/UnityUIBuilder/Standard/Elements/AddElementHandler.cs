using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Standard.Elements
{
    public class AddElementHandlerList<TAppData, TModuleData, TElementData> : VElementHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData 
        where TElementData : ICreateChildData<TElementData>
    {
        IAddElementHandler<TAppData, TModuleData, TElementData>[] handlers;

        public AddElementHandlerList(params IAddElementHandler<TAppData, TModuleData, TElementData>[] handlers)
        {
            this.handlers = handlers;
        }

        [Version(typeof(std_1_0))]
        new public IXMLElement AddElement(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
        {
            foreach(var h in handlers)
            {
                var result = h.AddElement(name, previewElement);
                if (!(result is FakeElement)) return result;
            }

            return new BombElement(name, previewElement.name);
        }
    }
}
