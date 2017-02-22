using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib.Parsing.XML;
using MyLib.Algoriphms;
using System;
using System.Reflection;
using UnityEngine.Events;
using UnityUIBuilder.Default;

namespace UnityUIBuilder
{
    public class XMLElementUI<TAppData, TModelData, TElementData> : IXMLElement
    {
        public readonly XMLModule<TAppData, TModelData, TElementData>.Internal module;
        public readonly TElementData data;


        public string name { get; private set; }

        public XMLElementUI(string name, TElementData data, XMLModule<TAppData, TModelData, TElementData>.Internal module)
        {
            this.name = name;
            this.data = data;
            this.module = module;
        }

        public void AddAttribute(string name, string value)
        {
            module.app.addAttributeHandler.AddAttribute(name, value, this);
        }

        public IXMLElement AddElement(string name)
        {
            return module.HandleElement(name, data);
        }

        public void SetValue(string value)
        {
            AddAttribute("text", value);
        }
    }
}