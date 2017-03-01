using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;
using UnityUIBuilder.Standard.Attributes;

namespace UnityUIBuilder.Standard.States
{
    public class ComponentState<TAppData, TModuleData, TElementData> : State<TAppData, TModuleData, TElementData>
        where TModuleData: INamespaceData, IResFoldersData, IIDData
        where TElementData : IGameObjectData, IControllerData
    {
        TElementData previewData;
        Component component;

        public ComponentState(TElementData previewData, XMLModule<TAppData, TModuleData, TElementData>.Internal module)
            : base("component", module)
        {
            this.previewData = previewData;
        }

        public override void AddAttribute(string name, string value)
        {
            if(name == "name")
                component = ComponentGetter.GetFromAssemblies(value, module.data.GetNamespaces(), previewData.GetGameObject());
            else if(component != null)
                PropertySetter.SetValue(component, name, value, PropertySetter.Data.Create(previewData, module.data));
        }

        public override IXMLElement AddElement(string name)
        {
            return new FakeElement(name);
        }

        public override void SetValue(string value)
        {
        }
    }
}
