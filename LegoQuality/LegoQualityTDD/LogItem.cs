using System;

namespace LegoQualityTDD
{
    public enum ElType
    {
        Type0,
        Type1,
        Type2,
    }

    public enum ErrorType
    {
        NoError,
        Error1,
        Error2,
        Error3
    }

    public class LogItem
    {
        public DateTime time;
        public string productionLineId;
        public string elemId;
        public string color;
        public double size;
        public ElType elType;
        public ErrorType errorType;
    }
}