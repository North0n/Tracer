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
    public long Milliseconds { get; }
    public IReadOnlyList<MethodInfo> Methods { get; } = new List<MethodInfo>();
}
