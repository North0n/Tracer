using System.Reflection;
using Tracer.Core;
using Tracer.Serialization.Abstractions;
using static System.Threading.Thread;

namespace Tracer.Example
{
    public class Foo
    {
        private readonly Bar _bar;
        private readonly ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }

        public void MyMethod()
        {
            _tracer.StartTrace();
            Sleep(100);
            _bar.InnerMethod();
            PrivateMethod();
            _tracer.StopTrace();
        }

        private void PrivateMethod()
        {
            _tracer.StartTrace();
            Sleep(105);
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private readonly ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            Sleep(200);
            _tracer.StopTrace();
        }
    }

    public static class Program
    {
        private static void Main()
        {
            var tracer = new Tracer.Core.Tracer();
            var foo = new Foo(tracer);
            var task = Task.Run(() => foo.MyMethod());
            foo.MyMethod();
            foo.MyMethod();
            task.Wait();
            var result = tracer.GetTraceResult();

            // TODO: replace with default foreach
            Directory.EnumerateFiles("TraceResultSerializers", "*.dll").ToList().ForEach(file =>
            {
                var serializerAssembly = Assembly.LoadFrom(file);
                // Get file name without extension
                file = file.Substring(file.LastIndexOf('\\') + 1, file.Length - file.LastIndexOf('\\') - 1);
                file = file[..file.LastIndexOf('.')];
                
                var extension = file.AsSpan(file.LastIndexOf('.') + 1, file.Length - file.LastIndexOf('.') - 1);
                var serializerName = $"{file}.{extension}Serializer";
                // TODO: replace with GetTypes. And among this types find those, which implement ITraceResultSerializer
                var serializerType = serializerAssembly.GetType(serializerName);
                if (serializerType == null)
                {
                    throw new Exception($"Serializer {serializerName} not found");
                }
                var serializer = (ITraceResultSerializer?)Activator.CreateInstance(serializerType);
                if (serializer == null)
                {
                    throw new Exception($"Serializer {serializerName} not created");
                }
                using var fileStream = new FileStream($"result.{extension}", FileMode.Create);
                serializer.Serialize(result, fileStream);
            });
        }
    }
}