﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;
using UnityUIBuilder.Default.States;

namespace UnityUIBuilder.Default.Handlers
{
    public class AddElementState<TAppData, TModuleData, TElementData> : VElementHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
        where TElementData : IGameObjectData, IControllerData, ICloneData<TElementData>
    {
        [Version(Versions.std_v_1_0, true)]
        new public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModuleData, TElementData>.External provider)
        {
            switch(name)
            {
                case "controller":
                    return new ControllerState<TAppData, TModuleData, TElementData>(previewData, provider.GetInternal());
                default:
                    return null;
            }
        }
    }
}
