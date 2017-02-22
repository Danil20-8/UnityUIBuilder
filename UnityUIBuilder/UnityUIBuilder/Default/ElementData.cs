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

        public MonoBehaviour GetController()
        {
            return controller;
        }

        public void ImporData(ElementData sourceData)
        {
            controller = sourceData.controller;
        }

        public void SetController(MonoBehaviour controller)
        {
            this.controller = controller;
        }
    }
}
