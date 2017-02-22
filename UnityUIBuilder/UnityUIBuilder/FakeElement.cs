using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Parsing.XML;

namespace UnityUIBuilder
{
    public class FakeElement : IXMLElement
    {
        public string name { get; private set; }

        public FakeElement(string name) { this.name = name; }

        public void AddAttribute(string name, string value)
        {
        }

        public IXMLElement AddElement(string name)
        {
            return new FakeElement(name);
        }

        public void SetValue(string value)
        {
        }
    }
}
