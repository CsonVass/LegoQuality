using System;
using System.Collections.Generic;
using System.Text;

namespace LegoQuality
{
    interface IInputSource
    {
        public String GetNextInput();
        public Boolean HasNextInput();
    }
}
