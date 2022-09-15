namespace Tracer.Serialization.Yaml;

public class MethodInfo
{
    public MethodInfo(Core.MethodInfo methodInfo)
    {
        Class = methodInfo.Class;
        Name = methodInfo.Name;
        Time = $"{methodInfo.Milliseconds}ms";
        Methods = new List<MethodInfo>(methodInfo.Methods.Select(m => new MethodInfo(m)));
    }
    
    public string Name { get; }
    public string Class { get; }
    public string Time { get; }
    public IReadOnlyList<MethodInfo> Methods { get; }
}