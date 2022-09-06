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
    internal long Milliseconds { get; }
    // TODO: Store just long Milliseconds and do this transformation in the serializer class
    public string Time => $"{Milliseconds}ms";
    public IReadOnlyList<MethodInfo> Methods { get; }
}