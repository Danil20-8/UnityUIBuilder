using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRLib.Parsing.XML;
using UnityEngine;
using UnityUIBuilder.Standard.Attributes;

namespace UnityUIBuilder.Standard.States
{
    public class ComponentState<TAppData, TModuleData, TElementData> : State<TAppData, TModuleData, TElementData>
        where TModuleData: INamespaceData, IResFoldersData, IIDData
        where TElementData : IGameObjectData, IControllerData
    {
        Component component;

        public ComponentState(XMLElementUI<TAppData, TModuleData, TElementData> element)
            : base("component", element)
        {
        }

        public override void AddAttribute(string name, string value)
        {
            if (name == "name")
            {
                component = ComponentGetter.GetFromAssemblies(value, element.module.data.GetNamespaces(), element.data.GetGameObject());
                if (component == null)
                    throw new SetAttributeException(name, value, this.name, value + " is not found. Ensure you wrote it correct or added \"using namespace\" element to the xml module.");
            }
            else if (component != null)
                PropertySetter.SetValue(component, name, value, PropertySetter.Data.Create(element.data, element.module.data));
            else
                throw new SetAttributeException(name, value, this.name, "Set name attribute first.");
        }

        public override IXMLElement AddElement(string name)
        {
            element.module.app.Log("component element doesn't support nested elements.");
            return new FakeElement(name);
        }

        public override void SetValue(string value)
        {
            element.module.app.Log("component element doesn't support values.");
        }
    }
}
