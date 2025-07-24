using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace NameProject.Server.Utils;

/// <summary>
/// A Serilog enricher that attaches information from the current <see cref="Activity"/> to log events.
/// </summary>
/// <remarks>
/// Adds properties such as trace ID and span ID from the current <see cref="System.Diagnostics.Activity"/>
/// to the log events if an activity is active.
/// </remarks>
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