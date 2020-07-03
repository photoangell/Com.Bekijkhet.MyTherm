using System;
using System.Diagnostics;

public static class TimerComparison
{
    public static void Compare()
    {

        var x = new Stopwatch();
        // x.Start();
        // Thread.Sleep(1000);
        // x.Stop();
        // Console.WriteLine(x.ElapsedMilliseconds);
        // x.Reset();
        // x.Start();
        // Pi.Timing.SleepMilliseconds(1000);
        // x.Stop();
        // Console.WriteLine(x.ElapsedMilliseconds);

    if (Stopwatch.IsHighResolution)
    {
        Console.WriteLine("Operations timed using the system's high-resolution performance counter.");
    }
    else
    {
        Console.WriteLine("Operations timed using the DateTime class.");
    }

    long frequency = Stopwatch.Frequency;
    Console.WriteLine("  Timer frequency in ticks per second = {0}",
        frequency);
    long nanosecPerTick = (1000L*1000L*1000L) / frequency;
    Console.WriteLine("  Timer is accurate within {0} nanoseconds",
        nanosecPerTick);

        x.Reset();
        x.Start();
        WaitMicroseconds(40);
        x.Stop();
        Console.WriteLine(x.Elapsed);

        x.Reset();
        x.Start();
        SWWaitMicroseconds(40);
        x.Stop();
        Console.WriteLine(x.Elapsed);

        long timeTotal = 0;
        for (var y = 0; y < 10000; y++ ) {
            x.Reset();
            x.Start();
            SWWaitMicroseconds(40);
            x.Stop();
            timeTotal += x.ElapsedTicks;
        }
        Console.WriteLine("average microseconds time: " + (timeTotal / 10000 / 100));
        

    }
    private static void WaitMicroseconds(int microseconds)
    {
        var until = DateTime.UtcNow.Ticks + (microseconds * 10) - 20;
        while (DateTime.UtcNow.Ticks < until) { }
    }

    private static void SWWaitMicroseconds(int microseconds)
    {
        var durationTicks = (long)Math.Round((decimal)((Stopwatch.Frequency / 1000000)* microseconds));
        var sw = Stopwatch.StartNew();

        while (sw.ElapsedTicks < durationTicks - 1600)
        {

        }
    }
}