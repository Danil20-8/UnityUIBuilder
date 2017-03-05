using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Standard.States
{
    class UsingState<TAppData, TModelData, TElementData> : State<TAppData, TModelData, TElementData> where TModelData : INamespaceData, IResFoldersData
    {
        public UsingState(XMLModule<TAppData, TModelData, TElementData>.Internal module) : this("using", module) { }
        public UsingState(string name, XMLModule<TAppData, TModelData, TElementData>.Internal module) : base(name, module) { }

        public override void AddAttribute(string name, string value)
        {
            switch (name)
            {
                case "namespace":
                    module.data.AddNamespace(value);
                    break;
                case "folder":
                    module.data.AddResFolder(value);
                    break;
                default:
                    break;
            }
        }

        public override IXMLElement AddElement(string name)
        {
            module.app.Log("using element can't contain nested elements.");
            return new FakeElement(name);
        }

        public override void SetValue(string value)
        {
            module.app.Log("using element doesn't suppot values.");
        }
    }
}
