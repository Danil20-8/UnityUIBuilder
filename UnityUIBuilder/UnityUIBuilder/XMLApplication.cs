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
    public class XMLApplication<TAppData, TModelData>
    {
        public readonly TAppData data;

        public readonly IAddElementHandler<TAppData, TModelData> addElementHandler;
        public readonly IAddElementHandler<TAppData, TModelData> rootAddElementHandler;

        public readonly IAddAttributeHandler<TAppData, TModelData> addAttributeHandler;

        public XMLApplication(IAddElementHandler<TAppData, TModelData> addElementHandler,
            IAddElementHandler<TAppData, TModelData> rootAddElementHandler,
            IAddAttributeHandler<TAppData, TModelData> addAttributeHandler)

            : this(Activator.CreateInstance<TAppData>(), addElementHandler, rootAddElementHandler, addAttributeHandler)
        { }

        public XMLApplication(TAppData data, IAddElementHandler<TAppData, TModelData> addElementHandler,
            IAddElementHandler<TAppData, TModelData> rootAddElementHandler,
            IAddAttributeHandler<TAppData, TModelData> addAttributeHandler)
        {
            this.data = data;
            this.addElementHandler = addElementHandler;
            this.rootAddElementHandler = rootAddElementHandler;
            this.addAttributeHandler = addAttributeHandler;
        }

        public XMLModule<TAppData, TModelData> Perform(string moduleName, Transform transform)
        {
            var source = Load(moduleName);

            if (source != null)
                return Perform(moduleName, source, transform);
            else {
                PushError(moduleName + " is not found");
                return null;
            }
        }

        public XMLModule<TAppData, TModelData> Perform(string moduleName, IEnumerable<char> source, Transform transform)
        {
            var module = new XMLModule<TAppData, TModelData>(this, transform);

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

    public class XMLApplication<TModuleData> : XMLApplication<AppData, TModuleData>
    {
        public XMLApplication(IAddElementHandler<AppData, TModuleData> addElementHandler, IAddElementHandler<AppData, TModuleData> rootAddElementHandler, IAddAttributeHandler<AppData, TModuleData> addAttributeHanedler)
            : base(new AppData(), addElementHandler, rootAddElementHandler, addAttributeHanedler)
        {

        }
    }

    public class XMLApplication : XMLApplication<AppData, ModuleData>
    {
        public XMLApplication(IAddElementHandler<AppData, ModuleData> addElementHandler, IAddElementHandler<AppData, ModuleData> rootAddElementHandler, IAddAttributeHandler<AppData, ModuleData> addAttributeHanedler)
            : base(new AppData(), addElementHandler, rootAddElementHandler, addAttributeHanedler)
        {

        }

        public XMLApplication()
            :base(new AppData(),
                    new AddElementHandlerList<AppData, ModuleData>(
                        new AddElementState<AppData, ModuleData>(),
                        new AddElementFromUnityRes<AppData, ModuleData>(),
                        new AddElementFromAssemlies<AppData, ModuleData>()
                        ),
                    new AddRootState<AppData, ModuleData>(),
                    new AttributesList<AppData, ModuleData>(
                        new AttributesForUI<AppData, ModuleData>(),
                        new ConstStatementAttribute<AppData, ModuleData>(),
                        new SetPropertyAttribute<AppData, ModuleData>(),
                        new SetPropertyFromUnityRes<AppData, ModuleData>()
                        )
                    )
        {
        }
    }
}
