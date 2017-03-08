using UnityUIBuilder.Standard.Elements;
using UnityUIBuilder.Standard.Attributes;
using DRLib.Parsing.XML;
using System;

namespace UnityUIBuilder.Standard
{
    public class AddElementHandler : IAddElementHandler<AppData, ModuleData, ElementData>
    {
        AddElementCase<AppData, ModuleData, ElementData> addElementHandler;
        public AddElementHandler()
        {
            addElementHandler = new AddElementCase<AppData, ModuleData, ElementData>(
                    new VListElementHandler<AppData, ModuleData, ElementData>(
                        new AddRootState<AppData, ModuleData, ElementData>(),

                        new AddElementFromConst<AppData, ModuleData, ElementData>(),
                        new AddElementFromUnityRes<AppData, ModuleData, ElementData>(),
                        new AddElementFromAssemblies<AppData, ModuleData, ElementData>()
                    ),
                    new VListElementHandler<AppData, ModuleData, ElementData>(
                        new AddElementFromConst<AppData, ModuleData, ElementData>(),
                        new AddElementFromUnityRes<AppData, ModuleData, ElementData>(),
                        new AddElementFromAssemblies<AppData, ModuleData, ElementData>()
                        )
                );
        }

        public IXMLElement AddElement(string name, XMLElementUI<AppData, ModuleData, ElementData> previewElement)
        {
            var result = addElementHandler.AddElement(name, previewElement);
            var bomb = result as BombElement;
            if(bomb != null)
            {
                bomb.message = "Ensure you added required using namespaces or folders. If you need an empty gameObject use \"void\" element";
                return bomb;
            }
            return result;
        }
    }

    public class AddAttributeHandler : VListAttributeHandler<AppData, ModuleData, ElementData>
    {
        public AddAttributeHandler()
            : base(
                    new AttributesForUI<AppData, ModuleData, ElementData>(),
                    new ConstStatementAttribute<AppData, ModuleData, ElementData>(),
                    new SetPropertyAttribute<AppData, ModuleData, ElementData>()
                  )
        {
        }
    }
}
