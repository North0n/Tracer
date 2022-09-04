namespace Tracer.Core;

public class TraceResult
{
    public TraceResult(IReadOnlyList<ThreadInfo> methodInfos)
    {
        Threads = methodInfos;
    }
    public IReadOnlyList<ThreadInfo> Threads { get; }
}