using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRLib.Parsing.XML;

namespace UnityUIBuilder.Standard.States
{
    public class IncludeState<TAppData, TModelData, TElementData> : State<TAppData, TModelData, TElementData>
        where TModelData : IDataImport<TModelData>
        where TElementData : ITransformData
    {
        public IncludeState(XMLElementUI<TAppData, TModelData, TElementData> element) : this("include", element) { }
        public IncludeState(string name, XMLElementUI<TAppData, TModelData, TElementData> element) : base(name, element) { }

        public override void AddAttribute(string name, string value)
        {
            switch (name)
            {
                case "xml":
                    IncludeXML(value);
                    break;
                default:
                    throw new SetAttributeException(name, value, this.name, "The attribute is not supported.");
            }
        }

        public override IXMLElement AddElement(string name)
        {
            element.module.app.Log("include does not support nested elements.");
            return new FakeElement(name);
        }

        public override void SetValue(string value)
        {
            IncludeXML(value);
        }

        void IncludeXML(string name)
        {
            try
            {
                var m = element.module.app.Perform(name, element.module.rootElement.data);
                element.module.data.ImportData(m.data);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
