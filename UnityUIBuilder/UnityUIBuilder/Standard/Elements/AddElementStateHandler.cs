﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;
using UnityUIBuilder.Standard.States;

namespace UnityUIBuilder.Standard.Elements
{
    public class AddElementFromConst<TAppData, TModuleData, TElementData> : VElementHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData, IIDData, IResFoldersData, INamespaceData
        where TElementData : IGameObjectData, IControllerData, ICloneData<TElementData>, ICreateChildData<TElementData>
    {
        [Version(typeof(std_1_0))]
        new public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModuleData, TElementData>.External provider)
        {
            switch(name)
            {
                case "controller":
                    return new ControllerState<TAppData, TModuleData, TElementData>(previewData, provider.GetInternal());
                case "component":
                    return new ComponentState<TAppData, TModuleData, TElementData>(previewData, provider.GetInternal());
                case "void":
                    return provider.AddElement(name, previewData.CreateChild(name));
                default:
                    return null;
            }
        }
    }
}