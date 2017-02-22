using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Default.Handlers
{
    public class AddElementHandlerList<TAppData, TModelData> : IAddElementHandler<TAppData, TModelData>
    {
        IAddElementHandler<TAppData, TModelData>[] handlers;

        public AddElementHandlerList(params IAddElementHandler<TAppData, TModelData>[] handlers)
        {
            this.handlers = handlers;
        }

        public IXMLElement AddElement(string name, Transform parent, MonoBehaviour controller, XMLModule<TAppData, TModelData>.External provider)
        {
            foreach(var h in handlers)
            {
                var result = h.AddElement(name, parent, controller, provider);
                if (result != null) return result;
            }

            return provider.AddElement(name, new GameObject(name), parent, controller);
        }
    }
}
