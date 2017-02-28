using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;

namespace UnityUIBuilder.Standard.Attributes
{
    public static class ValueGetter
    {
        public static Color GetColor(string value)
        {
            if (value[0] == '#')
            {
                var c = int.Parse(value.Substring(1), System.Globalization.NumberStyles.HexNumber);
                return new Color(((float)((c >> 16)) % 256) / 255f, ((float)((c >> 8)) % 256) / 255f, ((float)(c % 256)) / 255f);
            }
            else if (value[0] == '(')
            {
                float[] rgba = value.Trim('(', ')').Split(',').Select(c => float.Parse(c)).ToArray();
                switch (rgba.Length)
                {
                    case 3:
                        return new Color(rgba[0], rgba[1], rgba[2]);
                    case 4:
                        return new Color(rgba[0], rgba[1], rgba[2], rgba[3]);
                }
            }
            else
            {
                var cp = typeof(Color).GetProperty(value, BindingFlags.Static | BindingFlags.Public);
                if (cp != null)
                    return (Color)cp.GetValue(null, null);
            }

            return Color.white;
        }
        public static UnityAction GetUnityAction(string value, MonoBehaviour controller)
        {
            var m = controller.GetType().GetMethod(value, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (m != null && m.ReturnType == typeof(void) && m.GetParameters().Length == 0)
                return Delegate.CreateDelegate(typeof(UnityAction), controller, m) as UnityAction;
            return null;
        }
        public static Vector4 GetVector4(string value)
        {
            var v = value.Trim('(', ')').Split(',');
            return new Vector4(float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]), float.Parse(v[3]));
        }
        public static Vector3 GetVector3(string value)
        {
            var v = value.Trim('(', ')').Split(',');
            return new Vector3(float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]));
        }
        public static Vector2 GetVector2(string value)
        {
            var v = value.Trim('(', ')').Split(',');
            return new Vector2(float.Parse(v[0]), float.Parse(v[1]));
        }
        public static Quaternion GetQuaternion(string value)
        {
            var v = value.Trim('(', ')').Split(',');
            return Quaternion.Euler(float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]));
        }
        public static UnityEngine.Object GetResObject(string value, Type resType, IEnumerable<string> folders)
        {
            foreach (var f in folders)
            {
                var res = Resources.Load(f + "/" + value, resType);
                if (res != null)
                    return res;
            }
            return null;
        }
    }
}
