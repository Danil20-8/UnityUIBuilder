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

        string message { get { return _message != null ? message : "Ensure you added required using namespaces or folders. If you need an empty gameObject use \"void\" element."; } }
        string _message;

        string parentName;

        public BombElement(string name, string parentName, string message = null) : base(name) { this.parentName = parentName; _message = message; }

        public override IXMLElement AddElement(string notUsedName)
        {
            throw new AddElementException(this.name, parentName, message);
        }
        public override void AddAttribute(string notUsedName, string notUsedValue)
        {
            throw new AddElementException(this.name, parentName, message);
        }
        public override void SetValue(string value)
        {
            throw new AddElementException(this.name, parentName, message);
        }
    }
}
