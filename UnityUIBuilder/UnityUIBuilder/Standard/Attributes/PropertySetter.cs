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
            where TData : IControllerData, IGameObjectData, IResFoldersData, IIDData
        {
            var p = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if(p != null)
                return SetValue(p, obj, value, data);
            return false;
        }

        public static bool SetValue<TData>(PropertyInfo p, object obj, string value, TData data)
            where TData : IControllerData, IGameObjectData, IResFoldersData, IIDData
        {
            if (SetString(p, obj, value)) return true;
            if (SetNumeric(p, obj, value)) return true;
            if (SetEnum(p, obj, value)) return true;
            if (SetColor(p, obj, value)) return true;
            if (SetVector(p, obj, value)) return true;
            if (SetQuaternion(p, obj, value)) return true;
            if (SetUnityEvent(p, obj, value, data.GetController())) return true;
            if (SetGameObject(p, obj, value, data, data, data)) return true;
            if (SetComponent(p, obj, value, data, data, data)) return true;
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

        public static bool SetGameObject(PropertyInfo p, object obj, string value, IIDData idData, IGameObjectData gameObjectData, IResFoldersData resFoldersData)
        {
            if (p.PropertyType != typeof(GameObject))
                return false;

            if(value.IndexOf("id:") == 0)
            {
                idData.WaitForID(value.Replace("id:", ""), go => p.SetValue(obj, go, null));
                return true;
            }

            var gameObject = ValueGetter.GetGameObject(value, idData, gameObjectData);
            if (gameObject == null)
                gameObject = ValueGetter.GetResObject(value, typeof(GameObject), resFoldersData.GetResFolders()) as GameObject;

            if (gameObject != null)
                p.SetValue(obj, gameObject, null);

            return true;

        }

        public static bool SetComponent(PropertyInfo p, object obj, string value, IIDData idData, IGameObjectData gameObjectData, IResFoldersData resFolderData)
        {
            if (p.PropertyType.GetGeneration(typeof(Component)) == -1)
                return false;

            if (value.IndexOf("id:") == 0)
            {
                idData.WaitForID(value.Replace("id:", ""), go => {
                    var c = go.GetComponent(p.PropertyType);
                    if(c != null)
                        p.SetValue(obj, c, null);
                    }
                );
                return true;
            }

            object component = ValueGetter.GetComponent(value, p.PropertyType, idData, gameObjectData);
            if (component == null)
                component = ValueGetter.GetResObject(value, p.PropertyType, resFolderData.GetResFolders());

            if (component != null)
            {
                p.SetValue(obj, component, null);
            }

            return true;
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

        public struct Data : IControllerData, IResFoldersData, IIDData, IGameObjectData
        {
            IControllerData controllerData;
            IResFoldersData resFolderData;
            IIDData idData;
            IGameObjectData gameObjectData;

            public static Data Create<TData1, TData2>(TData1 data1, TData2 data2)
                where TData1 : IGameObjectData, IControllerData
                where TData2 : IResFoldersData, IIDData
            {
                return new Data
                {
                    controllerData = data1,
                    gameObjectData = data1,
                    idData = data2,
                    resFolderData = data2
                };
            }


            public MonoBehaviour GetController()
            {
                return controllerData.GetController();
            }

            public IEnumerable<string> GetResFolders()
            {
                return resFolderData.GetResFolders();
            }

            public GameObject GetObjectByID(string id)
            {
                return idData.GetObjectByID(id);
            }


            public GameObject GetGameObject()
            {
                return gameObjectData.GetGameObject();
            }

            public void WaitForID(string id, Action<GameObject> func)
            {
                idData.WaitForID(id, func);
            }

            void IControllerData.SetController(MonoBehaviour controller)
            {
                throw new NotImplementedException();
            }

            void IResFoldersData.AddResFolder(string folder)
            {
                throw new NotImplementedException();
            }

            void IIDData.AddIDObject(string id, GameObject gameObjetc)
            {
                throw new NotImplementedException();
            }

        }
    }
}
