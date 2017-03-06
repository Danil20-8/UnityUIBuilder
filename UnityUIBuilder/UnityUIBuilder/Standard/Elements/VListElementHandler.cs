using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Standard.Elements
{
    public class VListElementHandler<TAppData, TModuleData, TElementData> : IAddElementHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
    {

        Dictionary<string, VElementHandler<TAppData, TModuleData, TElementData>.AddElementDelegate[]> versions = new Dictionary<string, VElementHandler<TAppData, TModuleData, TElementData>.AddElementDelegate[]> ();

        VElementHandler<TAppData, TModuleData, TElementData>[] handlers;

        public VListElementHandler(params VElementHandler<TAppData, TModuleData, TElementData>[] handlers)
        {
            this.handlers = handlers;
        }
        public IXMLElement AddElement(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
        {
            string version = previewElement.module.data.GetVersion();
            VElementHandler<TAppData, TModuleData, TElementData>.AddElementDelegate[] hs;
            if (!versions.TryGetValue(version, out hs))
            {
                hs = handlers.Select(h => h.GetMethod(version)).Where(h => h != null).ToArray();
                if (hs.Length == 0)
                    throw new VersionException(this, version);
                versions.Add(version, hs);
            }

            foreach (var h in hs)
            {
                var r = h(name, previewElement);
                if (!(r is FakeElement)) return r;
            }

            return new BombElement(name, previewElement.name);
        }
    }
}
