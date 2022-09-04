namespace Tracer.Core;

public interface ITracer
{
    void StartTrace();

    void StopTrace();
    
    TraceResult GetTraceResult();
}