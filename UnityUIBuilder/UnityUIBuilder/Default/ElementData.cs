using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Default
{
    public class ElementData : IDefaultElementData<ElementData>
    {
        MonoBehaviour controller;
        GameObject gameObject;

        public ElementData(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public ElementData CreateChild(string name)
        {
            return CreateChild(new GameObject(name));
        }

        public ElementData CreateChild(GameObject gameObject)
        {
            ResetGameObject(gameObject);

            return new ElementData(gameObject)
            {
                controller = controller
            };

        }

        protected virtual void ResetGameObject(GameObject gameObject)
        {
            RectTransform rt = gameObject.GetComponent<RectTransform>();
            if (rt == null)
                rt = gameObject.AddComponent<RectTransform>();

            rt.SetParent(this.gameObject.transform);

            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.zero;
            rt.anchoredPosition = Vector3.zero;
        }

        public MonoBehaviour GetController()
        {
            return controller;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public Transform GetTransform()
        {
            return gameObject.transform;
        }

        public void SetController(MonoBehaviour controller)
        {
            this.controller = controller;
        }

        public ElementData Clone()
        {
            return new ElementData(gameObject)
            {
                controller = controller
            };
        }

        public static implicit operator ElementData(GameObject gameObject)
        {
            return new ElementData(gameObject);
        }
    }
}
