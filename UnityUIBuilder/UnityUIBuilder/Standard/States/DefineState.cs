using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRLib.Parsing.XML;

namespace UnityUIBuilder.Standard.States
{
    public class DefineState<TAppData, TModuleData, TElementData> : State<TAppData, TModuleData, TElementData> where TModuleData : IClassData
    {
        public DefineState(XMLElementUI<TAppData, TModuleData, TElementData> element) : base("define", element) { }

        public override void AddAttribute(string name, string value)
        {
        }

        public override IXMLElement AddElement(string name)
        {
            try
            {
                element.module.data.AddClass(name, new ClassAttribute[0]);
                return new DefineHelper(name, element);
            }
            catch
            {
                throw new AddElementException(name, this.name, "module already contains defenition for the class");
            }
        }

        public override void SetValue(string value)
        {
            element.module.app.Log("define element doesn't support values");
        }

        class DefineHelper : State<TAppData, TModuleData, TElementData>
        {
            public DefineHelper(string className, XMLElementUI<TAppData, TModuleData, TElementData> element) : base(className, element) { }

            public override void AddAttribute(string name, string value)
            {
                element.module.data.AddClassAttribute(this.name, new ClassAttribute { name = name, value = value });
            }

            public override IXMLElement AddElement(string name)
            {
                element.module.app.Log("class element cannot't contain nested elements.");
                return new FakeElement(name);
            }

            public override void SetValue(string value)
            {
                element.module.app.Log("class element doesn't support values");
            }
        }
    }
}
