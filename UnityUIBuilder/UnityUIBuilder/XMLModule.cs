using MyLib.Parsing.XML;
using System;
using System.Collections.Generic;

namespace UnityUIBuilder
{

    public class XMLModule<TAppData, TModelData, TElementData> : IXMLModule
    {
        public readonly TModelData data;
        public readonly XMLApplication<TAppData, TModelData, TElementData> app;
        readonly TElementData rootData;

        readonly Internal _in;

        public XMLModule(XMLApplication<TAppData, TModelData, TElementData> app, TElementData rootData)
            :this(app, rootData, Activator.CreateInstance<TModelData>())
        {
        }
        public XMLModule(XMLApplication<TAppData, TModelData, TElementData> app, TElementData rootData, TModelData data)
        {
            this.app = app;
            this.data = data;
            this.rootData = rootData;

            _in = new Internal(this);
        }

        IXMLElement IXMLModule.AddElement(string name)
        {
            return _in.HandleElement(name);
        }

        IXMLElement AddElement(string name, TElementData previewData)
        {
            return _in.HandleElement(name, previewData);
        }

        public void Perform(IEnumerable<char> source)
        {
            XMLParser xml = new XMLParser();
            xml.Parse(source, this);
        }

        public class External
        {
            protected XMLModule<TAppData, TModelData, TElementData> module;

            public XMLApplication<TAppData, TModelData, TElementData> app { get { return module.app; } }
            public TModelData data { get { return module.data; } }

            public External(XMLModule<TAppData, TModelData, TElementData> module)
            {
                this.module = module;
            }

            public IXMLElement AddElement(string name, TElementData elementData)
            {
                return new XMLElementUI<TAppData, TModelData, TElementData>(name, elementData, module._in);
            }

            public Internal GetInternal()
            {
                return module._in;
            }
        }

        /// <summary>
        /// Don't use this in IAddElementHandler. Methods of class will bring you to stack overflow exception.
        /// </summary>
        public class Internal: External
        {
            public TElementData rootData { get { return module.rootData; } }

            public Internal(XMLModule<TAppData, TModelData, TElementData> module) : base(module) { }

            public IXMLElement HandleElement(string name)
            {
                var result = app.rootAddElementHandler.AddElement(name, module.rootData, this);
                if (result != null)
                    return result;
                return HandleElement(name, module.rootData);
            }

            public IXMLElement HandleElement(string name, TElementData previewData)
            {
                return app.addElementHandler.AddElement(name, previewData, this);
            }

        }
    }

}