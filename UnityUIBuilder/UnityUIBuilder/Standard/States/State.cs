using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRLib.Parsing.XML;

namespace UnityUIBuilder.Standard.States
{
    public abstract class State<TAppData, TModelData, TElementData> : IXMLElement
    {
        public string name { get; private set; }
        protected XMLElementUI<TAppData, TModelData, TElementData> element;

        public State(string name, XMLElementUI<TAppData, TModelData, TElementData> element) { this.name = name; this.element = element; }

        public abstract void AddAttribute(string name, string value);

        public abstract IXMLElement AddElement(string name);

        public abstract void SetValue(string value);
    }
}