using System;

public enum ElType
{
    Type0,
    Type1,
    Type2,
}

public enum ErrorType
{
    Error0,
    Error1,
    Error2
}

class LogItem
{
    public DateTime time;
    public string productionLineId;
    public string elemId;
    public string color;
    public double size;
    public ElType elType;
    public ErrorType errType;
}
