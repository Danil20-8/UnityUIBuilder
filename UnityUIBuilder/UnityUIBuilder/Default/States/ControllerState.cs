using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;

namespace UnityUIBuilder.Default.States
{
    public class ControllerState<TAppData, TModelData, TElementData> : State<TAppData, TModelData, TElementData>, IXMLElement
        where TElementData : IGameObjectData, IControllerData, ICloneData<TElementData>
    {
        TElementData data;

        public ControllerState(TElementData previewData, XMLModule<TAppData, TModelData, TElementData>.Internal module)
            :this("controller", previewData, module)
        {
        }

        public ControllerState(string name, TElementData previewData, XMLModule<TAppData, TModelData, TElementData>.Internal module)
            :base(name, module)
        {
            data = previewData.Clone();
        }

        public override void AddAttribute(string name, string value)
        {
            if(name=="name")
            {
                var controller = data.GetGameObject().GetComponentsInParent<MonoBehaviour>(true)
                    .FirstOrDefault(b => b.GetType().Name == value);
                if (controller == null)
                    module.app.PushError("controller: " + name + " is not found");
                else
                {
                    data.SetController(controller);
                }
            }
        }

        public override IXMLElement AddElement(string name)
        {
            if (data.GetController() == null) module.app.PushError("controller requiers name=\"controllerName\" attribute");
            return module.HandleElement(name, data);
        }

        public override void SetValue(string value)
        {
            module.app.PushError("controller element doesn't support a value");
        }
    }
}
