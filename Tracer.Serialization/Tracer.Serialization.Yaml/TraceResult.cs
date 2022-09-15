namespace Tracer.Serialization.Yaml;

public class TraceResult 
{
    public TraceResult(Core.TraceResult traceResult)
    {
        Threads = new List<ThreadInfo>(traceResult.Threads.Select(m => new ThreadInfo(m)));
    }
    
    public IReadOnlyList<ThreadInfo> Threads { get; }
}