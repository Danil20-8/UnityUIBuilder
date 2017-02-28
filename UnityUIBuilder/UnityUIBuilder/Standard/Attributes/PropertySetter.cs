using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MyLib.Algoriphms;
using UnityEngine;
using UnityEngine.Events;

namespace UnityUIBuilder.Standard.Attributes
{
    public static class PropertySetter
    {
        public static bool SetValue<TData>(object obj, string propertyName, string value, TData data)
            where TData : IControllerData, IResFoldersData
        {
            var p = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if(p != null)
                return SetValue(p, obj, value, data);
            return false;
        }

        public static bool SetValue<TData>(PropertyInfo p, object obj, string value, TData data)
            where TData : IControllerData, IResFoldersData
        {
            if (SetString(p, obj, value)) return true;
            if (SetNumeric(p, obj, value)) return true;
            if (SetEnum(p, obj, value)) return true;
            if (SetColor(p, obj, value)) return true;
            if (SetVector(p, obj, value)) return true;
            if (SetQuaternion(p, obj, value)) return true;
            if (SetUnityEvent(p, obj, value, data.GetController())) return true;
            if (SetResObject(p, obj, value, data.GetResFolders())) return true;
            return false;
        }

        public static bool SetString(PropertyInfo p, object obj, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                p.SetValue(obj, value, null);
                return true;
            }
            return false;
        }
        public static bool SetNumeric(PropertyInfo p, object obj, string value)
        {
            var ptype = p.PropertyType;
            if (ptype == typeof(int))
                p.SetValue(obj, int.Parse(value), null);
            else if (ptype == typeof(float))
                p.SetValue(obj, float.Parse(value), null);
            else
                return false;
            return true;
        }
        public static bool SetEnum(PropertyInfo p, object obj, string value)
        {
            Type et = p.PropertyType;
            if (et.IsEnum)
            {
                p.SetValue(obj, Enum.Parse(et, value), null);
                return true;
            }
            return false;
        }
        public static bool SetColor(PropertyInfo p, object obj, string value)
        {
            if (p.PropertyType == typeof(Color))
            {
                p.SetValue(obj, ValueGetter.GetColor(value), null);
                return true;
            }
            return false;
        }
        public static bool SetUnityEvent(PropertyInfo p, object obj, string value, MonoBehaviour controller)
        {
            if (p.PropertyType.GetGeneration(typeof(UnityEvent)) == -1)
                return false;

            var ev = p.GetValue(obj, null) as UnityEvent;
            if (ev == null)
            {
                ev = Activator.CreateInstance(p.PropertyType) as UnityEvent;
                p.SetValue(obj, ev, null);
            }

            ev.AddListener(ValueGetter.GetUnityAction(value, controller));
            return true;
        }
        public static bool SetVector(PropertyInfo p, object obj, string value)
        {
            var type = p.PropertyType;
            if (type == typeof(Vector4))
                p.SetValue(obj, ValueGetter.GetVector4(value), null);
            else if (type == typeof(Vector3))
                p.SetValue(obj, ValueGetter.GetVector3(value), null);
            else if (type == typeof(Vector2))
                p.SetValue(obj, ValueGetter.GetVector2(value), null);
            else
                return false;
            return true;
        }
        public static bool SetQuaternion(PropertyInfo p, object obj, string value)
        {
            if (p.PropertyType == typeof(Quaternion))
            {
                p.SetValue(obj, ValueGetter.GetQuaternion(value), null);
                return true;
            }
            return false;

        }

        public static bool SetResObject(PropertyInfo p, object obj, string value, IEnumerable<string> folders)
        {
            if (p.PropertyType.GetGeneration(typeof(UnityEngine.Object)) != 1)
            {
                var res = ValueGetter.GetResObject(value, p.PropertyType, folders);
                if (res != null)
                    p.SetValue(obj, res, null);
                return true;
            }
            return false;
        }

        public struct Data : IControllerData, IResFoldersData
        {
            IControllerData controllerData;
            IResFoldersData resFolderData;

            public Data(IControllerData controllerData, IResFoldersData resFolderData)
            {
                this.controllerData = controllerData;
                this.resFolderData = resFolderData;
            }

            public void AddResFolder(string folder)
            {
            }

            public MonoBehaviour GetController()
            {
                return controllerData.GetController();
            }

            public IEnumerable<string> GetResFolders()
            {
                return resFolderData.GetResFolders();
            }

            public void SetController(MonoBehaviour controller)
            {
            }
        }
    }
}
