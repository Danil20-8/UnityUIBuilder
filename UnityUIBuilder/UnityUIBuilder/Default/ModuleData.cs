using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder.Default
{
    public class ModuleData : IDefaultTModelData<ModuleData>
    {
        readonly HashSet<string> namespaces = new HashSet<string>();
        readonly HashSet<string> resFolders = new HashSet<string>();
        readonly Dictionary<string, List<ClassAttribute>> classes = new Dictionary<string, List<ClassAttribute>>();

        readonly Dictionary<string, GameObject> idList = new Dictionary<string, GameObject>();

        public void AddIDElement(string id, GameObject obj)
        {
            if (!idList.ContainsKey(id))
                idList.Add(id, obj);
            else
                throw new Exception(idList[id] + " already uses id: " + id + " set another id to " + obj);
        }

        public GameObject GetByID(string id)
        {
            GameObject result;
            if (idList.TryGetValue(id, out result))
                return result;
            return null;
        }

        public void AddResFolder(string folder)
        {
            resFolders.Add(folder);
        }
        public IEnumerable<string> GetResFolders()
        {
            return resFolders;
        }

        public void AddNamespace(string name)
        {
            namespaces.Add(name);
        }
        public IEnumerable<string> GetNamespaces()
        {
            return namespaces;
        }

        public void AddClass(string className, IEnumerable<ClassAttribute> attributes)
        {
            if(classes.ContainsKey(className))
                throw new Exception(className + " already added");
            classes.Add(className, attributes.ToList());
        }

        public void AddClassAttribute(string className, ClassAttribute attribute)
        {
            List<ClassAttribute> cl;
            if (classes.TryGetValue(className, out cl))
                cl.Add(attribute);
            else
                throw new Exception(className + " is not found");
        }

        public IEnumerable<ClassAttribute> GetClassAttributes(string className)
        {
            List<ClassAttribute> result;
            if (classes.TryGetValue(className, out result))
            {
                return result;
            }

            throw new Exception(className + " is not found");
        }

        public IEnumerable<Class> GetClasses()
        {
            return classes.Select(p => new Class { name = p.Key, attributes = p.Value });
        }

        public void ImporData(ModuleData sourceData)
        {
            foreach (var n in sourceData.namespaces)
                namespaces.Add(n);

            foreach (var f in sourceData.resFolders)
                resFolders.Add(f);

            foreach (var c in sourceData.classes)
                if (!classes.ContainsKey(c.Key))
                    classes.Add(c.Key, c.Value.ToList());
        }
    }
}
