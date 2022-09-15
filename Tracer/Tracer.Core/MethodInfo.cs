namespace Tracer.Core;

public class MethodInfo
{
    public MethodInfo(string name, string className, long ms, IReadOnlyList<MethodInfo> methods)
    {
        Name = name;
        Class = className;
        Milliseconds = ms;
        Methods = methods;
    }

    public string Name { get; }
    public string Class { get; }
    public long Milliseconds { get; }
    public IReadOnlyList<MethodInfo> Methods { get; }
}
