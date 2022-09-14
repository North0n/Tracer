namespace Tracer.Core;

public class MethodInfo
{
    public MethodInfo(string name, string className, int ms)
    {
        Name = name;
        Class = className;
        Milliseconds = ms;
    }

    public string Name { get; }
    public string Class { get; }
    // TODO: Store just long Milliseconds and do this transformation in the serializer class
    public long Milliseconds { get; internal set; }
    // TODO: Make IReadOnlyList
    public List<MethodInfo> Methods { get; } = new();
}
