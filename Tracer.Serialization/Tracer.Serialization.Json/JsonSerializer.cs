using System.Text;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Json;

public class JsonSerializer : ITraceResultSerializer
{
    public void Serialize(TraceResult traceResult, Stream to)
    {
        var res = System.Text.Json.JsonSerializer.Serialize<TraceResult>(traceResult);
        to.Write(Encoding.Default.GetBytes(res));
    }
}