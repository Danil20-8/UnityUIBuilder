using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Default.States
{
    public abstract class State<TAppData, TModelData, TElementData> : IXMLElement
    {
        public string name { get; private set; }
        protected XMLModule<TAppData, TModelData, TElementData>.Internal module;

        public State(string name, XMLModule<TAppData, TModelData, TElementData>.Internal module) { this.name = name; this.module = module; }

        public abstract void AddAttribute(string name, string value);

        public abstract IXMLElement AddElement(string name);

        public abstract void SetValue(string value);
    }
}