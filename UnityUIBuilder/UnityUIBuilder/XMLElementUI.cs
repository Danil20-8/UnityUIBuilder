using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib.Parsing.XML;
using MyLib.Algoriphms;
using System;
using System.Reflection;
using UnityEngine.Events;

namespace UnityUIBuilder
{
    public class XMLElementUI<TAppData, TModelData> : IXMLElement
    {
        public readonly GameObject gameObject;
        public readonly MonoBehaviour controller;
        public readonly XMLModule<TAppData, TModelData>.Internal module;

        public string name { get; private set; }

        public XMLElementUI(string name, GameObject gameObject, MonoBehaviour controller, XMLModule<TAppData, TModelData>.Internal module)
        {
            this.name = name;
            this.gameObject = gameObject;
            this.controller = controller;
            this.module = module;
        }

        public void AddAttribute(string name, string value)
        {
            module.app.addAttributeHandler.AddAttribute(name, value, this);
        }

        public IXMLElement AddElement(string name)
        {
            return module.HandleElement(name, gameObject.transform, controller);
        }

        public void SetValue(string value)
        {
            AddAttribute("text", value);
        }
    }
}