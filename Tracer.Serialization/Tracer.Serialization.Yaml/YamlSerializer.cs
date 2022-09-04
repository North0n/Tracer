using System.Text;
using Tracer.Core;
using Tracer.Serialization.Abstractions;
using YamlDotNet.Serialization;

namespace Tracer.Serialization.Yaml;

public class YamlSerializer : ITraceResultSerializer
{
    public void Serialize(TraceResult traceResult, Stream to)
    {
        var serializer = new SerializerBuilder().DisableAliases().Build();
        var result = serializer.Serialize(traceResult);
        to.Write(Encoding.UTF8.GetBytes(result));
    }
}