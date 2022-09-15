namespace Tracer.Core.Tests;

public class Tests
{
    internal class Foo
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
            Task.Run(() => _bar.InnerMethod()).Wait();
            _tracer.StopTrace();
        }

        public void MethodWithNoNested()
        {
            _tracer.StartTrace();
            Thread.Sleep(105);
            _tracer.StopTrace();
        }
    }

    internal class Bar
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
            PrivateMethod();
            _tracer.StopTrace();
        }

        private void PrivateMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            _tracer.StopTrace();
        }
    }

    private static int GetMillisecondsValue(string ms)
    {
        if (!int.TryParse(ms[..^2], out var value))
        {
            throw new Exception($"Invalid milliseconds value: {ms}");
        }

        return value;
    }
    
    [Test]
    public void SingleThreadTest()
    {
        var tracer = new Tracer();
        var boo = new Bar(tracer);
        boo.InnerMethod();
        var result = tracer.GetTraceResult();
        
        Assert.That(result.Threads, Has.Count.EqualTo(1));
        Assert.That(result.Threads[0].Methods, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result.Threads[0].Methods[0].Name, Is.EqualTo("InnerMethod"));
            Assert.That(result.Threads[0].Methods[0].Class, Is.EqualTo("Bar"));
            Assert.That(result.Threads[0].Methods[0].Milliseconds, Is.GreaterThanOrEqualTo(400));
        });
        Assert.That(result.Threads[0].Methods[0].Methods, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result.Threads[0].Methods[0].Methods[0].Name, Is.EqualTo("PrivateMethod"));
            Assert.That(result.Threads[0].Methods[0].Methods[0].Class, Is.EqualTo("Bar"));
            Assert.That(result.Threads[0].Methods[0].Methods[0].Milliseconds, Is.GreaterThanOrEqualTo(200));
        });
        Assert.That(result.Threads[0].Methods[0].Methods[0].Methods, Has.Count.EqualTo(0));
    }

    [Test]
    public void MultiThreadedTest()
    {
        var tracer = new Tracer();
        var foo = new Foo(tracer);
        foo.MethodWithNoNested();
        foo.MyMethod();
        var result = tracer.GetTraceResult();
        
        Assert.That(result.Threads, Has.Count.EqualTo(2));
        Assert.That(result.Threads[0].Methods, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result.Threads[0].Methods[0].Name, Is.EqualTo("InnerMethod"));
            Assert.That(result.Threads[0].Methods[0].Class, Is.EqualTo("Bar"));
            Assert.That(result.Threads[0].Methods[0].Milliseconds, Is.GreaterThanOrEqualTo(400));
        });
        Assert.That(result.Threads[0].Methods[0].Methods, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result.Threads[0].Methods[0].Methods[0].Name, Is.EqualTo("PrivateMethod"));
            Assert.That(result.Threads[0].Methods[0].Methods[0].Class, Is.EqualTo("Bar"));
            Assert.That(result.Threads[0].Methods[0].Methods[0].Milliseconds, Is.GreaterThanOrEqualTo(200));
        });
        Assert.That(result.Threads[0].Methods[0].Methods[0].Methods, Has.Count.EqualTo(0));
        
        Assert.That(result.Threads[1].Methods, Has.Count.EqualTo(2));
        Assert.That(result.Threads[1].Methods[0].Methods, Has.Count.EqualTo(0));
        Assert.Multiple(() =>
        {
            Assert.That(result.Threads[1].Methods[0].Name, Is.EqualTo("MethodWithNoNested"));
            Assert.That(result.Threads[1].Methods[0].Class, Is.EqualTo("Foo"));
            Assert.That(result.Threads[1].Methods[0].Milliseconds, Is.GreaterThanOrEqualTo(105));
        });
        
        Assert.That(result.Threads[1].Methods[1].Methods, Has.Count.EqualTo(0));
        Assert.Multiple(() =>
        {
            Assert.That(result.Threads[1].Methods[1].Name, Is.EqualTo("MyMethod"));
            Assert.That(result.Threads[1].Methods[1].Class, Is.EqualTo("Foo"));
            Assert.That(result.Threads[1].Methods[1].Milliseconds, Is.GreaterThanOrEqualTo(300));
        });
    }
}