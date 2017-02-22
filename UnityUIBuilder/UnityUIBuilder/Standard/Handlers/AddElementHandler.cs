using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Standard.Handlers
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

        [Version(Versions.std_v_1_0, true)]
        new public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModuleData, TElementData>.External provider)
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
