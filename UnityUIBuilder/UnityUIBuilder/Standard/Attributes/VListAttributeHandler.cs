using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityUIBuilder.Standard.Attributes
{
    public class VListAttributeHandler<TAppData, TModuleData, TElementData> : IAddAttributeHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
    {

        Dictionary<string, VAttributeHandler<TAppData, TModuleData, TElementData>.AddAttributeDelegate[]> versions = new Dictionary<string, VAttributeHandler<TAppData, TModuleData, TElementData>.AddAttributeDelegate[]>();

        VAttributeHandler<TAppData, TModuleData, TElementData>[] handlers;

        public VListAttributeHandler(params VAttributeHandler<TAppData, TModuleData, TElementData>[] handlers)
        {
            this.handlers = handlers;
        }

        public bool AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModuleData, TElementData> element)
        {
            string version = element.module.data.GetVersion();
            VAttributeHandler<TAppData, TModuleData, TElementData>.AddAttributeDelegate[] hs;
            if (!versions.TryGetValue(version, out hs))
            {
                hs = handlers.Select(h => h.GetMethod(version)).Where(h => h != null).ToArray();
                if (hs.Length == 0)
                    throw new VersionException(this, version);
                versions.Add(version, hs);
            }
            foreach (var h in hs)
                if (h(attributeName, attributeValue, element)) return true;

            return false;
        }
    }
}
