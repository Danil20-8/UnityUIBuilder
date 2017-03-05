using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Standard.States
{
    public class DefineState<TAppData, TModelData, TElementData> : State<TAppData, TModelData, TElementData> where TModelData : IClassData
    {
        public DefineState(XMLModule<TAppData, TModelData, TElementData>.Internal module) : base("define", module) { }

        public override void AddAttribute(string name, string value)
        {
        }

        public override IXMLElement AddElement(string name)
        {
            try
            {
                module.data.AddClass(name, new ClassAttribute[0]);
            }
            catch
            {
                throw new AddElementException(name, this.name, "module already contains defenition for the class");
            }
            return new DefineHelper(name, module);
        }

        public override void SetValue(string value)
        {
            module.app.Log("define element doesn't support values");
        }

        class DefineHelper : State<TAppData, TModelData, TElementData>
        {
            public DefineHelper(string className, XMLModule<TAppData, TModelData, TElementData>.Internal module) : base(className, module) { }

            public override void AddAttribute(string name, string value)
            {
                module.data.AddClassAttribute(this.name, new ClassAttribute { name = name, value = value });
            }

            public override IXMLElement AddElement(string name)
            {
                module.app.Log("class element cannot't contain nested elements.");
                return new FakeElement(name);
            }

            public override void SetValue(string value)
            {
                module.app.Log("class element doesn't support values");
            }
        }
    }
}
