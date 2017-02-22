using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;
using UnityUIBuilder.Default.States;

namespace UnityUIBuilder.Default.Handlers
{
    public class AddElementState<TAppData, TModelData> : IAddElementHandler<TAppData, TModelData>
    {
        public IXMLElement AddElement(string name, Transform parent, MonoBehaviour controller, XMLModule<TAppData, TModelData>.External provider)
        {
            switch(name)
            {
                case "controller":
                    return new ControllerState<TAppData, TModelData>(parent, provider.GetInternal());
                default:
                    return null;
            }
        }
    }
}
