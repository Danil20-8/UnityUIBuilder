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
        }

        public override IXMLElement AddElement(string name)
        {
            return new FakeElement(name);
        }

        public override void SetValue(string value)
        {
            module.data.SetVersion(value);
        }
    }
}
