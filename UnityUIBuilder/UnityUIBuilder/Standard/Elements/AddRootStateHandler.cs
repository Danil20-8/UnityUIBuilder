using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRLib.Parsing.XML;
using UnityEngine;
using UnityUIBuilder.Standard.States;

namespace UnityUIBuilder.Standard.Elements
{
    public class AddRootState<TAppData, TModuleData, TElementData> : VElementHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IDataImport<TModuleData>, INamespaceData, IClassData, IResFoldersData, IModuleVersionData
        where TElementData : ITransformData
    {
        [Version(typeof(std_1_0))]
        new public IXMLElement AddElement(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
        {
            switch(name)
            {
                case "version":
                    return new VersionState<TAppData, TModuleData, TElementData>(previewElement);
                case "using":
                    return new UsingState<TAppData, TModuleData, TElementData>(previewElement);
                case "include":
                    return new IncludeState<TAppData, TModuleData, TElementData>(previewElement);
                case "define":
                    return new DefineState<TAppData, TModuleData, TElementData>(previewElement);
                default:
                    return new BombElement(name, previewElement.name);
            }
        }
    }
}
