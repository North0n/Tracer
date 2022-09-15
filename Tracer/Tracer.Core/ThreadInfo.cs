namespace Tracer.Core;

public class ThreadInfo
{
    public ThreadInfo(IReadOnlyList<MethodInfo> methods, int id)
    {
        Methods = methods;
        Milliseconds = methods.Sum(method => method.Milliseconds);
        Id = id;
    }
    
    public int Id { get; }
    public long Milliseconds { get; }
    public IReadOnlyList<MethodInfo> Methods { get; }
}