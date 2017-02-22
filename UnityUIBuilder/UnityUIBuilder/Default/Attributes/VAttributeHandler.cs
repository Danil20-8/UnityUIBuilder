using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace UnityUIBuilder.Default.Attributes
{
    public abstract class VAttributeHandler<TAppData, TModuleData, TElementData> : IAddAttributeHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
    {

        public delegate bool AddAttributeDelegate(string attributeName, string attributeValue, XMLElementUI<TAppData, TModuleData, TElementData> element);

        Dictionary<string, AddAttributeDelegate> versions = new Dictionary<string, AddAttributeDelegate>();
        AddAttributeDelegate defaultFunc;

        public VAttributeHandler()
        {
            foreach (var m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var attrs = m.GetCustomAttributes(typeof(VersionAttribute), false);
                if (attrs.Length > 0)
                {
                    VersionAttribute a = (VersionAttribute)attrs[0];

                    var f = Delegate.CreateDelegate(typeof(AddAttributeDelegate), this, m) as AddAttributeDelegate;

                    versions.Add(a.version, f);

                    if (a.useByDefault)
                        defaultFunc = f;
                }
            }
        }

        public bool AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModuleData, TElementData> element)
        {
            AddAttributeDelegate ae;

            if (versions.TryGetValue(element.module.data.GetVersion(), out ae))
                return ae.Invoke(attributeName, attributeValue, element);
            else if (defaultFunc != null)
                return defaultFunc.Invoke(attributeName, attributeValue, element);

            element.module.app.PushError("Please, pass module version to the begin of module");
            return false;
        }
    }
}
