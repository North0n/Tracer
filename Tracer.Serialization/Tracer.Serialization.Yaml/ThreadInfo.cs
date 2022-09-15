namespace Tracer.Serialization.Yaml;

public class ThreadInfo
{
    public ThreadInfo(Core.ThreadInfo threadInfo)
    {
        Id = threadInfo.Id;
        Time = $"{threadInfo.Milliseconds}ms";
        Methods = new List<MethodInfo>(threadInfo.Methods.Select(m => new MethodInfo(m)));
    }
    
    public int Id { get; }
    public string Time { get; }
    public IReadOnlyList<MethodInfo> Methods { get; }
}