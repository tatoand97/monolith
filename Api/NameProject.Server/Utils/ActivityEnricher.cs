using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace NameProject.Server.Utils;

public class ActivityEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory factory)
    {
        var activity = Activity.Current;
        if (activity is null) return;

        logEvent.AddPropertyIfAbsent(factory.CreateProperty("trace_id", activity.TraceId.ToString()));
        logEvent.AddPropertyIfAbsent(factory.CreateProperty("span_id", activity.SpanId.ToString()));
    }
}