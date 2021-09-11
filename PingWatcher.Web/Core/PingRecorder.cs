using System;
using System.Threading.Tasks;

public class PingRecorder
{
    public static DateTime DefaultLastPing = DateTime.MinValue.ToUniversalTime();

    static DateTime _lastPing = DateTime.MinValue.ToUniversalTime();

    static object _lock = new object();

    public PingRecorder()
    {
        
    }

    public Task Ping()
    {
        lock(_lock)
        {
            _lastPing = DateTime.UtcNow;
        }

        return Task.CompletedTask;
    }

    public Task<DateTime> GetLastPingDateTime()
    {
        return Task.FromResult(new DateTime(_lastPing.Ticks, DateTimeKind.Utc));
    }
}