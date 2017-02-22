using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;
using UnityUIBuilder.Default.States;

namespace UnityUIBuilder.Default.Handlers
{
    public class AddRootState<TAppData, TModelData> : IAddElementHandler<TAppData, TModelData> where TModelData : IDataImport<TModelData>, INamespaceData, IClassData, IResFoldersData
    {
        public IXMLElement AddElement(string name, Transform parent, MonoBehaviour controller, XMLModule<TAppData, TModelData>.External module)
        {
            switch(name)
            {
                case "using":
                    return new UsingState<TAppData, TModelData>(module.GetInternal());
                case "include":
                    return new IncludeState<TAppData, TModelData>(module.GetInternal());
                case "define":
                    return new DefineState<TAppData, TModelData>(module.GetInternal());
                default:
                    return module.GetInternal().HandleElement(name, parent, controller);
            }
        }
    }
}
