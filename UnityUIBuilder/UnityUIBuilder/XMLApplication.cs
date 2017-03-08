using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DRLib.Parsing.XML;
using DRLib.Algoriphms;
using UnityUIBuilder.Standard;

namespace UnityUIBuilder
{
    public class XMLApplication<TAppData, TModuleData, TElementData>
    {
        public readonly TAppData data;

        readonly IAddElementHandler<TAppData, TModuleData, TElementData> addElementHandler;
        readonly IAddAttributeHandler<TAppData, TModuleData, TElementData> addAttributeHandler;

        public XMLApplication(IAddElementHandler<TAppData, TModuleData, TElementData> addElementHandler,
            IAddElementHandler<TAppData, TModuleData, TElementData> rootAddElementHandler,
            IAddAttributeHandler<TAppData, TModuleData, TElementData> addAttributeHandler)

            : this(Activator.CreateInstance<TAppData>(), addElementHandler, addAttributeHandler)
        { }

        public XMLApplication(TAppData data, IAddElementHandler<TAppData, TModuleData, TElementData> addElementHandler,
            IAddAttributeHandler<TAppData, TModuleData, TElementData> addAttributeHandler)
        {
            this.data = data;
            this.addElementHandler = addElementHandler;
            this.addAttributeHandler = addAttributeHandler;
        }

        public XMLModule<TAppData, TModuleData, TElementData> Perform(string moduleName, TElementData rootData)
        {
            var source = Load(moduleName);

            return Perform(moduleName, source, rootData);
        }

        public XMLModule<TAppData, TModuleData, TElementData> Perform(string moduleName, IEnumerable<char> source, TElementData rootData)
        {
            var module = new XMLModule<TAppData, TModuleData, TElementData>(this, rootData);

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

        public IXMLElement AddElementTo(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
        {
            var result = addElementHandler.AddElement(name, previewElement);
            var bomb = result as BombElement;
            if (bomb != null)
                bomb.Detonate();
            return result;
        }
        public void SetAttributeTo(string name, string value, XMLElementUI<TAppData, TModuleData, TElementData> element)
        {
            var r = addAttributeHandler.AddAttribute(name, value, element);
            if (!r.ok)
                throw new SetAttributeException(name, value, element.name, r.message);
        }
    }

    public class XMLApplication : XMLApplication<AppData, ModuleData, ElementData>
    {
        public XMLApplication(IAddElementHandler<AppData, ModuleData, ElementData> addElementHandler, IAddAttributeHandler<AppData, ModuleData, ElementData> addAttributeHanedler)
            : base(new AppData(), addElementHandler, addAttributeHanedler)
        {

        }

        public XMLApplication()
            :base(new AppData(),
                 new AddElementHandler(),
                 new AddAttributeHandler()
                    )
        {
        }
    }
}
