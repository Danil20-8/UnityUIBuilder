using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;
using UnityUIBuilder.Standard.States;

namespace UnityUIBuilder.Standard.Elements
{
    public class AddRootState<TAppData, TModuleData, TElementData> : VElementHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IDataImport<TModuleData>, INamespaceData, IClassData, IResFoldersData, IModuleVersionData
        where TElementData : ITransformData
    {
        [Version(typeof(std_1_0))]
        new public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModuleData, TElementData>.External module)
        {
            switch(name)
            {
                case "version":
                    return new VersionState<TAppData, TModuleData, TElementData>(module.GetInternal());
                case "using":
                    return new UsingState<TAppData, TModuleData, TElementData>(module.GetInternal());
                case "include":
                    return new IncludeState<TAppData, TModuleData, TElementData>(module.GetInternal());
                case "define":
                    return new DefineState<TAppData, TModuleData, TElementData>(module.GetInternal());
                default:
                    return module.GetInternal().HandleElement(name, previewData);
            }
        }
    }
}
