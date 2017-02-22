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
    public static class ValueSetter
    {
        public static bool SetValue<TData>(PropertyInfo p, object obj, string value, TData data) where TData : IControllerData
        {
            if (SetString(p, obj, value)) return true;
            if (SetParsable(p, obj, value)) return true;
            if (SetEnum(p, obj, value)) return true;
            if (SetColor(p, obj, value)) return true;
            if (SetVector(p, obj, value)) return true;
            if (SetQuaternion(p, obj, value)) return true;
            if (SetUnityEvent(p, obj, value, data.GetController())) return true;

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
        public static bool SetParsable(PropertyInfo p, object obj, string value)
        {
            var pm = p.PropertyType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
            if (pm != null)
            {
                p.SetValue(obj, pm.Invoke(null, new object[] { value }), null);
                return true;
            }
            return false;
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
            if (p.PropertyType != typeof(Color))
                return false;

            if (value[0] == '#')
            {
                var c = int.Parse(value.Substring(1), System.Globalization.NumberStyles.HexNumber);
                p.SetValue(obj, new Color(((float)((c >> 16)) % 256) / 256f, ((float)((c >> 8)) % 256) / 256f, ((float)(c % 256)) / 256f), null);
            }
            else if (value[0] == '(')
            {
                float[] rgba = value.Trim('(', ')').Split(',').Select(c => float.Parse(c)).ToArray();
                switch (rgba.Length)
                {
                    case 3:
                        p.SetValue(obj, new Color(rgba[0], rgba[1], rgba[2]), null);
                        break;
                    case 4:
                        p.SetValue(obj, new Color(rgba[0], rgba[1], rgba[2], rgba[3]), null);
                        break;
                }
            }
            else
            {
                var cp = typeof(Color).GetProperty(value, BindingFlags.Static | BindingFlags.Public);
                if (cp != null)
                    p.SetValue(obj, (Color)cp.GetValue(null, null), null);
            }

            return true;
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

            var m = controller.GetType().GetMethod(value, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (m != null && m.ReturnType == typeof(void) && m.GetParameters().Length == 0)
            {
                var ua = Delegate.CreateDelegate(typeof(UnityAction), controller, m) as UnityAction;
                ev.AddListener(ua);
            }
            return true;
        }
        public static bool SetVector(PropertyInfo p, object obj, string value)
        {
            var type = p.PropertyType;
            if (type == typeof(Vector4))
            {
                var v = value.Trim('(', ')').Split(',');
                p.SetValue(obj, new Vector4(float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]), float.Parse(v[3])), null);

            }
            else if (type == typeof(Vector3))
            {
                var v = value.Trim('(', ')').Split(',');
                p.SetValue(obj, new Vector3(float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2])), null);
            }
            else if (type == typeof(Vector2))
            {
                var v = value.Trim('(', ')').Split(',');
                p.SetValue(obj, new Vector2(float.Parse(v[0]), float.Parse(v[1])), null);
            }
            else
                return false;
            return true;
        }
        public static bool SetQuaternion(PropertyInfo p, object obj, string value)
        {
            if (p.PropertyType != typeof(Quaternion))
                return false;

            var v = value.Trim('(', ')').Split(',');
            if (v.Length == 3)
                p.SetValue(obj, Quaternion.Euler(float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2])), null);

            return true;
        }
    }
}
