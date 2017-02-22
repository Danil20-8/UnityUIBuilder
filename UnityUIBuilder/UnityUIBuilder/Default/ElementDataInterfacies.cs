using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Default
{
    public interface IDefaultElementData<TData> : IControllerData, IDataImport<TData>
    {
    }

    public interface IControllerData
    {
        MonoBehaviour GetController();
        void SetController(MonoBehaviour controller);
    }
}
