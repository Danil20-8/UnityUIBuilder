using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityUIBuilder.Standard;
using UnityUIBuilder.Standard.Elements;
using UnityUIBuilder.Standard.Attributes;

namespace UnityUIBuilder
{
    public class XMLApplication<TAppData, TModelData, TElementData>
    {
        public readonly TAppData data;

        public readonly IAddElementHandler<TAppData, TModelData, TElementData> addElementHandler;
        public readonly IAddElementHandler<TAppData, TModelData, TElementData> rootAddElementHandler;

        public readonly IAddAttributeHandler<TAppData, TModelData, TElementData> addAttributeHandler;

        public XMLApplication(IAddElementHandler<TAppData, TModelData, TElementData> addElementHandler,
            IAddElementHandler<TAppData, TModelData, TElementData> rootAddElementHandler,
            IAddAttributeHandler<TAppData, TModelData, TElementData> addAttributeHandler)

            : this(Activator.CreateInstance<TAppData>(), addElementHandler, rootAddElementHandler, addAttributeHandler)
        { }

        public XMLApplication(TAppData data, IAddElementHandler<TAppData, TModelData, TElementData> addElementHandler,
            IAddElementHandler<TAppData, TModelData, TElementData> rootAddElementHandler,
            IAddAttributeHandler<TAppData, TModelData, TElementData> addAttributeHandler)
        {
            this.data = data;
            this.addElementHandler = addElementHandler;
            this.rootAddElementHandler = rootAddElementHandler;
            this.addAttributeHandler = addAttributeHandler;
        }

        public XMLModule<TAppData, TModelData, TElementData> Perform(string moduleName, TElementData rootData)
        {
            var source = Load(moduleName);

            return Perform(moduleName, source, rootData);
        }

        public XMLModule<TAppData, TModelData, TElementData> Perform(string moduleName, IEnumerable<char> source, TElementData rootData)
        {
            var module = new XMLModule<TAppData, TModelData, TElementData>(this, rootData);

            module.Perform(source);

            return module;
        }

        protected virtual IEnumerable<char> Load(string moduleName)
        {
            var result = Resources.Load<TextAsset>(moduleName);
            if (result != null)
                return result.text;

            throw new Exception("Module " + moduleName + " is not found");
        }

        public void Log(string format, params object[] args)
        {
            Log(string.Format(format, args));
        }

        public void Log<T>(T message)
        {
            Log(message.ToString());
        }

        public virtual void Log(string message)
        {
            UnityEngine.Debug.Log(message);
        }
    }

    public class XMLApplication : XMLApplication<AppData, ModuleData, ElementData>
    {
        public XMLApplication(IAddElementHandler<AppData, ModuleData, ElementData> addElementHandler, IAddElementHandler<AppData, ModuleData, ElementData> rootAddElementHandler, IAddAttributeHandler<AppData, ModuleData, ElementData> addAttributeHanedler)
            : base(new AppData(), addElementHandler, rootAddElementHandler, addAttributeHanedler)
        {

        }

        public XMLApplication()
            :base(new AppData(),
                    new VListElementHandler<AppData, ModuleData, ElementData>(
                        new AddElementFromConst<AppData, ModuleData, ElementData>(),
                        new AddElementFromUnityRes<AppData, ModuleData, ElementData>(),
                        new AddElementFromAssemblies<AppData, ModuleData, ElementData>()
                        ),
                    new AddRootState<AppData, ModuleData, ElementData>(),
                    new VListAttributeHandler<AppData, ModuleData, ElementData>(
                        new AttributesForUI<AppData, ModuleData, ElementData>(),
                        new ConstStatementAttribute<AppData, ModuleData, ElementData>(),
                        new SetPropertyAttribute<AppData, ModuleData, ElementData>()
                        )
                    )
        {
        }
    }
}
