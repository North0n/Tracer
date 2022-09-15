using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Xml;

public class XmlSerializer : ITraceResultSerializer
{
    public void Serialize(Core.TraceResult traceResult, Stream to)
    {
        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(TraceResult));
        xmlSerializer.Serialize(to, new TraceResult(traceResult));
    }

    public string Extension { get; } = "Xml";
}