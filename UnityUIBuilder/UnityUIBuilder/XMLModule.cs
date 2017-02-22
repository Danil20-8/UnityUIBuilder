using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;
using System.Reflection;
using UnityUIBuilder.Default;
using UnityUIBuilder.Default.Handlers;

namespace UnityUIBuilder
{

    public class XMLModule<TAppData, TModelData> : IXMLModule
    {
        public readonly TModelData data;
        public readonly XMLApplication<TAppData, TModelData> app;
        public readonly Transform transform;

        readonly Internal _in;

        public XMLModule(XMLApplication<TAppData, TModelData> app, Transform transform)
            :this(app, transform, Activator.CreateInstance<TModelData>())
        {
        }
        public XMLModule(XMLApplication<TAppData, TModelData> app, Transform transform, TModelData data)
        {
            this.app = app;
            this.data = data;
            this.transform = transform;

            _in = new Internal(this);
        }

        IXMLElement IXMLModule.AddElement(string name)
        {
            return _in.HandleElement(name);
        }

        IXMLElement AddElement(string name, Transform parent, MonoBehaviour controller)
        {
            return _in.HandleElement(name, parent, controller);
        }

        protected virtual void HandleObject(GameObject gameObject, Transform parent)
        {
            RectTransform rt = gameObject.GetComponent<RectTransform>();
            if (rt == null)
                rt = gameObject.AddComponent<RectTransform>();

            rt.SetParent(parent);

            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.zero;
            rt.anchoredPosition = Vector3.zero;
        }

        public void Perform(IEnumerable<char> source)
        {
            XMLParser xml = new XMLParser();
            xml.Parse(source, this);
        }

        public class External
        {
            protected XMLModule<TAppData, TModelData> module;

            public XMLApplication<TAppData, TModelData> app { get { return module.app; } }
            public TModelData data { get { return module.data; } }
            public Transform transform { get { return module.transform; } }

            public External(XMLModule<TAppData, TModelData> module)
            {
                this.module = module;
            }

            public IXMLElement AddElement(string name, GameObject gameObject, Transform parent, MonoBehaviour controller)
            {
                module.HandleObject(gameObject, parent);
                return new XMLElementUI<TAppData, TModelData>(name, gameObject, controller, module._in);
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
            public Internal(XMLModule<TAppData, TModelData> module) : base(module) { }

            public IXMLElement HandleElement(string name)
            {
                var result = app.rootAddElementHandler.AddElement(name, transform, null, this);
                if (result != null)
                    return result;
                return HandleElement(name, transform, null);
            }

            public IXMLElement HandleElement(string name, Transform parent, MonoBehaviour controller)
            {
                return app.addElementHandler.AddElement(name, parent, controller, this);
            }

        }
    }

}