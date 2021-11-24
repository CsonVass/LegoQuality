using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LegoQualityTDD
{
    public class SummaryCreator
    {
        public void CreateSummaryFromLogFile(string filepath, string targetfilepath)
        {
            string[] lines = GetFileLines(filepath);
            using StreamWriter file = new StreamWriter(targetfilepath, false);

            file.WriteLine($"Total number of elements: {GetNumberOfElements(lines)}");
            file.WriteLine($"Total number of faulty elements: {GetNumberOfFaultyElements(lines)}");
            file.WriteLine($"Fault rate: {GetFaultPercent(lines) * 100.0}%");

            file.WriteLine("\nErrors by code");
            foreach (var item in GetErrorTypes(lines))
            {
                file.WriteLine($"Error code: {item.Key}, number: {item.Value}, " +
                    $"percentage: {(double)item.Value / GetNumberOfFaultyElements(lines) * 100.0}%");
            }
        }

        public string[] GetFileLines(string filepath)
        {
            if (File.Exists(filepath))
                return File.ReadAllLines(filepath);
            throw new FileNotFoundException();
        }

        public int GetNumberOfElements(string[] lines)
        {
            try
            {
                return int.Parse(lines[0]);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public int GetNumberOfFaultyElements(string[] lines)
        {
            if (lines == null || lines.Length == 0)
                return 0;
            return lines.Length - 1;
        }

        public double GetFaultPercent(string[] lines)
        {
            int numOfElements = GetNumberOfElements(lines);
            if (numOfElements == 0)
                return 0;
            double percent = (double)GetNumberOfFaultyElements(lines) / numOfElements;
            if (percent > 1 || percent < 0)
                throw new ArgumentOutOfRangeException();
            return percent;
        }

        public Dictionary<int, int> GetErrorTypes(string[] lines)
        {
            Dictionary<int, int> errorTypes = new Dictionary<int, int>();
            for (int i = 1; i < lines.Length; i++)
            {
                try
                {
                    string[] elements = lines[i].Split(';');
                    int errorCode = int.Parse(elements[6]);
                    if (errorTypes.ContainsKey(errorCode))
                        errorTypes[errorCode]++;
                    else
                        errorTypes.Add(errorCode, 1);
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
            return errorTypes;
        }
    }
}
