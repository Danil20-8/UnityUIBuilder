using System;

namespace UnityUIBuilder
{
    public class SetAttributeException : Exception
    {

        public SetAttributeException(string attributeName, string value, string elementName)
            :base(MakeString(attributeName, value, elementName, string.Empty))
        {
        }

        public SetAttributeException(string attributeName, string value, string elementName, string message)
            : base(MakeString(attributeName, value, elementName, message))
        {
        }

        public SetAttributeException(string attributeName, string value, string elementName, Exception innerException)
            :base(MakeString(attributeName, value, elementName, string.Empty), innerException)
        {
        }

        public SetAttributeException(string attributeName, string value, string elementName, string message, Exception innerException)
            : base(MakeString(attributeName, value, elementName, message), innerException)
        {
        }

        static string MakeString(string attributeName, string value, string elementName, string message)
        {
            return string.Format("Cannot set {0} attribute with value {1} in {2} element.", attributeName, value, elementName) + (message != string.Empty ? " " + message + "." : message);
        }
    }

    public class AddElementException : Exception
    {
        public AddElementException(string elementName, string parentElemenentName)
            :base(MakeString(elementName, parentElemenentName, string.Empty))
        {
        }

        public AddElementException(string elementName, string parentElemenentName, Exception innerException)
            : base(MakeString(elementName, parentElemenentName, string.Empty), innerException)
        {
        }

        public AddElementException(string elementName, string parentElemenentName, string message)
            : base(MakeString(elementName, parentElemenentName, message))
        {
        }

        public AddElementException(string elementName, string parentElemenentName, string message, Exception innerException)
            : base(MakeString(elementName, parentElemenentName, message), innerException)
        {
        }

        static string MakeString(string elementName, string parentElementName, string message)
        {
            return string.Format("Cannot add {0} to {1}.", elementName, parentElementName) + (message != string.Empty ? " " + message + "." : message);
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
