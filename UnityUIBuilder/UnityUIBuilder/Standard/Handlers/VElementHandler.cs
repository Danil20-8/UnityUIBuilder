using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Standard.Handlers
{
    public abstract class VElementHandler<TAppData, TModuleData, TElementData> : IAddElementHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
    {
        public delegate IXMLElement AddElementDelegate(string name, TElementData previewData, XMLModule<TAppData, TModuleData, TElementData>.External provider);

        Dictionary<string, AddElementDelegate> versions = new Dictionary<string, AddElementDelegate>();
        AddElementDelegate defaultFunc;

        public VElementHandler()
        {
            foreach(var m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var attrs = m.GetCustomAttributes(typeof(VersionAttribute), false);
                if(attrs.Length > 0)
                {
                    VersionAttribute a = (VersionAttribute)attrs[0];

                    var f = Delegate.CreateDelegate(typeof(AddElementDelegate), this, m) as AddElementDelegate;

                    versions.Add(a.version, f);

                    if(a.useByDefault)
                        defaultFunc = f;
                }
            }
        }
        public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModuleData, TElementData>.External provider)
        {
            AddElementDelegate ae;

            if(versions.TryGetValue(provider.data.GetVersion(), out ae))
                return ae.Invoke(name, previewData, provider);
            else if(defaultFunc != null)
                return defaultFunc.Invoke(name, previewData, provider);

            provider.app.PushError("Please, pass module version to the begin of module");
            return new FakeElement(name);
        }
    }
}
