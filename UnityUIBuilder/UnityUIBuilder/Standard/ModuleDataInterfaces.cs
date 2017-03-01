using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Standard
{
    public interface IStandardTModuleData<TModuleData> : IModuleVersionData, IDataImport<TModuleData>, IResFoldersData, INamespaceData, IClassData, IIDData
    {
    }

    public interface IIDData
    {
        GameObject GetObjectByID(string id);
        void AddIDObject(string id, GameObject gameObjetc);
        void WaitForID(string id, Action<GameObject> func);
    }

    public interface IModuleVersionData
    {
        void SetVersion(string verison);
        string GetVersion();
    }

    public interface IDataImport<TModuleData>
    {
        void ImportData(TModuleData sourceData);
    }

    public interface IResFoldersData
    {
        void AddResFolder(string folder);
        IEnumerable<string> GetResFolders();
    }

    public interface INamespaceData
    {
        void AddNamespace(string name);
        IEnumerable<string> GetNamespaces();
    }

    public interface IClassData
    {
        void AddClass(string className, IEnumerable<ClassAttribute> attributes);
        void AddClassAttribute(string className, ClassAttribute attribute);
        IEnumerable<ClassAttribute> GetClassAttributes(string className);
        IEnumerable<Class> GetClasses();
    }

    public struct Class
    {
        public string name;
        public IEnumerable<ClassAttribute> attributes;
    }

    public struct ClassAttribute
    {
        public string name;
        public string value;
    }
}
