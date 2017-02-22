using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Default.States
{
    public class IncludeState<TAppData, TModelData> : State<TAppData, TModelData> where TModelData : IDataImport<TModelData>
    {
        public IncludeState(XMLModule<TAppData, TModelData>.Internal module) : this("include", module) { }
        public IncludeState(string name, XMLModule<TAppData, TModelData>.Internal module) : base(name, module) { }

        public override void AddAttribute(string name, string value)
        {
            switch (name)
            {
                case "xml":
                    IncludeXML(value);
                    break;
                case "css":
                    break;
            }
        }

        public override IXMLElement AddElement(string name)
        {
            module.app.PushError("include does not support nested elements");
            return null;
        }

        public override void SetValue(string value)
        {
            IncludeXML(value);
        }

        void IncludeXML(string name)
        {
            var m = module.app.Perform(name, module.transform);

            module.data.ImporData(m.data);
        }
    }
}
