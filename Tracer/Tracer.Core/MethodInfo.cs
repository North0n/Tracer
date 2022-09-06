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
    internal long Milliseconds { get; set; }
    // TODO: Store just long Milliseconds and do this transformation in the serializer class
    public string Time => $"{Milliseconds}ms";
    // TODO: Make IReadOnlyList
    public List<MethodInfo> Methods { get; } = new();
}
