using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LegoQuality
{
    class MockInputSource : IInputSource
    {
        private String mockInputFilePath;
        private List<String> mockInputLines;

        public MockInputSource(String mockInputFilePath)
        {
            this.mockInputFilePath = mockInputFilePath;
            this.mockInputLines = new List<string>(File.ReadAllLines(mockInputFilePath)).ToList();
        }
        public string GetNextInput()
        {
            String input = mockInputLines[0];
            mockInputLines.RemoveAt(0);
            return input;
        }

        public bool HasNextInput()
        {
            return mockInputLines.Count > 0;
        }
    }
}
