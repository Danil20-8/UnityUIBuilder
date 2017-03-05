using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;

namespace UnityUIBuilder.Standard.Attributes
{
    public class AttributesForUI<TAppData, TModuleData, TElementData> : VAttributeHandler<TAppData, TModuleData, TElementData>
        where TModuleData : IModuleVersionData
        where TElementData : IGameObjectData
    {
        [Version(typeof(std_1_0))]
        new public bool AddAttribute(string attributeName, string value, XMLElementUI<TAppData, TModuleData, TElementData> element)
        {
            var rt = element.data.GetGameObject().GetComponent<RectTransform>();
            if (rt == null)
                return false;
            try
            {
                switch (attributeName)
                {
                    case "left":
                        Left(rt, value);
                        return true;
                    case "right":
                        Right(rt, value);
                        return true;
                    case "bottom":
                        Bottom(rt, value);
                        return true;
                    case "top":
                        Top(rt, value);
                        return true;
                    case "width":
                        Width(rt, value);
                        return true;
                    case "height":
                        Height(rt, value);
                        return true;
                    case "side":
                        Side(rt, value);
                        return true;
                    case "anchor":
                        Anchor(rt, value);
                        return true;
                }
            }
            catch(Exception e)
            {
                throw new SetAttributeException(attributeName, value, element.name, e);
            }
            return false;
        }


        float GetValue(float parentSize, string value)
        {
            if (value[value.Length - 1] == '%')
            {
                return parentSize * float.Parse(value.Substring(0, value.Length - 1)) / 100;
            }
            else
                return float.Parse(value);
        }

        bool IsRelative(string value) { return value[value.Length - 1] == '%'; }
        bool GetValue(string value, out float result)
        {
            if (IsRelative(value))
            {
                result = float.Parse(value.Substring(0, value.Length - 1)) / 100;
                return true;
            }
            else
            {
                result = float.Parse(value);
                return false;
            }
        }

        RectTransform GetParent(RectTransform transform) { var parent = transform.parent; return parent == null ? null : parent.GetComponent<RectTransform>(); }

        float GetParentWidth(RectTransform transform) { var p = GetParent(transform); return p == null ? 0 : p.rect.width; }

        float GetParentHeigth(RectTransform transform) { var p = GetParent(transform); return p == null ? 0 : p.rect.height; }

        //Position

        void Left(RectTransform transform, string value)
        {
            float left;
            if (GetValue(value, out left))
            {
                var anch = transform.anchorMin;
                anch.x = left;
                var pos = transform.anchoredPosition;
                pos.x = 0;

                transform.anchorMin = anch;
                transform.anchoredPosition = pos;
            }
            else
            {
                Vector3 pos = transform.anchoredPosition;
                Vector3 size = transform.sizeDelta;
                float right = pos.x + size.x / 2;

                size.x = (right - left);
                pos.x = size.x / 2 + left;

                var anch = transform.anchorMin;
                anch.x = 0;

                transform.anchorMin = anch;
                transform.anchoredPosition = pos;
                transform.sizeDelta = size;
            }
        }
        void Right(RectTransform transform, string value)
        {
            float right;
            if (GetValue(value, out right))
            {
                var anch = transform.anchorMax;
                anch.x = right;
                var pos = transform.sizeDelta;
                pos.x = 0;

                transform.anchorMax = anch;
                transform.sizeDelta = pos;
            }
            else
            {
                Vector3 pos = transform.anchoredPosition;
                Vector3 size = transform.sizeDelta;
                float left = pos.x - size.x / 2;

                size.x = (-right - left);
                pos.x = size.x / 2 + left;

                var anch = transform.anchorMax;
                anch.x = 1;

                transform.anchorMax = anch;
                transform.anchoredPosition = pos;
                transform.sizeDelta = size;
            }
        }
        void Top(RectTransform transform, string value)
        {
            float top;
            if (GetValue(value, out top))
            {
                var anch = transform.anchorMax;
                anch.y = top;
                var pos = transform.sizeDelta;
                pos.y = 0;

                transform.anchorMax = anch;
                transform.sizeDelta = pos;
            }
            else
            {
                Vector3 pos = transform.anchoredPosition;
                Vector3 size = transform.sizeDelta;
                float bottom = pos.y - size.y / 2;

                size.y = (-top - bottom);
                pos.y = size.y / 2 + bottom;

                var anch = transform.anchorMax;
                anch.y = 1;

                transform.anchorMax = anch;
                transform.anchoredPosition = pos;
                transform.sizeDelta = size;
            }
        }
        void Bottom(RectTransform transform, string value)
        {
            float bottom;
            if (GetValue(value, out bottom))
            {
                var anch = transform.anchorMin;
                anch.y = bottom;
                var pos = transform.anchoredPosition;
                pos.y = 0;

                transform.anchorMin = anch;
                transform.anchoredPosition = pos;
            }
            else
            {
                Vector3 pos = transform.anchoredPosition;
                Vector3 size = transform.sizeDelta;
                float top = pos.y + size.y / 2;

                size.y = (top - bottom);
                pos.y = size.y / 2 + bottom;

                var anch = transform.anchorMin;
                anch.y = 0;

                transform.anchorMin = anch;
                transform.anchoredPosition = pos;
                transform.sizeDelta = size;
            }
        }

        //Size

        void Width(RectTransform transform, string value)
        {
            if (transform.anchorMax.x == transform.anchorMin.x)
            {
                var size = transform.sizeDelta;
                var pos = transform.anchoredPosition;
                float t;
                if (GetValue(value, out t))
                {
                    pos.x = t;
                    transform.anchorMax += pos;
                    size.x = 0;
                    transform.sizeDelta = size;
                }
                else
                {
                    size.x = 0;
                    transform.sizeDelta = size;
                }
            }
        }

        void Height(RectTransform transform, string value)
        {
            if (transform.anchorMax.y == transform.anchorMin.y)
            {
                var size = transform.sizeDelta;
                var pos = transform.anchoredPosition;
                float t;
                if (GetValue(value, out t))
                {
                    pos.y = t;
                    transform.anchorMax += pos;
                    size.y = 0;
                    transform.sizeDelta = size;
                }
                else
                {
                    size.y = 0;
                    transform.sizeDelta = size;
                }
            }
        }

        void Side(RectTransform transform, string value)
        {
            if(transform.anchorMax == transform.anchorMin)
            {
                float size;
                if(GetValue(value, out size))
                {
                    Vector2 result = new Vector2(size, size);

                    var p = transform.parent as RectTransform;
                    if(p != null)
                    {
                        if(p.rect.width > p.rect.height)
                            result.x *= p.rect.height / p.rect.width;
                        else if(p.rect.width < p.rect.height)
                            result.y *= p.rect.width / p.rect.height;
                    }

                    transform.anchorMax += result;
                    transform.sizeDelta = Vector2.zero;
                }
                else
                    transform.sizeDelta = new Vector2(size, size);
            }
        }

        void Anchor(RectTransform transform, string value)
        {
            var vals = value.Trim('(', ')').Split(',');
            float xVal;
            bool xIsRelative = GetValue(vals[0], out xVal);

            float yVal;
            bool yIsRelative = GetValue(vals[1], out yVal);

            if(xIsRelative && yIsRelative)
            {
                transform.anchorMax = transform.anchorMin = new Vector2(xVal, yVal);
            }
            else if(xIsRelative)
            {
                transform.anchorMax = transform.anchorMin = new Vector2(xVal, 0);
                transform.anchoredPosition = new Vector2(0, yVal);
            }
            else if(yIsRelative)
            {
                transform.anchorMax = transform.anchorMin = new Vector2(0, yVal);
                transform.anchoredPosition = new Vector2(xVal, 0);
            }
        }
    }
}
