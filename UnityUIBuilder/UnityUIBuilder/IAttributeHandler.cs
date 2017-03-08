using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityUIBuilder
{
    public interface IAddAttributeHandler<TAppData, TModuleData, TElementData>
    {
        AddResult AddAttribute(string attributeName, string attributeValue, XMLElementUI<TAppData, TModuleData, TElementData> element);
    }

    public struct AddResult
    {
        public bool ok { get { return state == State.OK; } }
        public bool ignored { get { return state == State.Ignored; } }
        public bool error { get { return state == State.Error; } }

        readonly State state;

        public string message { get { return _message != string.Empty ? stateString + ". " + _message : stateString; } set { _message = value; } }
        string _message;

        public string stateString
        {
            get
            {
                switch(state)
                {
                    case State.OK:
                        return "OK";
                    case State.Error:
                        return "Value error";
                    case State.Ignored:
                        return "None handler can handle the attribute";
                    default:
                        return string.Empty;
                }
            }
        }

        public AddResult(State state)
        {
            this.state = state;
            _message = string.Empty;
        }

        public static implicit operator AddResult(State state)
        {
            return new AddResult(state);
        }

        public static AddResult operator |(AddResult lvalue, AddResult rvalue)
        {
            return lvalue.state < rvalue.state ? lvalue.state : rvalue.state;
        }

        public enum State : byte
        {
            OK,
            Error,
            Ignored,
        }
    }
}
