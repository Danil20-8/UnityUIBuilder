using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRLib.Parsing.XML;

namespace UnityUIBuilder
{
    /// <summary>
    /// if none of handlers cannot handle an element return it instead null reference to keep aplication alive or use BombElement to stop work and throw exception
    /// </summary>
    public class FakeElement : IXMLElement
    {
        public string name { get; private set; }

        public FakeElement(string name) { this.name = name; }

        public virtual void AddAttribute(string name, string value)
        {
        }

        public virtual IXMLElement AddElement(string name)
        {
            return new FakeElement(name);
        }

        public virtual void SetValue(string value)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is FakeElement;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    /// <summary>
    /// Doesn't let application be working if all of handlers cannot handle an element
    /// </summary>
    public class BombElement : FakeElement
    {
        public string message { get { return _message; } set { _message = value; } }
        string _message = string.Empty;

        string parentName;

        public BombElement(string name, string parentName) : base(name)
        {
            this.parentName = parentName;
        }

        public override IXMLElement AddElement(string notUsedName)
        {
            Detonate();
            return null; // just for compile. It never comes here.
        }
        public override void AddAttribute(string notUsedName, string notUsedValue)
        {
            Detonate();
        }
        public override void SetValue(string value)
        {
            Detonate();
        }

        public void Detonate()
        {
            throw new AddElementException(this.name, parentName, message);
        }
    }
}
