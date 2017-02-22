using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;
using UnityUIBuilder.Default.States;

namespace UnityUIBuilder.Default.Handlers
{
    public class AddRootState<TAppData, TModelData, TElementData> : IAddElementHandler<TAppData, TModelData, TElementData>
        where TModelData : IDataImport<TModelData>, INamespaceData, IClassData, IResFoldersData
        where TElementData : ITransformData
    {
        public IXMLElement AddElement(string name, TElementData previewData, XMLModule<TAppData, TModelData, TElementData>.External module)
        {
            switch(name)
            {
                case "using":
                    return new UsingState<TAppData, TModelData, TElementData>(module.GetInternal());
                case "include":
                    return new IncludeState<TAppData, TModelData, TElementData>(module.GetInternal());
                case "define":
                    return new DefineState<TAppData, TModelData, TElementData>(module.GetInternal());
                default:
                    return module.GetInternal().HandleElement(name, previewData);
            }
        }
    }
}
