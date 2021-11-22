using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class GroupedLogWriter
{
    private string logFileName;
    private List<LogItem> logItems;
    private int goodItems = 0;

    public GroupedLogWriter(string logFileName)
    {
        this.logFileName = logFileName;

        logItems =
            new List<string>(File.ReadAllLines(logFileName))
            .Skip(1)
            .Select(ParseLine)
            .ToList();
    }

    public void AddLogItem(LogItem logItem) => logItems.Add(logItem);

    public void GoodItemFound()
    {
        goodItems++;
    }

    public void WriteToFile()
    {
        var logItemsGrouped = logItems
            .OrderBy(l => l.errType)
            .ThenBy(l => l.time)
            .Select(LogItemToString);

        var lines = new List<string>(){
                (logItemsGrouped.Count() + goodItems).ToString()
            }
            .Concat(logItemsGrouped);

        File.WriteAllLines(logFileName, lines);
    }

    private LogItem ParseLine(string line)
    {
        string[] values = line.Split(';');
        return new LogItem()
        {
            time = DateTimeOffset
                .FromUnixTimeSeconds(long.Parse(values[0]))
                .DateTime,
            productionLineId = values[1],
            elemId = values[2],
            color = values[3],
            size = int.Parse(values[4]),
            elType = (ElType)int.Parse(values[5]),
            errType = (ErrorType)int.Parse(values[6])

        };
    }

    private string LogItemToString(LogItem logItem)
    {
        return String.Join(';',
            new DateTimeOffset(logItem.time).ToUnixTimeSeconds(),
            logItem.productionLineId,
            logItem.elemId,
            logItem.color,
            logItem.size,
            (int)logItem.elType,
            (int)logItem.errType
        );
    }
}
