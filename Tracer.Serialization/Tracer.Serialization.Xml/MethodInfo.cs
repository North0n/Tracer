using System.Xml.Schema;
using System.Xml.Serialization;

namespace Tracer.Serialization.Xml;

[XmlRoot(ElementName = "Method")]
public class MethodInfo
{
    public MethodInfo()
    {
    }

    public MethodInfo(Core.MethodInfo methodInfo)
    {
        Class = methodInfo.Class;
        Name = methodInfo.Name;
        Time = $"{methodInfo.Milliseconds}ms";
        Methods = new List<MethodInfo>(methodInfo.Methods.Select(m => new MethodInfo(m)));
    }
    
    [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
    public string Name { get; set; } = "";

    [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
    public string Class { get; set; } = "";

    [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
    public string Time { get; set; } = "";
    public List<MethodInfo> Methods { get; } = new();
}