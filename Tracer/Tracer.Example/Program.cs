using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Example
{
    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }

        public void MyMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            _bar.InnerMethod();
            PrivateMethod();
            _tracer.StopTrace();
        }

        private void PrivateMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(105);
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            _tracer.StopTrace();
        }
    }

    public static class Program
    {
        private static void Main(string[] args)
        {
            var assembly = Assembly.LoadFrom("TraceResultSerializers/Tracer.Serialization.Json.dll");
            var type = assembly.GetType("Tracer.Serialization.Json.JsonSerializer");

            var tracer = new Tracer.Core.Tracer();
            var foo = new Foo(tracer);
            var task = Task.Run(() => foo.MyMethod());
            foo.MyMethod();
            foo.MyMethod();
            task.Wait();
            var result = tracer.GetTraceResult();

            if (Activator.CreateInstance(type) is ITraceResultSerializer serializer)
            {
                using (var fstream = new FileStream("result.json", FileMode.Create))
                {
                    serializer.Serialize(result, fstream);
                }
            }
        }
    }
}