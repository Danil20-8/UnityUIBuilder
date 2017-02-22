using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Standard
{
    public class AppData : IStandardAppData<AppData>
    {
        readonly Dictionary<string, GameObject> idList = new Dictionary<string, GameObject>();

        public void AddIDObject(string id, GameObject gameObject)
        {
            idList.Add(id, gameObject);
        }

        public GameObject GetObjectByID(string id)
        {
            return idList[id];
        }
    }
}
