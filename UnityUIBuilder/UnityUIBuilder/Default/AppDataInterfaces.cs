using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Default
{
    public interface IDefaultAppData<TAppData> : IIDData
    {
    }

    public interface IIDData
    {
        GameObject GetObjectByID(string id);
        void AddIDObject(string id, GameObject gameObjetc);
    }
}
