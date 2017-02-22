using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Default.Handlers
{
    public class AddElementHandlerList<TAppData, TModelData, TElementData> : IAddElementHandler<TAppData, TModelData, TElementData>
        where TElementData : ICreateChildData<TElementData>
    {
        IAddElementHandler<TAppData, TModelData, TElementData>[] handlers;

        public AddElementHandlerList(params IAddElementHandler<TAppData, TModelData, TElementData>[] handlers)
        {
            this.handlers = handlers;
        }

        public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModelData, TElementData>.External provider)
        {
            foreach(var h in handlers)
            {
                var result = h.AddElement(name, previewData, provider);
                if (result != null) return result;
            }

            return provider.AddElement(name, previewData.CreateChild(name), previewData);
        }
    }
}
