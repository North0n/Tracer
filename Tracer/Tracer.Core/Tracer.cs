using System.Collections.Concurrent;
using System.Diagnostics;

namespace Tracer.Core;

public class Tracer : ITracer
{
    private class MethodTrace
    {
        public MethodTrace(string name, string className)
        {
            Name = name;
            Class = className;
        }

        public string Name { get; }
        public string Class { get; }
        public Stopwatch Stopwatch { get; } = new();
        public List<MethodTrace> Methods { get; } = new();
    }

    public void StartTrace()
    {
        var threadId = Environment.CurrentManagedThreadId;
        var stackTrace = new StackTrace();
        
        // Collect method info
        // GetFrame(0) - this method
        // GetFrame(1) - method to measure
        var method = stackTrace.GetFrame(1)!.GetMethod();
        var methodName = method!.Name;
        var className = method.DeclaringType!.Name;
        var info = new MethodTrace(methodName, className);

        _traceResult.GetOrAdd(threadId, _ => new RunningThreadInfo());
        // Place method info into right place
        if (_traceResult[threadId].RunningMethods.Count == 0)
        {
            _traceResult[threadId].Methods.Add(info);
        }
        else
        {
            _traceResult[threadId].RunningMethods.Peek().Methods.Add(info);
        }
        _traceResult[threadId].RunningMethods.Push(info);

        // Start time measurement
        _traceResult[threadId].RunningMethods.Peek().Stopwatch.Start();
    }

    public void StopTrace()
    {
        var threadId = Environment.CurrentManagedThreadId;
        _traceResult[threadId].RunningMethods.Pop().Stopwatch.Stop();
    }

    public TraceResult GetTraceResult()
    {
        return new TraceResult(_traceResult.Select(info =>
            new ThreadInfo(
                info.Value.Methods.Select(m => new MethodInfo(m.Name, m.Class, m.Stopwatch.ElapsedMilliseconds))
                    .ToList(), info.Key)).ToList());
    }

    private class RunningThreadInfo
    {
        public List<MethodTrace> Methods { get; set; } = new();
        public Stack<MethodTrace> RunningMethods { get; set; } = new();
    }
    
    private readonly ConcurrentDictionary<int, RunningThreadInfo> _traceResult = new();
}