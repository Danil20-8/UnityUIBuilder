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

        public ComponentState(XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
            : base("component", previewElement.module)
        {
            this.previewData = previewElement.data;
        }

        public override void AddAttribute(string name, string value)
        {
            if (name == "name")
            {
                component = ComponentGetter.GetFromAssemblies(value, module.data.GetNamespaces(), previewData.GetGameObject());
                if (component == null)
                    throw new SetAttributeException(name, value, this.name, value + " is not found. Ensure you wrote it correct or added \"using namespace\" element to the xml module.");
            }
            else if (component != null)
                PropertySetter.SetValue(component, name, value, PropertySetter.Data.Create(previewData, module.data));
            else
                throw new SetAttributeException(name, value, this.name, "Set name attribute first.");
        }

        public override IXMLElement AddElement(string name)
        {
            module.app.Log("component element doesn't support nested elements.");
            return new FakeElement(name);
        }

        public override void SetValue(string value)
        {
            module.app.Log("component element doesn't support values.");
        }
    }
}
