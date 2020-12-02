using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Tracing;
using Coderr.Client.Contracts;

namespace Coderr.Client.AspNet.WebApi.Integrations
{
    /// <summary>
    ///     Stores log entries so that Coderr can attach them to errors.
    /// </summary>
    public class CoderrTracer : ITraceWriter
    {
        internal static CoderrTracer Instance = new CoderrTracer();

        private readonly LinkedList<LogEntryDto> LatestLogEntries = new LinkedList<LogEntryDto>();

        /// <summary>
        ///     Max age for log entries that will be included in the error report.
        /// </summary>
        /// <remarks>
        ///     Default is five minutes.
        /// </remarks>
        public TimeSpan MaxAge { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        ///     Max amount of entries that may be included in an error report.
        /// </summary>
        /// <remarks>
        ///     Default is 100.
        /// </remarks>
        public int MaxEntries { get; set; } = 100;

        /// <summary>
        ///     Minimum level of log entries to attach.
        /// </summary>
        /// <remarks>
        ///     Default is <see cref="TraceLevel.Debug" />.
        /// </remarks>
        public TraceLevel MinLevel { get; set; } =
            TraceLevel.Debug;

        /// <inheritdoc />
        public void Trace(HttpRequestMessage request, string category, TraceLevel level,
            Action<TraceRecord> traceAction)
        {
            if (MinLevel > level)
                return;

            var rec = new TraceRecord(request, category, level);
            traceAction(rec);
            var message = $"{rec.Category} {rec.Operator} {rec.Operation} {rec.Message}";
            var entry = new LogEntryDto(DateTime.UtcNow, ConvertLevel(level), message);

            lock (LatestLogEntries)
            {
                LatestLogEntries.AddLast(entry);
                RemoveOldEntries(LatestLogEntries);
            }
        }

        /// <summary>
        ///     Retrieve all entries that pass our filters.
        /// </summary>
        /// <returns></returns>
        public LogEntryDto[] GetEntries()
        {
            lock (LatestLogEntries)
            {
                RemoveOldEntries(LatestLogEntries);
                return LatestLogEntries.ToArray();
            }
        }

        private int ConvertLevel(TraceLevel level)
        {
            switch (level)
            {
                case TraceLevel.Debug:
                    return 1;
                case TraceLevel.Info:
                    return 2;
                case TraceLevel.Warn:
                    return 3;
                case TraceLevel.Error:
                    return 4;
                case TraceLevel.Fatal:
                    return 5;
                default:
                    return 0;
            }
        }

        private void RemoveOldEntries(LinkedList<LogEntryDto> latestLogEntries)
        {
            while (latestLogEntries.Count > MaxEntries) latestLogEntries.RemoveFirst();

            var item = latestLogEntries.First;
            var threshold = DateTime.UtcNow.Subtract(MaxAge);
            while (latestLogEntries.First != null && latestLogEntries.First.Value.TimestampUtc < threshold)
                latestLogEntries.RemoveFirst();
        }
    }
}