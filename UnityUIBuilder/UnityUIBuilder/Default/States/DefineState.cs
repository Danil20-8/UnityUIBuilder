using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Default.States
{
    public class DefineState<TAppData, TModelData> : State<TAppData, TModelData> where TModelData : IClassData
    {
        public DefineState(XMLModule<TAppData, TModelData>.Internal module) : base("define", module) { }

        public override void AddAttribute(string name, string value)
        {
        }

        public override IXMLElement AddElement(string name)
        {
            module.data.AddClass(name, new ClassAttribute[0]);
            return new DefineHelper(name, module);
        }

        public override void SetValue(string value)
        {
        }

        class DefineHelper : State<TAppData, TModelData>
        {
            public DefineHelper(string className, XMLModule<TAppData, TModelData>.Internal module) : base(className, module) { }

            public override void AddAttribute(string name, string value)
            {
                module.data.AddClassAttribute(this.name, new ClassAttribute { name = name, value = value });
            }

            public override IXMLElement AddElement(string name)
            {
                module.app.PushError("define: " + this.name + ": Class definition can't contain nested elements");
                return new FakeElement(name);
            }

            public override void SetValue(string value)
            {
            }
        }
    }
}
