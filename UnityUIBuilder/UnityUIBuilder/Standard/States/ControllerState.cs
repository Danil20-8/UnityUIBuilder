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
        TElementData data;
        GameObject id = null;

        public ControllerState(TElementData previewData, XMLModule<TAppData, TModuleData, TElementData>.Internal module)
            :this("controller", previewData, module)
        {
        }

        public ControllerState(string name, TElementData previewData, XMLModule<TAppData, TModuleData, TElementData>.Internal module)
            :base(name, module)
        {
            data = previewData.Clone();
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
                        controller = data.GetGameObject().GetComponentsInParent<MonoBehaviour>(true)
                            .FirstOrDefault(b => b.GetType().Name == value);
                    if (controller == null)
                        module.app.PushError("controller: " + name + " is not found");
                    else
                    {
                        data.SetController(controller);
                    }
                    break;
                case "id":
                    id = module.data.GetObjectByID(value);
                    break;
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
