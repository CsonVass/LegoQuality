using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LegoQualityTDD
{
    public class GroupedLogWriter
    {
        int nofItems;
        private readonly List<LogItem> logItems = new List<LogItem>();

        public IReadOnlyList<LogItem> LogItems =>
            logItems
                .OrderBy(l => l.errorType)
                .ThenBy(l => l.time)
                .ToList();

        public void LoadFromFile(string logFileName)
        {
            var lines = File.ReadAllLines(logFileName);

            nofItems = int.Parse(lines[0]);

            var fileLogItems = lines
                .Skip(1)
                .Select(ParseLogItem);
            logItems.AddRange(fileLogItems);
        }



        public void AddLogItem(LogItem logItem) => logItems.Add(logItem);

        public void Write(Stream fileStream)
        {
            using var streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine(nofItems);
            LogItems.Select(LogItemToString)
                    .ToList()
                    .ForEach(l => streamWriter.WriteLine(l));
        }

        private string LogItemToString(LogItem logItem)
        {
            return string.Join(';',
                new DateTimeOffset(logItem.time).ToUnixTimeSeconds(),
                logItem.productionLineId,
                logItem.elemId,
                logItem.color,
                logItem.size,
                (int)logItem.elType,
                (int)logItem.errorType
            );
        }

        private LogItem ParseLogItem(string line)
        {
            string[] values = line.Split(';');
            return new LogItem()
            {
                time = DateTimeOffset
                    .FromUnixTimeSeconds(long.Parse(values[0]))
                    .LocalDateTime,
                productionLineId = values[1],
                elemId = values[2],
                color = values[3],
                size = int.Parse(values[4]),
                elType = (ElType)int.Parse(values[5]),
                errorType = (ErrorType)int.Parse(values[6])

            };
        }
    }
}
