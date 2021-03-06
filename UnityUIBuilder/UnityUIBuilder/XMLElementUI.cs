﻿using DRLib.Parsing.XML;

namespace UnityUIBuilder
{
    public class XMLElementUI<TAppData, TModuleData, TElementData> : IXMLElement
    {
        public readonly XMLModule<TAppData, TModuleData, TElementData> module;
        public readonly TElementData data;

        public bool isRoot { get { return module.rootElement == this; } }

        public string name { get; private set; }

        public XMLElementUI(string name, TElementData data, XMLModule<TAppData, TModuleData, TElementData> module)
        {
            this.name = name;
            this.data = data;
            this.module = module;
        }

        public IXMLElement AddElement(string name)
        {
            return module.app.AddElementTo(name, this);
        }

        public IXMLElement CreateElement(string name, TElementData data)
        {
            return new XMLElementUI<TAppData, TModuleData, TElementData>(name, data, module);
        }

        public void AddAttribute(string name, string value)
        {
            module.app.SetAttributeTo(name, value, this);
        }

        void IXMLElement.SetValue(string value)
        {
            (this as IXMLElement).AddAttribute("text", value);
        }
    }
}