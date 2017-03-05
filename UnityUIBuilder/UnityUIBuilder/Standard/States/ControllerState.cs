using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;

namespace UnityUIBuilder.Standard.States
{
    public class ControllerState<TAppData, TModuleData, TElementData> : State<TAppData, TModuleData, TElementData>, IXMLElement
        where TElementData : IGameObjectData, IControllerData, ICloneData<TElementData>
        where TModuleData : IIDData
    {
        XMLElementUI<TAppData, TModuleData, TElementData> element;
        GameObject id = null;

        public ControllerState(XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
            :this("controller", previewElement)
        {
        }

        public ControllerState(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
            :base(name, previewElement.module)
        {
            // creating new element with clone preview element data to keep preview controller unaltered
            element = new XMLElementUI<TAppData, TModuleData, TElementData>(previewElement.name, previewElement.data.Clone(), module);
        }

        public override void AddAttribute(string name, string value)
        {
            switch(name)
            {
                case "name":
                    MonoBehaviour controller = null;
                    if (id != null)
                        controller = id.GetComponent<MonoBehaviour>();
                    else
                        controller = element.data.GetGameObject().GetComponentsInParent<MonoBehaviour>(true)
                            .FirstOrDefault(b => b.GetType().Name == value);
                    if (controller == null)
                        throw new SetAttributeException(name, value, this.name, "controller's not found. Ensure you wrote it correct, added a gameObject with this component with id or one of outer gameObject has this component.");
                    else
                    {
                        element.data.SetController(controller);
                    }
                    break;
                case "id":
                    try {
                        id = module.data.GetObjectByID(value);
                    }
                    catch
                    {
                        throw new SetAttributeException(name, value, this.name, "GameObject with id is not found. Ensure you wrote correct id or one of upper gameObjects has the id.");
                    }
                    break;
            }
        }

        public override IXMLElement AddElement(string name)
        {
            if (element.data.GetController() == null) throw new AddElementException(name, this.name, "Please, add aatribute name to the controller before you will able add child elements.");
            return module.HandleElement(name, element);
        }

        public override void SetValue(string value)
        {
            module.app.Log("controller element doesn't support a value");
        }
    }
}
