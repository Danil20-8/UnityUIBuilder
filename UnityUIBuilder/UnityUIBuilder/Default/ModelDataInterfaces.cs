using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityUIBuilder.Default
{
    public interface IDefaultTModelData<TModelData> : IDataImport<TModelData>, IResFoldersData, INamespaceData, IClassData
    {
    }

    public interface IDataImport<TModelData>
    {
        void ImportData(TModelData sourceData);
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
