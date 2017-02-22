using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;

namespace UnityUIBuilder.Default.States
{
    public abstract class State<TAppData, TModelData> : IXMLElement
    {
        public string name { get; private set; }
        protected XMLModule<TAppData, TModelData>.Internal module;

        public State(string name, XMLModule<TAppData, TModelData>.Internal module) { this.name = name; this.module = module; }

        public abstract void AddAttribute(string name, string value);

        public abstract IXMLElement AddElement(string name);

        public abstract void SetValue(string value);
    }
}