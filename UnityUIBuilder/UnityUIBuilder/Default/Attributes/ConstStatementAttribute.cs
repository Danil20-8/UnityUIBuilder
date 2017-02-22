using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

namespace UnityUIBuilder.Default.Attributes
{
    public class ConstStatementAttribute<TAppData, TModelData> : IAddAttributeHandler<TAppData, TModelData> where TModelData : IClassData where TAppData : IIDData
    {
        /// <summary>
        /// Derives from class. You can define a class with using statement. Example : class="className".
        /// </summary>
        public const string class_st = "class";
        /// <summary>
        /// Adds to application id list. You can get gameObject of this element by id after perfoming ends. Example: id="myID".
        /// </summary>
        public const string id_st = "id";
        /// <summary>
        /// Call a controller method with GameObject parameter. Example: call="SayHello".
        /// </summary>
        public const string call_st = "call";

        public bool AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModelData> element)
        {
            switch(attributeName)
            {
                case class_st:
                    foreach (var a in element.module.data.GetClassAttributes(attributeValue))
                        element.AddAttribute(a.name, a.value);
                    return true;
                case id_st:
                    element.module.app.data.AddIDObject(attributeValue, element.gameObject);
                    return true;
                case call_st:
                    var m = element.controller.GetType().GetMethod(attributeValue, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(GameObject) }, null);
                    if (m != null) m.Invoke(element.controller, new object[] { element.gameObject });
                    else element.module.app.PushError(element.controller + " has no a method name " + attributeValue);
                    return true;
                default:
                    return false;
            }
        }
    }
}
