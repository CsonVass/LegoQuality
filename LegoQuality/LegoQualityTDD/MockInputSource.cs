using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LegoQualityTDD
{
    public class MockInputSource
    {
        private List<String> mockInputLines;
 

        public MockInputSource(String mockInputFilePath)
        {
            this.mockInputLines = new List<string>(File.ReadAllLines(mockInputFilePath)).ToList();
        }
        public string GetNextInput()
        {
            if (this.HasNextInput())
            {
                String input = mockInputLines[0];
                mockInputLines.RemoveAt(0);
                return input;
            }
            else
            {
                throw new Exception();
            }
        }

        public bool HasNextInput()
        {
            return mockInputLines.Count > 0;
        }
    }
}
