using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Standard.States
{
    class UsingState<TAppData, TModelData, TElementData> : State<TAppData, TModelData, TElementData> where TModelData : INamespaceData, IResFoldersData
    {
        public UsingState(XMLElementUI<TAppData, TModelData, TElementData> element) : this("using", element) { }
        public UsingState(string name, XMLElementUI<TAppData, TModelData, TElementData> element) : base(name, element) { }

        public override void AddAttribute(string name, string value)
        {
            switch (name)
            {
                case "namespace":
                    element.module.data.AddNamespace(value);
                    break;
                case "folder":
                    element.module.data.AddResFolder(value);
                    break;
                default:
                    throw new SetAttributeException(name, value, this.name, "The attribute is not supported.");
            }
        }

        public override IXMLElement AddElement(string name)
        {
            element.module.app.Log("using element can't contain nested elements.");
            return new FakeElement(name);
        }

        public override void SetValue(string value)
        {
            element.module.app.Log("using element doesn't suppot values.");
        }
    }
}
