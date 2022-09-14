using System.Collections.Concurrent;
using System.Diagnostics;

namespace Tracer.Core;

public class Tracer : ITracer
{
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
        var info = new MethodInfo(methodName, className, 0);

        _traceResult.GetOrAdd(threadId, _ => new RunningThreadInfo());
        _stopwatches.GetOrAdd(threadId, new Stack<Stopwatch>());
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
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        _stopwatches[threadId].Push(stopwatch);
    }

    public void StopTrace()
    {
        var threadId = Environment.CurrentManagedThreadId;
        _stopwatches[threadId].Peek().Stop();
        _traceResult[threadId].RunningMethods.Pop().Milliseconds = _stopwatches[threadId].Pop().ElapsedMilliseconds; 
    }

    public TraceResult GetTraceResult()
    {
        return new TraceResult(_traceResult.Select(info => new ThreadInfo(info.Value.Methods, info.Key)).ToList());
    }

    private class RunningThreadInfo
    {
        public List<MethodInfo> Methods { get; set; } = new();
        public Stack<MethodInfo> RunningMethods { get; set; } = new();
    }
    
    // TODO: remove one dictionary
    private readonly ConcurrentDictionary<int, RunningThreadInfo> _traceResult = new();
    private readonly ConcurrentDictionary<int, Stack<Stopwatch>> _stopwatches = new();
}