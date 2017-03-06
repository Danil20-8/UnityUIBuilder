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
        public readonly XMLElementUI<TAppData, TModuleData, TElementData> rootElement;

        public XMLModule(XMLApplication<TAppData, TModuleData, TElementData> app, TElementData rootData)
            :this(app, rootData, Activator.CreateInstance<TModuleData>())
        {
        }
        public XMLModule(XMLApplication<TAppData, TModuleData, TElementData> app, TElementData rootData, TModuleData data)
        {
            this.app = app;
            this.data = data;
            this.rootElement = new XMLElementUI<TAppData, TModuleData, TElementData>("root", rootData, this);
        }

        IXMLElement IXMLModule.AddElement(string name)
        {
            return app.rootAddElementHandler.AddElement(name, rootElement);
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
        }
    }

}