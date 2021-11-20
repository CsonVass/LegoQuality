using System.Collections.Generic;
using System.IO;

namespace LegoQuality
{
    class SummaryCreator
    {
        public void CreateSummaryFromLogFile(string filepath, string targetfilepath)
        {
            string[] lines = File.ReadAllLines(filepath);
            using StreamWriter file = new StreamWriter(targetfilepath, false);

            file.WriteLine($"Total number of elements: {GetNumOfElements(lines)}");
            file.WriteLine($"Total number of faulty elements: {GetNumberOfFaultyElements(lines)}");
            file.WriteLine($"Fault rate: {GetFaultPercent(lines) * 100.0}%");

            file.WriteLine("\nErrors by code");
            foreach (var item in GetErrorTypes(lines))
            {
                file.WriteLine($"Error code: {item.Key}, number: {item.Value}, " +
                    $"percentage: {(double) item.Value / GetNumberOfFaultyElements(lines) * 100.0}%");
            }
        }

        private int GetNumOfElements(string[] lines)
        {
            return int.Parse(lines[0]);
        }

        private int GetNumberOfFaultyElements(string[] lines)
        {
            return lines.Length - 1;
        }

        private double GetFaultPercent(string[] lines)
        {
            return (double) GetNumberOfFaultyElements(lines) / GetNumOfElements(lines);
        }

        private Dictionary<int, int> GetErrorTypes(string[] lines)
        {
            Dictionary<int, int> errorTypes = new Dictionary<int, int>();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] elements = lines[i].Split(';');
                int errorCode = int.Parse(elements[6]);
                if (errorTypes.ContainsKey(errorCode))
                    errorTypes[errorCode]++;
                else
                    errorTypes.Add(errorCode, 1);
            }
            return errorTypes;
        }
    }
}
