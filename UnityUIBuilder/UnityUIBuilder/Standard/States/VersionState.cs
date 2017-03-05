using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Standard.States
{
    public class VersionState<TAppData, TModuleData, TElementData> : State<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
    {

        public VersionState(XMLModule<TAppData, TModuleData, TElementData>.Internal module)
            :base("version", module)
        {
        }

        public override void AddAttribute(string name, string value)
        {
            module.app.Log("version element doesn't support attributes. Use set value xml style for tell which version should be used.");
        }

        public override IXMLElement AddElement(string name)
        {
            throw new AddElementException(name, this.name, "version element doesn't supports nested elements. Use set value xml style for tell which version should be used.");
        }

        public override void SetValue(string value)
        {
            module.data.SetVersion(value);
        }
    }
}
