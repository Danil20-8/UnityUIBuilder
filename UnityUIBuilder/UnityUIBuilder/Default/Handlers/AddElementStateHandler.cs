using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;
using UnityUIBuilder.Default.States;

namespace UnityUIBuilder.Default.Handlers
{
    public class AddElementState<TAppData, TModelData, TElementData> : IAddElementHandler<TAppData, TModelData, TElementData>
        where TElementData : IGameObjectData, IControllerData, ICloneData<TElementData>
    {
        public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModelData, TElementData>.External provider)
        {
            switch(name)
            {
                case "controller":
                    return new ControllerState<TAppData, TModelData, TElementData>(previewData, provider.GetInternal());
                default:
                    return null;
            }
        }
    }
}
