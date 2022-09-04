using Tracer.Core;

namespace Tracer.Serialization.Abstractions;

public interface ITraceResultSerializer
{
    void Serialize(TraceResult traceResult, Stream to);
}