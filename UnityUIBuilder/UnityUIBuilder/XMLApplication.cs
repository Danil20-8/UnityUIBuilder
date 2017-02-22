using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityUIBuilder.Default;
using UnityUIBuilder.Default.Handlers;
using UnityUIBuilder.Default.Attributes;

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

            if (source != null)
                return Perform(moduleName, source, rootData);
            else {
                PushError(moduleName + " is not found");
                return null;
            }
        }

        public XMLModule<TAppData, TModelData, TElementData> Perform(string moduleName, IEnumerable<char> source, TElementData rootData)
        {
            var module = new XMLModule<TAppData, TModelData, TElementData>(this, rootData);

            module.Perform(source);

            return module;
        }

        protected virtual IEnumerable<char> Load(string moduleName)
        {
            return Resources.Load<TextAsset>(moduleName).text;
        }

        public void PushError(string format, params object[] args)
        {
            PushError(string.Format(format, args));
        }

        public void PushError<T>(T message)
        {
            PushError(message.ToString());
        }

        public virtual void PushError(string message)
        {
            throw new Exception(message);
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
                    new AddElementHandlerList<AppData, ModuleData, ElementData>(
                        new AddElementState<AppData, ModuleData, ElementData>(),
                        new AddElementFromUnityRes<AppData, ModuleData, ElementData>(),
                        new AddElementFromAssemlies<AppData, ModuleData, ElementData>()
                        ),
                    new AddRootState<AppData, ModuleData, ElementData>(),
                    new AttributesList<AppData, ModuleData, ElementData>(
                        new AttributesForUI<AppData, ModuleData, ElementData>(),
                        new ConstStatementAttribute<AppData, ModuleData, ElementData>(),
                        new SetPropertyAttribute<AppData, ModuleData, ElementData>(),
                        new SetPropertyFromUnityRes<AppData, ModuleData, ElementData>()
                        )
                    )
        {
        }
    }
}
