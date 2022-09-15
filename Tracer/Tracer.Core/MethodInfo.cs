namespace Tracer.Core;

public class MethodInfo
{
    public MethodInfo(string name, string className, long ms)
    {
        Name = name;
        Class = className;
        Milliseconds = ms;
    }

    public string Name { get; }
    public string Class { get; }
    // TODO: Store just long Milliseconds and do this transformation in the serializer class
    public long Milliseconds { get; }
    public IReadOnlyList<MethodInfo> Methods { get; } = new List<MethodInfo>();
}
