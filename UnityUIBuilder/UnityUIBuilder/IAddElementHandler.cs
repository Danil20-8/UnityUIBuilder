using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;
using UnityEngine;

namespace UnityUIBuilder
{
    public interface IAddElementHandler<TAppData, TModelData>
    {
        IXMLElement AddElement(string name, Transform parent, MonoBehaviour controller, XMLModule<TAppData, TModelData>.External provider);
    }
}
