using System.Xml.Schema;
using System.Xml.Serialization;

namespace Tracer.Serialization.Xml;

[XmlRoot(ElementName = "Thread")]
public class ThreadInfo
{
    public ThreadInfo()
    {
    }
    public ThreadInfo(Core.ThreadInfo threadInfo)
    {
        Id = threadInfo.Id;
        Time = $"{threadInfo.Milliseconds}ms";
        Methods = new List<MethodInfo>(threadInfo.Methods.Select(m => new MethodInfo(m)));
    }

    [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
    public int Id { get; set; } = -1;

    [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
    public string Time { get; set; } = "";

    [XmlArray()]
    public List<MethodInfo> Methods { get; } = new();
}