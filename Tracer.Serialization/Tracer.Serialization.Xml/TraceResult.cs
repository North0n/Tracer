using System.Xml.Serialization;

namespace Tracer.Serialization.Xml;

[XmlRoot(ElementName = "Root")]
public class TraceResult 
{
    public TraceResult()
    {
    }
    public TraceResult(Core.TraceResult traceResult)
    {
        Threads = new List<ThreadInfo>(traceResult.Threads.Select(m => new ThreadInfo(m)));
    }
    
    public List<ThreadInfo> Threads { get; } = new();
}