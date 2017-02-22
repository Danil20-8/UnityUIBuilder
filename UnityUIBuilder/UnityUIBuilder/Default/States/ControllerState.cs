using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;

namespace UnityUIBuilder.Default.States
{
    public class ControllerState<TAppData, TModelData> : State<TAppData, TModelData>, IXMLElement
    {
        Transform transform;
        MonoBehaviour controller;

        public ControllerState(Transform transform, XMLModule<TAppData, TModelData>.Internal module)
            :this("controller", transform, module)
        {
        }

        public ControllerState(string name, Transform transform, XMLModule<TAppData, TModelData>.Internal module)
            :base(name, module)
        {
            this.transform = transform;
        }

        public override void AddAttribute(string name, string value)
        {
            if(name=="name")
            {
                this.controller = transform.GetComponentsInParent<MonoBehaviour>(true)
                    .FirstOrDefault(b => b.GetType().Name == value);
                if (this.controller == null)
                    module.app.PushError("controller: " + name + " is not found");
            }
        }

        public override IXMLElement AddElement(string name)
        {
            if (controller == null) module.app.PushError("controller requiers name=\"controllerName\" attribute");
            return module.HandleElement(name, transform, controller);
        }

        public override void SetValue(string value)
        {
            module.app.PushError("controller element doesn't support a value");
        }
    }
}
