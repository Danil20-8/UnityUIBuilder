﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

namespace UnityUIBuilder.Standard.Attributes
{
    public class ConstStatementAttribute<TAppData, TModelData, TElementData> : VAttributeHandler<TAppData, TModelData, TElementData>
        where TModelData : IClassData, IModuleVersionData, IIDData
        where TAppData : IIDData
        where TElementData : IGameObjectData, IControllerData
    {
        /// <summary>
        /// Derives from class. You can define a class with using statement. Example : class="className".
        /// </summary>
        public const string class_st = "class";
        /// <summary>
        /// Adds to application id list. You can get gameObject of this element by id after perfoming ends. Example: id="myID".
        /// </summary>
        public const string id_st = "id";
        public const string globalid_st = "globalID";
        /// <summary>
        /// Call a controller method with GameObject parameter. Example: call="SayHello".
        /// </summary>
        public const string call_st = "call";

        [Version(Versions.std_v_1_0, true)]
        new public bool AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModelData, TElementData> element)
        {
            switch(attributeName)
            {
                case class_st:
                    foreach (var a in element.module.data.GetClassAttributes(attributeValue))
                        element.AddAttribute(a.name, a.value);
                    return true;
                case globalid_st:
                    element.module.app.data.AddIDObject(attributeValue, element.data.GetGameObject());
                    return true;
                case id_st:
                    element.module.data.AddIDObject(attributeValue, element.data.GetGameObject());
                    return true;
                case call_st:
                    var controller = element.data.GetController();
                    var gameObject = element.data.GetGameObject();

                    var m = controller.GetType().GetMethod(attributeValue, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(GameObject) }, null);
                    if (m != null) m.Invoke(controller, new object[] { gameObject });
                    else element.module.app.PushError(controller + " has no a method name " + attributeValue);
                    return true;
                default:
                    return false;
            }
        }
    }
}