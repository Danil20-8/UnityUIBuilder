using MyLib.Parsing.XML;
using MyLib.Parsing;
using System;
using System.Collections.Generic;

namespace UnityUIBuilder
{

    public class XMLModule<TAppData, TModuleData, TElementData> : IXMLModule
    {
        public readonly TModuleData data;
        public readonly XMLApplication<TAppData, TModuleData, TElementData> app;
        readonly XMLElementUI<TAppData, TModuleData, TElementData> rootElement;

        readonly Internal _in;

        public XMLModule(XMLApplication<TAppData, TModuleData, TElementData> app, TElementData rootData)
            :this(app, rootData, Activator.CreateInstance<TModuleData>())
        {
        }
        public XMLModule(XMLApplication<TAppData, TModuleData, TElementData> app, TElementData rootData, TModuleData data)
        {
            this.app = app;
            this.data = data;
            _in = new Internal(this);
            this.rootElement = new XMLElementUI<TAppData, TModuleData, TElementData>("root", rootData, _in);
        }

        IXMLElement IXMLModule.AddElement(string name)
        {
            return _in.HandleElement(name);
        }

        IXMLElement AddElement(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
        {
            return _in.HandleElement(name, previewElement);
        }

        public void Perform(IEnumerable<char> source)
        {
            XMLParser xml = new XMLParser();
            try {
                xml.Parse(source, this);
            }
            catch(EndOfSequenceParseException e)
            {
                throw new XMLParseException("XML Module parse exception: syntax error.", e);
            }
        }

        public class External
        {
            protected XMLModule<TAppData, TModuleData, TElementData> module;

            public XMLApplication<TAppData, TModuleData, TElementData> app { get { return module.app; } }
            public TModuleData data { get { return module.data; } }

            public External(XMLModule<TAppData, TModuleData, TElementData> module)
            {
                this.module = module;
            }

            public IXMLElement AddElement(string name, TElementData elementData)
            {
                return new XMLElementUI<TAppData, TModuleData, TElementData>(name, elementData, module._in);
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
            public XMLElementUI<TAppData, TModuleData, TElementData> rootElement { get { return module.rootElement; } }

            public Internal(XMLModule<TAppData, TModuleData, TElementData> module) : base(module) { }

            public IXMLElement HandleElement(string name)
            {
                return app.rootAddElementHandler.AddElement(name, module.rootElement);
            }

            public IXMLElement HandleElement(string name, XMLElementUI<TAppData, TModuleData, TElementData> previewElement)
            {
                return app.addElementHandler.AddElement(name, previewElement);
            }

        }
    }

}