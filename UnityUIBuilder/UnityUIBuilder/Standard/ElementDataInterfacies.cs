using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Standard
{
    public interface IStandardElementData<TData> : IControllerData, ITransformData, IGameObjectData, ICreateChildData<TData>, ICloneData<TData>
    {
    }

    public interface IControllerData
    {
        MonoBehaviour GetController();
        void SetController(MonoBehaviour controller);
    }

    public interface ITransformData
    {
        Transform GetTransform();
    }
    public interface IGameObjectData
    {
        GameObject GetGameObject();
    }
    public interface ICreateChildData<TData>
    {
        TData CreateChild(string name);
        TData CreateChild(GameObject gameObject);
    }
    public interface ICloneData<TData>
    {
        TData Clone();
    }
}
namespace UnityUIBuilder
{
    public interface ISetPreviewable<TData>
    {
        void SetPreview(TData data);
    }
}
