using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityUIBuilder
{
    public class SetAttributeException : Exception
    {

        public SetAttributeException(string attributeName, string value, string elementName)
            :base(MakeString(attributeName, value, elementName))
        {
        }

        public SetAttributeException(string attributeName, string value, string elementName, string message)
            : base(MakeString(attributeName, value, elementName) + ". " + message)
        {
        }

        public SetAttributeException(string attributeName, string value, string elementName, Exception innerException)
            :base(MakeString(attributeName, value, elementName), innerException)
        {
        }

        public SetAttributeException(string attributeName, string value, string elementName, string message, Exception innerException)
            : base(MakeString(attributeName, value, elementName) + ". " + message, innerException)
        {
        }

        static string MakeString(string attributeName, string value, string elementName)
        {
            return "Cannot set to " + attributeName + " attribute with value " + value + " in " + elementName + " element.";
        }
    }

    public class AddElementException : Exception
    {
        public AddElementException(string elementName, string parentElemenentName)
            :base(MakeString(elementName, parentElemenentName))
        {
        }

        public AddElementException(string elementName, string parentElemenentName, Exception innerException)
            : base(MakeString(elementName, parentElemenentName), innerException)
        {
        }

        public AddElementException(string elementName, string parentElemenentName, string message)
            : base(MakeString(elementName, parentElemenentName) + " " + message + ".")
        {
        }

        public AddElementException(string elementName, string parentElemenentName, string message, Exception innerException)
            : base(MakeString(elementName, parentElemenentName) + " " + message + ".", innerException)
        {
        }

        static string MakeString(string elementName, string parentElementName)
        {
            return "Cannot add " + elementName + " to " + parentElementName + ".";
        }
    }

    public class XMLParseException : Exception
    {
        public XMLParseException(string message, Exception e)
            :base(message, e)
        {

        }
    }
}
